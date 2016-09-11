using System.Collections.Generic;
using System.IO;

namespace MWBS.Dictionary
{
    /// <summary>
    /// Dictionary for this app. Reads are thread safe, writes are not.
    /// </summary>
    public class WordDictionary : IWordDictionary
    {
        public WordDictionary()
        {
            List<string> words = DefaultWordDictionary.GetDefaultWords();
            InitDict(words);
        }

        public WordDictionary(List<string> words)
        {
            InitDict(words);
        }

        private void InitDict(IEnumerable<string> words)
        {
            ClearWords();
            foreach (string word in words)
            {
                AddWord(word.ToLower());
            }
        }

        private static readonly HashSet<string> Dictionary = new HashSet<string>();
        
        public void AddWord(string word)
        {
            Dictionary.Add(word);
        }

        public bool Contains(string word)
        {
            return Dictionary.Contains(word.ToLower());
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