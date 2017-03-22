using Sitecore.Configuration;

namespace Sitecore.SharedSource.CognitiveServices.Repositories
{
    public class ApiKeys : IApiKeys
    {
        public virtual string Academic => Settings.GetSetting("CognitiveService.ApiKey.Academic");
        public virtual string BingSpeech => Settings.GetSetting("CognitiveService.ApiKey.BingSpeech");
        public virtual string BingSpellCheck => Settings.GetSetting("CognitiveService.ApiKey.BingSpellCheck");
        public virtual string BingAutosuggest => Settings.GetSetting("CognitiveService.ApiKey.BingAutosuggest");
        public virtual string BingSearch => Settings.GetSetting("CognitiveService.ApiKey.BingSearch");
        public virtual string ComputerVision => Settings.GetSetting("CognitiveService.ApiKey.ComputerVision");
        public virtual string ContentModerator => Settings.GetSetting("CognitiveService.ApiKey.ContentModerator");
        public virtual string ContentModeratorClientId => Settings.GetSetting("CognitiveService.ApiKey.ContentModerator.ClientId");
        public virtual string ContentModeratorPrivateKey => Settings.GetSetting("CognitiveService.ApiKey.ContentModerator.PrivateKey");
        public virtual string EntityLinking => Settings.GetSetting("CognitiveService.ApiKey.EntityLinking");
        public virtual string Emotion => Settings.GetSetting("CognitiveService.ApiKey.Emotion");
        public virtual string Face => Settings.GetSetting("CognitiveService.ApiKey.Face");
        public virtual string LinguisticAnalysis => Settings.GetSetting("CognitiveService.ApiKey.LinguisticAnalysis");
        public virtual string Recommendations => Settings.GetSetting("CognitiveService.ApiKey.Recommendations");
        public virtual string SpeakerRecognition => Settings.GetSetting("CognitiveService.ApiKey.SpeakerRecognition");
        public virtual string TextAnalytics => Settings.GetSetting("CognitiveService.ApiKey.TextAnalytics");
        public virtual string TranslateSpeech => Settings.GetSetting("CognitiveService.ApiKey.TranslateSpeech");
        public virtual string TranslateText => Settings.GetSetting("CognitiveService.ApiKey.TranslateText");
        public virtual string Video => Settings.GetSetting("CognitiveService.ApiKey.Video");
        public virtual string WebLM => Settings.GetSetting("CognitiveService.ApiKey.WebLM");
    }
}
