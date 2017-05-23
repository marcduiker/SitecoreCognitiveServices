using System;
using Microsoft.Extensions.DependencyInjection;
using Sitecore.SharedSource.CognitiveServices.Ole.Intents;

namespace Sitecore.SharedSource.CognitiveServices.Factories.Intents {
    
    public class GreetIntentFactory : IIntentFactory<IGreetIntent> {
        
        protected readonly IServiceProvider Provider;

        public GreetIntentFactory(IServiceProvider provider)
        {
            Provider = provider;
        }

        public virtual IGreetIntent Create() {
            return Provider.GetService<IGreetIntent>();
        }
    }
}