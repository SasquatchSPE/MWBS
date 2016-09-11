using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MWBS.Dictionary
{
    public interface IWordDictionary
    {
        bool Contains(string word);
    }
}