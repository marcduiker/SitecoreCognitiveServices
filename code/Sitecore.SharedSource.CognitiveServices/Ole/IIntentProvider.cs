using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.SharedSource.CognitiveServices.Ole.Intents;

namespace Sitecore.SharedSource.CognitiveServices.Ole {
    public interface IIntentProvider
    {
        Dictionary<string, IIntent> GetAllIntents(Guid appId);

        IIntent GetIntent(Guid appId, string intentName);

        string GetDefaultResponse(Guid appId);
    }
}