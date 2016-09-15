using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MWBS.Models
{
    public class LetterPoint
    {
        public LetterPoint()
        {
            Point = new Point {X = -1, Y = -1};
        }

        public Point Point { get; set; }
        public char? Letter { get; set; }

        public override string ToString()
        {
            return $"{Letter} {Point.X},{Point.Y}";
        }
    }
}