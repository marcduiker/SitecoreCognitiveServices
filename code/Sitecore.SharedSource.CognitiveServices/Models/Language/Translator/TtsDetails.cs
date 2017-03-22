using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitecore.SharedSource.CognitiveServices.Models.Language.Translator {
    public class TtsDetails {
        public string displayName { get; set; }
        public string gender { get; set; }
        public string locale { get; set; }
        public string languageName { get; set; }
        public string regionName { get; set; }
        public string language { get; set; }
    }
}