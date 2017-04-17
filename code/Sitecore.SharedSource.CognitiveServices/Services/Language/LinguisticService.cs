using System;
using System.Threading.Tasks;
using Sitecore.SharedSource.CognitiveServices.Foundation;
using Microsoft.SharedSource.CognitiveServices.Repositories.Language;
using Microsoft.SharedSource.CognitiveServices.Models.Language.Linguistic;

namespace Sitecore.SharedSource.CognitiveServices.Services.Language
{
    public class LinguisticService : ILinguisticService
    {
        protected ILinguisticRepository LinguisticRepository;
        protected ILogWrapper Logger;

        public LinguisticService(
            ILinguisticRepository linguisticRepository,
            ILogWrapper logger) 
        {
            LinguisticRepository = linguisticRepository;
            Logger = logger;
        }

        public virtual POSTagsTextAnalysisResponse GetPOSTagsTextAnalysis(TextAnalysisRequest request)
        {
            try {
                var result = Task.Run(async () => await LinguisticRepository.GetPOSTagsTextAnalysisAsync(request)).Result;

                return result;
            } catch (Exception ex) {
                Logger.Error("SentimentService.GetSentiment failed", this, ex);
            }

            return null;
        }

        public virtual ConstituencyTreeTextAnalysisResponse GetConstituencyTreeTextAnalysis(TextAnalysisRequest request) {
            try {
                var result = Task.Run(async () => await LinguisticRepository.GetConstituencyTreeTextAnalysisAsync(request)).Result;

                return result;
            } catch (Exception ex) {
                Logger.Error("SentimentService.GetSentiment failed", this, ex);
            }

            return null;
        }

        public virtual TokensTextAnalysisResponse GetTokensTextAnalysis(TextAnalysisRequest request) {
            try {
                var result = Task.Run(async () => await LinguisticRepository.GetTokensTextAnalysisAsync(request)).Result;

                return result;
            } catch (Exception ex) {
                Logger.Error("SentimentService.GetSentiment failed", this, ex);
            }

            return null;
        }
    }
}