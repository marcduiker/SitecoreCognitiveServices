using System;
using Microsoft.Extensions.DependencyInjection;
using Sitecore.SharedSource.CognitiveServices.Ole.Intents;

namespace Sitecore.SharedSource.CognitiveServices.Factories.Intents {
    
    public class DefaultIntentFactory : IIntentFactory<IDefaultIntent> {
        
        protected readonly IServiceProvider Provider;

        public DefaultIntentFactory(IServiceProvider provider)
        {
            Provider = provider;
        }

        public virtual IDefaultIntent Create() {
            return Provider.GetService<IDefaultIntent>();
        }
    }
}