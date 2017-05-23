using System;
using System.Collections.Generic;
using System.Linq;
using Sitecore.SharedSource.CognitiveServices.Foundation;
using Microsoft.SharedSource.CognitiveServices.Models.Language.Luis;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.SearchTypes;
using Sitecore.ContentSearch.Security;
using Sitecore.SharedSource.CognitiveServices.Models.Ole;

namespace Sitecore.SharedSource.CognitiveServices.Ole.Intents {

    public interface ILockedItemCountIntent : IIntent { }

    public class LockedItemCountIntent : ILockedItemCountIntent {

        protected readonly ITextTranslator Translator;
        protected readonly IApplicationSettings Settings;

        public Guid ApplicationId => Settings.OleApplicationId;

        public string Name => "locked item count";

        public string Description => "Count your locked items";

        public LockedItemCountIntent(
            ITextTranslator translator, 
            IApplicationSettings settings) {
            Translator = translator;
            Settings = settings;
            }
        
        public string Respond(QueryResult result, ItemContextParameters parameters)
        {
            var items = GetCurrentUserUnlockedItems(parameters.Database);
            
            return $"You have {items.Count} locked items";
        }
        
        protected List<SearchResultItem> GetCurrentUserUnlockedItems(string db)
        {
            var userMod = Sitecore.Context.User.DisplayName.Replace("\\", "").ToLower();

            using (var context = ContentSearchManager.GetIndex($"sitecore_{db}_index").CreateSearchContext(SearchSecurityOptions.DisableSecurityCheck)) {
                return context
                    .GetQueryable<SearchResultItem>()
                    .Where(a => a.LockOwner.Equals(userMod)).ToList();
            }
        }
    }
}