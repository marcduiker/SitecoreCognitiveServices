using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.SharedSource.CognitiveServices.Models.Language.Translator;

namespace Sitecore.SharedSource.CognitiveServices.LaunchDemo.Models {
    public class TranslatorResult {
        public GetLanguageResponse Languages { get; set; }
        public TranslateResponse Translation { get; set; }
    }
}