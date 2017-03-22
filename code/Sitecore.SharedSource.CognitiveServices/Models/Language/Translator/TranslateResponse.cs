using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitecore.SharedSource.CognitiveServices.Models.Language.Translator {
    public class TranslateResponse {
        public string type { get; set; }
        public string id { get; set; }
        public string recognition { get; set; }
        public string translation { get; set; }
        public int audioStreamPosition { get; set; }
        public int audioSizeBytes { get; set; }
        public long audioTimeOffset { get; set; }
        public int audioTimeSize { get; set; }
    }
}