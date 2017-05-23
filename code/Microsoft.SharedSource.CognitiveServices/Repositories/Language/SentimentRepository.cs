using System.Threading.Tasks;
using Microsoft.ProjectOxford.Text.Sentiment;
using Microsoft.ProjectOxford.Video.Contract;
using Newtonsoft.Json;
using Microsoft.SharedSource.CognitiveServices.Models.Language.Sentiment;

namespace Microsoft.SharedSource.CognitiveServices.Repositories.Language
{
    public class SentimentRepository : ISentimentRepository
    {
        protected static readonly string baseUrl = "https://westus.api.cognitive.microsoft.com/text/analytics/v2.0/";
        protected static readonly string keyPhraseUrl = $"{baseUrl}keyPhrases";
        protected static readonly string topicUrl = $"{baseUrl}topics";
        protected static readonly string sentimentUrl = $"{baseUrl}sentiment";

        protected readonly IApiKeys ApiKeys;
        protected readonly IRepositoryClient RepositoryClient;

        public SentimentRepository(
            IApiKeys apiKeys,
            IRepositoryClient repoClient)
        {
            ApiKeys = apiKeys;
            RepositoryClient = repoClient;
        }

        #region Sentiment

        public virtual SentimentResponse GetSentiment(SentimentRequest request)
        {
            var response = RepositoryClient.SendJsonPost(ApiKeys.TextAnalytics, sentimentUrl, JsonConvert.SerializeObject(request));

            return JsonConvert.DeserializeObject<SentimentResponse>(response);
        }

        public virtual async Task<SentimentResponse> GetSentimentAsync(SentimentRequest request)
        {
            var response = await RepositoryClient.SendJsonPostAsync(ApiKeys.TextAnalytics, sentimentUrl, JsonConvert.SerializeObject(request));

            return JsonConvert.DeserializeObject<SentimentResponse>(response);
        }

        #endregion Sentiment

        #region Key Phrase

        public virtual KeyPhraseSentimentResponse GetKeyPhrases(SentimentRequest request)
        {
            var response = RepositoryClient.SendJsonPost(ApiKeys.TextAnalytics, keyPhraseUrl, JsonConvert.SerializeObject(request));

            return JsonConvert.DeserializeObject<KeyPhraseSentimentResponse>(response);
        }

        public virtual async Task<KeyPhraseSentimentResponse> GetKeyPhrasesAsync(SentimentRequest request)
        {
            var response = await RepositoryClient.SendJsonPostAsync(ApiKeys.TextAnalytics, keyPhraseUrl, JsonConvert.SerializeObject(request));

            return JsonConvert.DeserializeObject<KeyPhraseSentimentResponse>(response);
        }

        #endregion Key Phrase

        #region Topics

        public virtual string GetTopics(TopicRequest request) 
        {
            return RepositoryClient.SendOperationPost(ApiKeys.TextAnalytics, topicUrl, JsonConvert.SerializeObject((object)request));
        }

        /// <summary>
        /// This takes in at least 100 documents and returns a url to the operation where you check for the status of the result
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public virtual async Task<string> GetTopicsAsync(TopicRequest request) {
            return await RepositoryClient.SendOperationPostAsync(ApiKeys.TextAnalytics, topicUrl, JsonConvert.SerializeObject((object)request));
        }

        #endregion Topics

        #region Operation

        public virtual OperationResult GetOperation(string operationLocationUrl)
        {
            var response = RepositoryClient.SendJsonPost(ApiKeys.TextAnalytics, operationLocationUrl, "{}");

            return JsonConvert.DeserializeObject<OperationResult>(response);
        }

        /// <summary>
        /// This will return the status and result of the GetTopics operation
        /// </summary>
        /// <param name="operationLocationUrl"></param>
        /// <returns></returns>
        public virtual async Task<OperationResult> GetOperationAsync(string operationLocationUrl)
        {
            var response = await RepositoryClient.SendJsonPostAsync(ApiKeys.TextAnalytics, operationLocationUrl, "{}");

            return JsonConvert.DeserializeObject<OperationResult>(response);
        }

        #endregion Operation
    }
}
