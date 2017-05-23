using System;
using Microsoft.Extensions.DependencyInjection;
using Sitecore.SharedSource.CognitiveServices.Ole.Intents;

namespace Sitecore.SharedSource.CognitiveServices.Factories.Intents {
    
    public class LoggedInUsersIntentFactory : IIntentFactory<ILoggedInUsersIntent> {
        
        protected readonly IServiceProvider Provider;

        public LoggedInUsersIntentFactory(IServiceProvider provider)
        {
            Provider = provider;
        }

        public virtual ILoggedInUsersIntent Create() {
            return Provider.GetService<ILoggedInUsersIntent>();
        }
    }
}