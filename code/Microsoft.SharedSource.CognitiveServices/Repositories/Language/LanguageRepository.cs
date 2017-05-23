using Microsoft.ProjectOxford.Text.Language;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace Microsoft.SharedSource.CognitiveServices.Repositories.Language
{
    public class LanguageRepository : ILanguageRepository
    {
        protected static readonly string languageUrl = "https://westus.api.cognitive.microsoft.com/text/analytics/v2.0/languages";
        protected readonly IRepositoryClient RepositoryClient;
        protected readonly IApiKeys ApiKeys;

        public LanguageRepository(
            IApiKeys apiKeys,
            IRepositoryClient repoClient)
        {
            ApiKeys = apiKeys;
            RepositoryClient = repoClient;
        }

        public virtual string GetLanguageUrl(LanguageRequest request)
        {
            return (request.NumberOfLanguagesToDetect > 1)
                 ? $"{languageUrl}?numberOfLanguagesToDetect={request.NumberOfLanguagesToDetect}"
                 : languageUrl;
        }

        public virtual LanguageResponse GetLanguages(LanguageRequest request)
        {   
            var response = RepositoryClient.SendJsonPost(ApiKeys.TextAnalytics, GetLanguageUrl(request), JsonConvert.SerializeObject(request));

            return JsonConvert.DeserializeObject<LanguageResponse>(response);
        }

        public virtual async Task<LanguageResponse> GetLanguagesAsync(LanguageRequest request)
        {
            var response = await RepositoryClient.SendJsonPostAsync(ApiKeys.TextAnalytics, GetLanguageUrl(request), JsonConvert.SerializeObject(request));

            return JsonConvert.DeserializeObject<LanguageResponse>(response);
        }
    }
}
