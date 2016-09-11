using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MWBS.Models
{
    public class PuzzleSolution
    {
        public PuzzleSolution()
        {
            Solutions = new List<Word[]>();    
        }

        public List<Word[]> Solutions { get; set; } 
    }
}