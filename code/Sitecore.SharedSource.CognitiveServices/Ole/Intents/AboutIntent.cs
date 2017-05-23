using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Sitecore.SharedSource.CognitiveServices.Foundation;
using Microsoft.SharedSource.CognitiveServices.Models.Language.Luis;
using Sitecore.SharedSource.CognitiveServices.Factories.Intents;
using Sitecore.SharedSource.CognitiveServices.Models.Ole;

namespace Sitecore.SharedSource.CognitiveServices.Ole.Intents {

    public interface IAboutIntent : IIntent { }

    public class AboutIntent : IAboutIntent {

        protected readonly ITextTranslator Translator;
        protected readonly IApplicationSettings Settings;
        protected readonly IServiceProvider Provider;

        public Guid ApplicationId => Settings.OleApplicationId;

        public string Name => "about";

        public string Description => "Tell you about my abilities";

        public AboutIntent(
            ITextTranslator translator, 
            IApplicationSettings settings,
            IServiceProvider provider) {
            Translator = translator;
            Settings = settings;
            Provider = provider;
            }
        
        public string Respond(QueryResult result, ItemContextParameters parameters)
        {
            var intents = Provider.GetServices<IIntentFactory<IIntent>>()
                .Select(a => a.Create())
                .Where(g => g.ApplicationId.Equals(ApplicationId) && !g.Name.Equals("none"));
                
            var list = intents.Select(i => $"<li>{i.Description}</li>");

            var str = string.Join("", list);

            return $"Here's the list of things I can do: <br/><ul>{str}</ul>";
        }
    }
}