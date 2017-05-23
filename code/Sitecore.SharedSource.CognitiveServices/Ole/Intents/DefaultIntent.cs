using System;
using Sitecore.SharedSource.CognitiveServices.Foundation;
using Microsoft.SharedSource.CognitiveServices.Models.Language.Luis;
using Sitecore.SharedSource.CognitiveServices.Models.Ole;

namespace Sitecore.SharedSource.CognitiveServices.Ole.Intents {

    public interface IDefaultIntent : IIntent { }

    public class DefaultIntent : IDefaultIntent {

        protected readonly ITextTranslator Translator;
        protected readonly IApplicationSettings Settings;

        public Guid ApplicationId => Settings.OleApplicationId;

        public string Name => "none";

        public string Description => "";

        public DefaultIntent(
            ITextTranslator translator,
            IApplicationSettings settings) {
            Translator = translator;
            Settings = settings;
        }
        
        public string Respond(QueryResult result, ItemContextParameters parameters) {
            return "Sorry, can you try again? I didn't quite understand you.";
        }
    }
}