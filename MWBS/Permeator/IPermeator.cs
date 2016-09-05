using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MWBS.Permeator
{
    public interface IPermeator
    {
        List<string> Permeate(char[] letters);
        List<string> Permeate(char[] letters, int minWordLength, int maxWordLength);
    }
}