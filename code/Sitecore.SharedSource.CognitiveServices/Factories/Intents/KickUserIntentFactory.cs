using System;
using Microsoft.Extensions.DependencyInjection;
using Sitecore.SharedSource.CognitiveServices.Ole.Intents;

namespace Sitecore.SharedSource.CognitiveServices.Factories.Intents {
    
    public class KickUserIntentFactory : IIntentFactory<IKickUserIntent> {
        
        protected readonly IServiceProvider Provider;

        public KickUserIntentFactory(IServiceProvider provider)
        {
            Provider = provider;
        }

        public virtual IKickUserIntent Create() {
            return Provider.GetService<IKickUserIntent>();
        }
    }
}