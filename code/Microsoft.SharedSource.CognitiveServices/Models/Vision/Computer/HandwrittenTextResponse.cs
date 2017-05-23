using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.SharedSource.CognitiveServices.Models.Vision.Computer
{
    public class HandwrittenTextResponse
    {
        public string Status { get; set; }
        public RecognitionResult RecognitionResult { get; set; }
    }
}
