using System;
using Sitecore.SharedSource.CognitiveServices.Foundation;
using Microsoft.SharedSource.CognitiveServices.Models.Language.Luis;
using Sitecore.SharedSource.CognitiveServices.Models.Ole;

namespace Sitecore.SharedSource.CognitiveServices.Ole.Intents {
    public interface IIntent {
        Guid ApplicationId { get; }
        string Name { get; }
        string Description { get; }
        string Respond(QueryResult result, ItemContextParameters parameters);
    }
}