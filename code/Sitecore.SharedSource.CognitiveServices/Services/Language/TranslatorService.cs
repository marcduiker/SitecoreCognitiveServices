using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Sitecore.SharedSource.CognitiveServices.Enums;
using Sitecore.SharedSource.CognitiveServices.Foundation;
using Sitecore.SharedSource.CognitiveServices.Models.Language.Translator;
using Sitecore.SharedSource.CognitiveServices.Repositories.Language;

namespace Sitecore.SharedSource.CognitiveServices.Services.Language {
    public class TranslatorService : ITranslatorService {

        protected ITranslatorRepository TranslatorRepository;
        protected ILogWrapper Logger;

        public TranslatorService(
            ITranslatorRepository translatorRepository,
            ILogWrapper logger) {
            TranslatorRepository = translatorRepository;
            Logger = logger;
        }

        public GetLanguageResponse GetLanguages(IEnumerable<TranslateScopeOptions> scopes = null)
        {
            try
            {
                var result = Task.Run(async () => await TranslatorRepository.GetLanguagesAsync(scopes)).Result;

                return result;
            }
            catch (Exception ex)
            {
                Logger.Error("", this, ex);
            }

            return null;
        }

        public TranslateResponse Translate(
            string from, 
            string to,
            Stream stream,
            TranslateFormatOption format = TranslateFormatOption.wav, 
            TranslateProfanityMarkerOption marker = TranslateProfanityMarkerOption.asterisk, 
            TranslateProfanityActionOption action = TranslateProfanityActionOption.marked, 
            string voice = "", 
            IEnumerable<TranslateFeatureOptions> features = null)
        {
            try
            {
                var result = Task.Run(async () => await TranslatorRepository.TranslateAsync(from, to, stream, format, marker, action, voice, features)).Result;

                return result;
            }
            catch (Exception ex)
            {
                Logger.Error("", this, ex);
            }

            return null;
        }
    }
}