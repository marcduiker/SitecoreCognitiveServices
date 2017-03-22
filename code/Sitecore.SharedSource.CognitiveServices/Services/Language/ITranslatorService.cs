using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Sitecore.SharedSource.CognitiveServices.Enums;
using Sitecore.SharedSource.CognitiveServices.Models.Language.Translator;

namespace Sitecore.SharedSource.CognitiveServices.Services.Language {
    public interface ITranslatorService
    {
        GetLanguageResponse GetLanguages(IEnumerable<TranslateScopeOptions> scopes = null);
        TranslateResponse Translate(
            string from, 
            string to,
            Stream stream,
            TranslateFormatOption format = TranslateFormatOption.wav, 
            TranslateProfanityMarkerOption marker = TranslateProfanityMarkerOption.asterisk, 
            TranslateProfanityActionOption action = TranslateProfanityActionOption.marked, 
            string voice = "", 
            IEnumerable<TranslateFeatureOptions> features = null);
    }
}