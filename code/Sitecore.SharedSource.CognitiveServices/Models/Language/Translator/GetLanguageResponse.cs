using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitecore.SharedSource.CognitiveServices.Models.Language.Translator {
    
    public class GetLanguageResponse {
        public Dictionary<string, SpeechDetails> speech { get; set; }
        public Dictionary<string, TextDetails> text { get; set; }
        public Dictionary<string, TtsDetails> tts { get; set; }
    }
}