using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.ProjectOxford.Emotion.Contract;
using Microsoft.ProjectOxford.EntityLinking.Contract;
using Microsoft.ProjectOxford.Face;
using Microsoft.ProjectOxford.Text.Core;
using Microsoft.ProjectOxford.Text.Language;
using Microsoft.ProjectOxford.Text.Sentiment;
using Microsoft.ProjectOxford.Vision;
using Microsoft.ProjectOxford.Vision.Contract;
using Sitecore.Configuration;
using Sitecore.ContentSearch;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.SharedSource.CognitiveServices.Foundation;
using Sitecore.SharedSource.CognitiveServices.Models.Analysis;
using Microsoft.SharedSource.CognitiveServices.Models.Language.Linguistic;
using Microsoft.SharedSource.CognitiveServices.Repositories.Vision;
using Sitecore.SharedSource.CognitiveServices.Services.Knowledge;
using Sitecore.SharedSource.CognitiveServices.Services.Language;
using Face = Microsoft.ProjectOxford.Face.Contract.Face;

namespace Sitecore.SharedSource.CognitiveServices.Search
{
    public class CognitiveIndexableItem : SitecoreIndexableItem
    {
        public CognitiveIndexableItem(Item item) : base(item)
        {
            var dataService = DependencyResolver.Current.GetService<ISitecoreDataService>();
            if (dataService == null)
            {
                return;
            }

            if (dataService.IsMediaFile(item))
            {
                SetMediaProperties(item);
            }

            if (item.Paths.IsContentItem)
            {
                SetTextProperties(item);
            }
        }

        private void SetTextProperties(Item item)
        {
            SetSentimentInfo(item);
            SetLanguageInfo(item);
            SetLinguisticInfo(item);
            SetEntityLinking(item);
            SetSentitment(item);
        }

        private void SetSentitment(Item item)
        {
            var sentimentService = DependencyResolver.Current.GetService<ISentimentService>();
            if (sentimentService == null)
                return;

            SentimentRequest sr = new SentimentRequest();

            IEnumerable<Field> fields = GetTextualFields(item);
            if (!fields.Any())
            {
                return;
            }

            foreach (Field f in fields)
            {
                sr.Documents.Add(new SentimentDocument()
                {
                    Language = item.Language.CultureInfo.TwoLetterISOLanguageName.ToLower(),
                    Text = GetFormattedString(f.Value, 10240),
                    Id = f.DisplayName
                });
            }

            var result = sentimentService.GetSentiment(sr);
            if (result == null)
                return;

            Sentiment = result;
        }

        private void SetEntityLinking(Item item)
        {
            var entityLinkingService = DependencyResolver.Current.GetService<IEntityLinkingService>();
            if (entityLinkingService == null)
                return;

            List<LinkAnalysisResult> fieldResults = new List<LinkAnalysisResult>();
            IEnumerable<Field> fields = GetTextualFields(item);
            if (!fields.Any())
            {
                return;
            }

            foreach (Field f in fields)
            {
                string value = Regex.Replace(f.Value, "<.*?>", string.Empty);
                var result = entityLinkingService.Link(value) ?? new EntityLink[0];

                fieldResults.Add(
                    new LinkAnalysisResult()
                    {
                        EntityAnalysis = result,
                        FieldName = f.DisplayName,
                        FieldValue = value
                    });
            }

            EntityLinking = fieldResults;
        }

        private void SetLinguisticInfo(Item item)
        {
            var linguisticService = DependencyResolver.Current.GetService<ILinguisticService>();
            if (linguisticService == null)
                return;

            List<LinguisticAnalysisResult> fieldResults = new List<LinguisticAnalysisResult>();
            IEnumerable<Field> fields = GetTextualFields(item);
            if (!fields.Any())
            {
                return;
            }

            foreach (Field f in fields)
            {
                string value = RemoveHtmlMarkup(f.Value);
                TextAnalysisRequest tar = new TextAnalysisRequest()
                {
                    Language = item.Language.Name,
                    Text = value
                };

                var result1 = linguisticService.GetPOSTagsTextAnalysis(tar) ?? new POSTagsTextAnalysisResponse();
                var result2 = linguisticService.GetConstituencyTreeTextAnalysis(tar) ?? new ConstituencyTreeTextAnalysisResponse();

                fieldResults.Add(new LinguisticAnalysisResult()
                {
                    FieldName = f.DisplayName,
                    FieldValue = value,
                    POSTagsAnalysis = result1,
                    ConstituencyTreeAnalysis = result2,
                    TokensAnalysis = null
                });
            }

            Linguistics = fieldResults;
        }

        private void SetLanguageInfo(Item item)
        {
            var languageService = DependencyResolver.Current.GetService<ILanguageService>();
            if (languageService == null)
                return;

            LanguageRequest lr = new LanguageRequest();

            IEnumerable<Field> fields = GetTextualFields(item);
            if (!fields.Any())
            {
                return;
            }

            foreach (Field f in fields)
            {
                lr.Documents.Add(new Document()
                {
                    Text = GetFormattedString(f.Value, 10240),
                    Id = f.DisplayName
                });
            }

            Languages = languageService.GetLanguages(lr);
        }

        private void SetSentimentInfo(Item item)
        {
            var sentimentService = DependencyResolver.Current.GetService<ISentimentService>();
            if (sentimentService == null)
                return;

            SentimentRequest sr = new SentimentRequest();

            Dictionary<string, Field> fields = GetTextualFields(item).ToDictionary(a => a.DisplayName);
            if (!fields.Any())
            {
                return;
            }

            foreach (var f in fields)
            {
                sr.Documents.Add(new SentimentDocument()
                {
                    Language = item.Language.CultureInfo.TwoLetterISOLanguageName.ToLower(),
                    Text = GetFormattedString(f.Value.Value, 10240),
                    Id = f.Value.DisplayName
                });
            }


            var result = sentimentService.GetKeyPhrases(sr);
            if (result == null)
                return;

            KeyPhraseAnalysis = result
                .Documents
                .Select(d => new KeyPhraseAnalysisResult()
                {
                    FieldName = d.Id,
                    FieldValue = fields[d.Id].Value,
                    KeyPhraseAnalysis = d
                })
                .ToList();
        }

        private string RemoveHtmlMarkup(string value)
        {
            return Regex.Replace(value, "<.*?>", string.Empty);
        }

        private string GetFormattedString(string value, int sizeLimit)
        {
            string plainString = RemoveHtmlMarkup(value);

            var contentSize = Encoding.Unicode.GetByteCount(plainString);
            if (contentSize < sizeLimit)
                return plainString;

            string[] sentences = plainString.Split(new[] { "." }, StringSplitOptions.RemoveEmptyEntries);
            StringBuilder sb = new StringBuilder();
            foreach (string s in sentences)
            {
                if (Encoding.Unicode.GetByteCount($"{sb} {s}.") > sizeLimit)
                    break;

                sb.Append(sb.Length == 0 ? $"{s}." : $" {s}");
            }

            return sb.ToString();
        }

        private void SetMediaProperties(Item item)
        {
            MediaItem m = item;
            if (!MeetsRestrictions(m))
            {
                return;
            }

            try
            {
                var emotionRepository = DependencyResolver.Current.GetService<IEmotionRepository>();
                if (emotionRepository != null)
                {
                    Emotions = Task.Run(async () => await emotionRepository.RecognizeAsync(m.GetMediaStream())).Result;
                }

                var faceRepository = DependencyResolver.Current.GetService<IFaceRepository>();
                if (faceRepository != null)
                {
                    Faces = Task.Run(async () => await faceRepository.DetectAsync(m.GetMediaStream(), true, true, new List<FaceAttributeType>()
                    {
                        FaceAttributeType.Age,
                        FaceAttributeType.FacialHair,
                        FaceAttributeType.Gender,
                        FaceAttributeType.Glasses,
                        FaceAttributeType.HeadPose,
                        FaceAttributeType.Smile
                    })).Result;
                }

                var visionRepository = DependencyResolver.Current.GetService<IVisionRepository>();
                if (visionRepository != null)
                {

                    Visions = Task.Run(async () => await visionRepository.AnalyzeImageAsync(m.GetMediaStream(), new List<VisualFeature>() {
                        VisualFeature.Adult,
                        VisualFeature.Categories,
                        VisualFeature.Color,
                        VisualFeature.Description,
                        VisualFeature.Faces,
                        VisualFeature.ImageType,
                        VisualFeature.Tags
                    })).Result;

                    Text = Task.Run(async () => await visionRepository.RecognizeTextAsync(m.GetMediaStream(), "en", true)).Result;
                }
            }
            catch (Exception ex)
            {
                Log.Error(String.Format("Item: {0}, Message: {1}", item.ID, ex.Message), ex);
            }
        }

        /// <summary>
        /// This will get all text based content fields (as opposed to system fields) from the provided item
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public virtual IEnumerable<Field> GetTextualFields(Item item)
        {
            IEnumerable<Field> fields = item.Fields
                    .Where(f => !f.Name.StartsWith("__") && GetTextualFieldTypes().Contains(f.Type));

            return fields;
        }

        private static string[] GetTextualFieldTypes()
        {
            return Settings.GetSetting("CognitiveService.TextualFieldTypes").Split('|');
        }

        private bool MeetsRestrictions(MediaItem mediaItem)
        {
            int height = Int32.Parse(mediaItem.InnerItem["Height"]);
            int width = Int32.Parse(mediaItem.InnerItem["Width"]);

            if (height <= 36 || height > 4096)
            {
                return false;
            }

            if (width <= 36 || width > 4096)
            {
                return false;
            }

            List<string> acceptableFormats = new List<string>() { "jpg", "jpeg", "png", "gif", "bmp" };
            if (!acceptableFormats.Contains(mediaItem.Extension.ToLower()))
            {
                return false;
            }

            return true;
        }

        public OcrResults Text { get; set; }

        public AnalysisResult Visions { get; set; }

        public Face[] Faces { get; set; }

        public Emotion[] Emotions { get; set; }

        public List<KeyPhraseAnalysisResult> KeyPhraseAnalysis { get; set; }

        public LanguageResponse Languages { get; set; }

        public List<LinguisticAnalysisResult> Linguistics { get; set; }

        public List<LinkAnalysisResult> EntityLinking { get; set; }

        public SentimentResponse Sentiment { get; set; }

    }
}