using System;
using Microsoft.Extensions.DependencyInjection;
using Sitecore.SharedSource.CognitiveServices.Ole.Intents;

namespace Sitecore.SharedSource.CognitiveServices.Factories.Intents {
    
    public class LockedItemCountIntentFactory : IIntentFactory<ILockedItemCountIntent> {
        
        protected readonly IServiceProvider Provider;

        public LockedItemCountIntentFactory(IServiceProvider provider)
        {
            Provider = provider;
        }

        public virtual ILockedItemCountIntent Create() {
            return Provider.GetService<ILockedItemCountIntent>();
        }
    }
}