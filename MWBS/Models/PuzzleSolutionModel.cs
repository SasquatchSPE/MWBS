using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MWBS.Models
{
    public class PuzzleSolutionModel
    {
        public string ErrorMessage { get; set; }
        public List<List<string>> Words { get; set; }
        public List<string> SolutionList => Words.Select(w => string.Join(",  ", w)).ToList();
    }
}