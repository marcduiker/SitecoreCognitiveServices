using Microsoft.ProjectOxford.Text.Core;
using Newtonsoft.Json;
using Sitecore.SharedSource.CognitiveServices.Enums;
using Sitecore.SharedSource.CognitiveServices.Foundation;
using Sitecore.SharedSource.CognitiveServices.Models;
using Sitecore.SharedSource.CognitiveServices.Models.Language.Translator;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json.Linq;
using System.Threading;
using Sitecore.Diagnostics;
using Debug = Sitecore.Diagnostics.Debug;
using System.Net;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

namespace Sitecore.SharedSource.CognitiveServices.Repositories.Language {
    public class TranslatorRepository : TextClient, ITranslatorRepository {
        //https://github.com/MicrosoftTranslator/Csharp-Cmd-Line-Speech-Translate
        //http://docs.microsofttranslator.com/speech-translate.html#!/Speech_translation/SpeechTranslate_Get
        //http://docs.microsofttranslator.com/text-translate.html

        protected static readonly string translateBase = "://dev.microsofttranslator.com/";
        protected static readonly string translateSocketUrl = $"wss{translateBase}";
        protected static readonly string translateUrl = $"https{translateBase}";

        protected static readonly string translateSpeechSessionTokenKey = "TranslateSpeechSessionTokenKey";
        protected static readonly string translateTextSessionTokenKey = "TranslateTextSessionTokenKey";

        protected readonly IApiKeys ApiKeys;
        protected readonly IRepositoryClient RepositoryClient;
        protected readonly ILogWrapper Logger;

        protected HttpContextBase Context { get; set; }

        public TranslatorRepository(
            IApiKeys apiKeys,
            IRepositoryClient repoClient, 
            HttpContextBase context,
            ILogWrapper logger) : base(apiKeys.ContentModerator) {
            ApiKeys = apiKeys;
            RepositoryClient = repoClient;
            Context = context;
            Logger = logger;
        }

        #region Web Widget



        #endregion Web Widget

        #region Text Translation

        //public virtual string BreakSentencesMethod(string text) { 
        //    try {
        //        var result = Task.Run(async () => await BreakSentencesMethodAsync(text)).Result;

        //        return JsonConvert.DeserializeObject<string>(result);
        //    } catch (WebException e)
        //    {
        //        Logger.Error("Failure", this, e);
        //    }

        //    return null;
        //}

        //public virtual async Task<List<int>> BreakSentencesMethodAsync(string text)
        //{
        //    var token = RepositoryClient.SendTranslationTokenRequest(ApiKeys.TranslateText);
        //    var url = $"http://api.microsofttranslator.com/v2/Http.svc/BreakSentences?text={text}&language=en";

        //    var response = await RepositoryClient.SendAsync(ApiKey, url, "", "", "GET", token);

        //    //return JsonConvert.DeserializeObject<string>(response);

        //    MemoryStream stream = new MemoryStream();
        //    StreamWriter writer = new StreamWriter(stream);
        //    writer.Write(response);
        //    writer.Flush();
        //    stream.Position = 0;

        //    DataContractSerializer dcs = new DataContractSerializer(typeof(List<int>));
        //    return (List<int>)dcs.ReadObject(stream);
        //}

        //private static void AddTranslationArrayMethod(string authToken) {
        //    string appId = "";
        //    string uri = "http://api.microsofttranslator.com/v2/Http.svc/AddTranslationArray";
        //    string originalText1 = "una importante contribución a la rentabilidad de la empresa";
        //    string translatedText1 = "a significant contribution tothe company profitability";
        //    string originalText2 = "a veces los errores son divertidos";
        //    string translatedText2 = "in some cases errors are fun";


        //    string body = GenerateAddtranslationRequestBody(appId, "es", "en", "general", "text/plain", "", "TestUserId");
        //    string translationsCollection = string.Format("{0}{1}",
        //        GenerateAddtranslationRequestElement(originalText1, 8, 0, translatedText1),
        //        GenerateAddtranslationRequestElement(originalText2, 6, 0, translatedText2));

        //    // update the body
        //    string requestBody = string.Format(body, translationsCollection);

        //    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
        //    request.ContentType = "text/xml";
        //    request.Method = "POST";
        //    request.Headers.Add("Authorization", authToken);
        //    using (System.IO.Stream stream = request.GetRequestStream()) {

        //        byte[] arrBytes = System.Text.ASCIIEncoding.ASCII.GetBytes(requestBody);
        //        stream.Write(arrBytes, 0, arrBytes.Length);
        //    }

        //    // get the response
        //    WebResponse response = null;
        //    try {
        //        response = request.GetResponse();
        //        using (Stream respStream = response.GetResponseStream()) {
        //            Console.WriteLine(string.Format("Your translations for '{0}' and '{1}' has been added successfully.", originalText1, originalText2));
        //        }
        //    } catch {
        //        throw;
        //    } finally {
        //        if (response != null) {
        //            response.Close();
        //            response = null;
        //        }
        //    }
        //    Console.WriteLine("Press any key to continue...");
        //    Console.ReadKey(true);

        //}

        //private static string GenerateAddtranslationRequestBody(string appId, string from, string to, string category, string contentType, string uri, string user) {
        //    string body = "<AddtranslationsRequest>" +
        //                     "<AppId>{0}</AppId>" +
        //                     "<From>{1}</From>" +
        //                     "<Options>" +
        //                       "<Category xmlns=\"http://schemas.datacontract.org/2004/07/Microsoft.MT.Web.Service.V2\">{2}</Category>" +
        //                       "<ContentType xmlns=\"http://schemas.datacontract.org/2004/07/Microsoft.MT.Web.Service.V2\">{3}</ContentType>" +
        //                       "<User xmlns=\"http://schemas.datacontract.org/2004/07/Microsoft.MT.Web.Service.V2\">{4}</User>" +
        //                       "<Uri xmlns=\"http://schemas.datacontract.org/2004/07/Microsoft.MT.Web.Service.V2\">{5}</Uri>" +
        //                     "</Options>" +
        //                     "<To>{6}</To>" +
        //                     "<Translations>{7}</Translations>" +
        //                   "</AddtranslationsRequest>";
        //    return string.Format(body, appId, from, category, contentType, user, uri, to, "{0}");



        //}

        //private static string GenerateAddtranslationRequestElement(string originalText, int rating, int sequence, string translatedText) {
        //    string element = "<Translation xmlns=\"http://schemas.datacontract.org/2004/07/Microsoft.MT.Web.Service.V2\">" +
        //        "<OriginalText>{0}</OriginalText>" +
        //        "<Rating>{1}</Rating>" +
        //        "<TranslatedText>{2}</TranslatedText>" +
        //        "<Sequence>{3}</Sequence>" +
        //        "</Translation>";
        //    return string.Format(element, originalText, rating.ToString(), translatedText, sequence.ToString());
        //}


        #endregion Text Translation

        #region Speech Translation
        
        public async Task<GetLanguageResponse> GetLanguagesAsync(IEnumerable<TranslateScopeOptions> scopes = null) {

            StringBuilder sb = new StringBuilder();
            sb.Append("?api-version=1.0");

            if (scopes != null)
                sb.Append($"&scope={string.Join(",", scopes)}");

            var response = await SendGetAsync($"{translateUrl}languages{sb}");

            return JsonConvert.DeserializeObject<GetLanguageResponse>(response);
        }
        
        public async Task<TranslateResponse> TranslateAsync(
            string from, 
            string to, 
            Stream stream,
            TranslateFormatOption format = TranslateFormatOption.wav, 
            TranslateProfanityMarkerOption marker = TranslateProfanityMarkerOption.asterisk, 
            TranslateProfanityActionOption action = TranslateProfanityActionOption.marked, 
            string voice = "", 
            IEnumerable<TranslateFeatureOptions> features = null) {

            StringBuilder sb = new StringBuilder();
            sb.Append($"?api-version=1.0&from={from}&to={to}&format=audio/{format}&profanitymarker={marker}&profanityaction={action}");
            
            if (features != null)
                sb.Append($"&scope={string.Join(",", features)}");

            if (!string.IsNullOrEmpty(voice))
                sb.Append($"&voice={voice}");
            
            var token = GetTranslateSpeechToken();

            var uri = new Uri($"{translateUrl}languages{sb}");
            
            var response = await RepositoryClient.SendAsync(ApiKeys.TranslateSpeech, $"{translateUrl}speech/translate{sb}", RepositoryClient.GetStreamString(stream), RepositoryClient.GetImageStreamContentType(stream), "GET", token);

            return JsonConvert.DeserializeObject<TranslateResponse>(response);
        }

        public static async Task SendData() {
            HttpListener s = new HttpListener();
            s.Prefixes.Add("http://localhost:8000/ws");
            s.Start();

            var hc = await s.GetContextAsync();
            if (!hc.Request.IsWebSocketRequest) {
                hc.Response.StatusCode = 400;
                hc.Response.Close();
                return;
            }
            var wsc = await hc.AcceptWebSocketAsync(null);
            var ws = wsc.WebSocket;

            for (int i = 0; i != 10; ++i) {
                // await Task.Delay(20);
                var time = DateTime.Now.ToLongTimeString();
                var buffer = Encoding.UTF8.GetBytes(time);
                var segment = new ArraySegment<byte>(buffer);
                await ws.SendAsync(segment, System.Net.WebSockets.WebSocketMessageType.Text, true, CancellationToken.None);
            }
            await ws.CloseAsync(WebSocketCloseStatus.NormalClosure, "Done", CancellationToken.None);
        }

        public async Task ReceiveData() {
            ClientWebSocket ws = new ClientWebSocket();
            var uri = new Uri("ws://localhost:8000/ws/");

            await ws.ConnectAsync(uri, CancellationToken.None);
            var buffer = new byte[1024];
            while (true) {
                var segment = new ArraySegment<byte>(buffer);

                var result = await ws.ReceiveAsync(segment, CancellationToken.None);

                if (result.MessageType == WebSocketMessageType.Close) {
                    await ws.CloseAsync(WebSocketCloseStatus.InvalidMessageType, "I don't do binary", CancellationToken.None);
                    return;
                }

                int count = result.Count;
                while (!result.EndOfMessage) {
                    if (count >= buffer.Length) {
                        await ws.CloseAsync(WebSocketCloseStatus.InvalidPayloadData, "That's too long", CancellationToken.None);
                        return;
                    }

                    segment = new ArraySegment<byte>(buffer, count, buffer.Length - count);
                    result = await ws.ReceiveAsync(segment, CancellationToken.None);
                    count += result.Count;
                }

                var message = Encoding.UTF8.GetString(buffer, 0, count);
                Console.WriteLine(">" + message);
            }

        }

        #endregion Speech Translation

        #region Tokens

        protected virtual string GetTranslateSpeechToken() {
            return GetToken(ApiKeys.TranslateSpeech, translateSpeechSessionTokenKey);
        }

        protected virtual string GetTranslateTextToken() {
            return GetToken(ApiKeys.TranslateText, translateTextSessionTokenKey);
        }

        protected virtual string GetToken(string apiKey, string sessionKey) {
            if (Context?.Session?[sessionKey] != null) {
                var sessionToken = (TokenResponse)Context.Session[sessionKey];
                if (sessionToken.Expires_On != null && sessionToken.ExpirationDate >= DateTime.Now)
                    return sessionToken.Access_Token;
            }

            var tokenValue = RepositoryClient.SendTranslationTokenRequest(apiKey);
            TimeSpan expiry = new TimeSpan(DateTime.Now.AddMinutes(10).Ticks);
            TimeSpan unix = new TimeSpan(new DateTime(1970, 1, 1).Ticks);
            var token = new TokenResponse() { Access_Token = tokenValue, Expires_On = (expiry - unix).TotalSeconds.ToString() };
            Context.Session.Add(sessionKey, token);

            return token.Access_Token;
        }

        #endregion Tokens
    }
}