using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.SharedSource.CognitiveServices.Models.Vision.Computer
{
    public class Line
    {
        public int[] BoundingBox { get; set; }
        public string Text { get; set; }
        public Word[] Words { get; set; }
    }
}
