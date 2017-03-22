using System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitecore.SharedSource.CognitiveServices.Enums
{
    public enum WebLMModelOptions { title, anchor, query, body }
    public enum TranslateScopeOptions { speech, text, tts }
    public enum TranslateFeatureOptions { texttospeech, partial, timinginfo }
    public enum TranslateProfanityMarkerOption { asterisk, tag }
    public enum TranslateProfanityActionOption { noaction, marked, deleted }
    public enum TranslateFormatOption { wav, mp3 }
}