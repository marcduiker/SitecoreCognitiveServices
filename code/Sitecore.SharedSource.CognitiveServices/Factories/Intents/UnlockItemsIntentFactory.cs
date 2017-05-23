using System;
using Microsoft.Extensions.DependencyInjection;
using Sitecore.SharedSource.CognitiveServices.Ole.Intents;

namespace Sitecore.SharedSource.CognitiveServices.Factories.Intents {
    
    public class UnlockItemsIntentFactory : IIntentFactory<IUnlockItemsIntent> {
        
        protected readonly IServiceProvider Provider;

        public UnlockItemsIntentFactory(IServiceProvider provider)
        {
            Provider = provider;
        }

        public virtual IUnlockItemsIntent Create() {
            return Provider.GetService<IUnlockItemsIntent>();
        }
    }
}