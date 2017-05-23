using System;
using Microsoft.Extensions.DependencyInjection;
using Sitecore.SharedSource.CognitiveServices.Ole.Intents;

namespace Sitecore.SharedSource.CognitiveServices.Factories.Intents {
   
    public class AboutIntentFactory : IIntentFactory<IAboutIntent> {
        
        protected readonly IServiceProvider Provider;

        public AboutIntentFactory(IServiceProvider provider)
        {
            Provider = provider;
        }

        public virtual IAboutIntent Create() {
            return Provider.GetService<IAboutIntent>();
        }
    }
}