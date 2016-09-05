using System.Collections.Generic;

namespace MWBS.Dictionary
{
    /// <summary>
    /// Dictionary for this app. Reads are thread safe, writes are not.
    /// </summary>
    public class WordDictionary : IWordDictionary
    {
        private static readonly HashSet<string> Dictionary = new HashSet<string>();
        
        public void AddWord(string word)
        {
            Dictionary.Add(word);
        }

        public bool Contains(string word)
        {
            return Dictionary.Contains(word);
        }

        public void RemoveWord(string word)
        {
            Dictionary.Remove(word);   
        }

        public void ClearWords()
        {
            Dictionary.Clear();
        }
    }
}