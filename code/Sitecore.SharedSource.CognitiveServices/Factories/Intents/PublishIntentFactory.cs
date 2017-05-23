using System;
using Microsoft.Extensions.DependencyInjection;
using Sitecore.SharedSource.CognitiveServices.Ole.Intents;

namespace Sitecore.SharedSource.CognitiveServices.Factories.Intents {
    
    public class PublishIntentFactory : IIntentFactory<IPublishIntent> {
        
        protected readonly IServiceProvider Provider;

        public PublishIntentFactory(IServiceProvider provider)
        {
            Provider = provider;
        }

        public virtual IPublishIntent Create() {
            return Provider.GetService<IPublishIntent>();
        }
    }
}