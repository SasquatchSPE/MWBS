using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MWBS.Dictionary
{
    public interface IWordDictionary
    {
        void AddWord(string word);
        bool Contains(string word);
        void RemoveWord(string word);
        void ClearWords();
    }
}