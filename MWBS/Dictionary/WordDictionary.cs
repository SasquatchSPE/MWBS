using System.Collections.Generic;
using System.IO;
using System.Linq;

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
                int wordLength = word.Length;
                if (!WordLengthMap.ContainsKey(wordLength))
                {
                    WordLengthMap.Add(wordLength, new HashSet<string>());
                }

                WordLengthMap[wordLength].Add(word);
                AddWord(word.ToLower());
            }
        }

        private static readonly HashSet<string> Dictionary = new HashSet<string>();
        private static readonly Dictionary<int, HashSet<string>> WordLengthMap = new Dictionary<int, HashSet<string>>();
        
        public void AddWord(string word)
        {
            Dictionary.Add(word);
        }

        public bool Contains(string word)
        {
            return Dictionary.Contains(word.ToLower());
        }

        public IEnumerable<string> GetWordsOfLength(int length)
        {
            return WordLengthMap.ContainsKey(length) ? WordLengthMap[length] : Enumerable.Empty<string>();
        }

        public void RemoveWord(string word)
        {
            Dictionary.Remove(word);   
        }

        public void ClearWords()
        {
            WordLengthMap.Clear();
            Dictionary.Clear();
        }
    }
}