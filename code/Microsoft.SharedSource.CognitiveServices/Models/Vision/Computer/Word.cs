using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.SharedSource.CognitiveServices.Models.Vision.Computer
{
    public class Word
    {
        public int[] BoundingBox { get; set; }
        public string Text { get; set; }
    }
}
