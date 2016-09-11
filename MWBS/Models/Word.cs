using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MWBS.Models
{
    public class Word
    {
        public List<LetterPoint> LetterPoints { get; set; }

        public string Letters => string.Join("", LetterPoints.Select(lp => lp.Letter ?? '-').ToArray());

        public override string ToString()
        {
            return Letters;
        }
    }
}