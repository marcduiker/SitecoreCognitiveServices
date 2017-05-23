using System;
using System.Linq;
using Sitecore.SharedSource.CognitiveServices.Foundation;
using Microsoft.SharedSource.CognitiveServices.Models.Language.Luis;
using Sitecore.Data.Items;
using Sitecore.SharedSource.CognitiveServices.Models.Ole;

namespace Sitecore.SharedSource.CognitiveServices.Ole.Intents {

    public interface IPublishIntent : IIntent { }

    public class PublishIntent : IPublishIntent
    {
        protected readonly ITextTranslator Translator;
        protected readonly IApplicationSettings Settings;
        protected readonly ISitecoreDataService DataService;
        protected readonly IPublishWrapper PublishWrapper;

        public Guid ApplicationId => Settings.OleApplicationId;

        public string Name => "publish";

        public string Description => "Publish content";

        public PublishIntent(
            ITextTranslator translator,
            IApplicationSettings settings,
            ISitecoreDataService dataService,
            IPublishWrapper publishWrapper)
        {
            Translator = translator;
            Settings = settings;
            DataService = dataService;
            PublishWrapper = publishWrapper;
        }

        public string Respond(QueryResult result, ItemContextParameters parameters) {
            
            var entities = result?.Entities;
            if (entities == null)
                return "You need to tell me more about what to publish. For example, the 'to' database name, the item path and optionally if you want it recursively.";

            var dbName = entities.FirstOrDefault(x => x.Type.Equals("Database Name"))?.Entity;
            if (string.IsNullOrEmpty(dbName))
                return "Sorry, I think you forgot to mention the database name you wanted to publish to.";

            var toDb = DataService.GetDatabase(dbName);
            if (toDb == null)
                return "Sorry, I couldn't find that database.";

            var itemPath = entities.FirstOrDefault(x => x.Type.Equals("Item Path"))?.Entity;
            if (string.IsNullOrEmpty(itemPath))
                return "Sorry, I think you forgot to mention the root item path.";

            var recursively = entities.FirstOrDefault(x => x.Type.Equals("Recursion"))?.Entity;
            var isRecursive = !string.IsNullOrEmpty(recursively);

            var fromDb = DataService.GetDatabase(parameters.Database);
            Item item = fromDb.GetItem(itemPath);
            if (item == null)
                return $"Sorry, I couldn't find the item at {itemPath}. Are you sure that's the right path?";

            PublishWrapper.PublishItem(item, new[] { toDb }, new[] { item.Language }, isRecursive, false);

            return $"I've published {item.DisplayName} to the {dbName} database in {item.Language.Name} {(isRecursive ? " with it's children" : string.Empty)}";
        }
    }
}