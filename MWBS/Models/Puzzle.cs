using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MWBS.Models
{
    public class Puzzle
    {
        public Board Board { get; set; }
        public List<int> WordLengths { get; set; }
    }
}