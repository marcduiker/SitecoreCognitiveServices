using Sitecore.SharedSource.CognitiveServices.Ole.Intents;

namespace Sitecore.SharedSource.CognitiveServices.Factories.Intents {
    public interface IIntentFactory<out T> where T : IIntent {
        T Create();
    }
}