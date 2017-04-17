using System;
using System.Threading.Tasks;
using Microsoft.ProjectOxford.Text.Language;
using Sitecore.SharedSource.CognitiveServices.Foundation;
using Microsoft.SharedSource.CognitiveServices.Repositories.Language;

namespace Sitecore.SharedSource.CognitiveServices.Services.Language
{
    public class LanguageService : ILanguageService
    {
        protected ILanguageRepository LanguageRepository;
        protected ILogWrapper Logger;

        public LanguageService(
            ILanguageRepository languageRepository,
            ILogWrapper logger) 
        {
            LanguageRepository = languageRepository;
            Logger = logger;
        }

        public virtual LanguageResponse GetLanguages(LanguageRequest request)
        {
            try {
                var result = Task.Run(async () => await LanguageRepository.GetLanguagesAsync(request)).Result;
                
                return result;
            } catch (Exception ex) {
                Logger.Error("LanguageService.GetLanguages failed", this, ex);
            }

            return null;
        }
    }
}