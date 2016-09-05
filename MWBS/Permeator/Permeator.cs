using System;
using System.Collections.Generic;
using System.Linq;

namespace MWBS.Permeator
{
    public class Permeator : IPermeator
    {
        private readonly List<IncludeMask> _maskList;
        public Permeator(int maxWordLength)
        {
            _maskList = GenerateMaskList(maxWordLength);
        }

        public List<string> Permeate(char[] letters)
        {
            return Permeate(letters, 2, letters.Length);
        }

        public List<string> Permeate(char[] letters, int minWordLength, int maxWordLength)
        {
            int letterCount = letters.Length;
            var allPosibilities = new List<string>();
            for (int wordLength = minWordLength; wordLength <= maxWordLength; wordLength++)
            {
                List<int> includeInputs = GetIncludeMasks(letterCount, wordLength);
                foreach (int includeInput in includeInputs)
                {
                    string input = GetInput(letters, includeInput);
                    List<string> combos = GetAllCombos(input.ToCharArray()).Select(c => new string(c.ToArray())).Distinct().ToList();
                    allPosibilities.AddRange(combos);
                }
            }

            return allPosibilities.Distinct().ToList();
        }

        public string GetInput(char[] letters, int includeInput)
        {
            int letterCount = letters.Length;
            var word = new List<char>();
            
            // For 'asdf', 'a' is at the 0 index but the 4th bit of the include input
            // indicates whether it should be included, hence the logic below
            for (int index = letterCount; index > 0; index--)
            {
                if (IsIndexSet(includeInput, index))
                {
                    word.Add(letters[letterCount-index]);
                }
            }

            return new string(word.ToArray());
        }

        private bool IsIndexSet(int input, int bitIndex)
        {
            bool isSet = (input & (1 << bitIndex - 1)) != 0;
            return isSet;
        }

        public List<int> GetIncludeMasks(int length, int numberOfTruths)
        {
            var includeInput = new bool[length];
            for (int index = 0; index < numberOfTruths; index++)
            {
                includeInput[index] = true;
            }

            List<int> includeInputs =
                _maskList.Where(ml => ml.MinWordLength <= length && ml.NumberOfTruths == numberOfTruths)
                    .Select(ml => ml.Mask)
                    .ToList();
            return includeInputs;
        }

        /// <summary>
        /// Warning results contain duplicates
        /// </summary>
        /// <param name="combo"></param>
        /// <returns></returns>
        public List<List<T>> GetAllCombos<T>(T[] combo)
        {
            if (combo.Length == 0)
            {
                return new List<List<T>>();
            }

            if (combo.Length == 1)
            {
                return new List<List<T>> { new List<T> {combo[0]} };
            }

            var allCombos = new List<List<T>>();
            for (int index = 0; index < combo.Length; index++)
            {
                T firstItem = combo[index];
                T[] remains = GetAllButIndex(combo, index);
                List<List<T>> combos = GetAllCombos(remains).ToList();
                combos.ForEach(c => c.Insert(0, firstItem));
                allCombos.AddRange(combos);  
            }

            return allCombos;
        }

        public T[] GetAllButIndex<T>(T[] letters, int skipIndex)
        {
            int length = letters.Length;
            T[] remains = new T[length - 1];

            bool skipped = false;
            for (int index = 0; index < length; index++)
            {
                if (index == skipIndex)
                {
                    skipped = true;
                    continue;
                }

                remains[index - (skipped ? 1 : 0)] = letters[index];
            }

            return remains;
        }

        private static List<IncludeMask> GenerateMaskList(int wordLength)
        {
            double maxMask = Math.Pow(2, wordLength) - 1;
            List<IncludeMask> masks = new List<IncludeMask>();
            for (int number = 0; number <= maxMask; number++)
            {
                string binaryString = Convert.ToString(number, 2);
                int numOfTruths = binaryString.Count(bs => bs == '1');
                double minWordLength = Math.Floor(Math.Log(number, 2)) + 1;
                masks.Add(new IncludeMask {Mask = number, NumberOfTruths =  numOfTruths, MinWordLength = minWordLength});
            }
            return masks;
        }

        private class IncludeMask
        {
            public int Mask { get; set; }
            public int NumberOfTruths { get; set; }
            public double MinWordLength { get; set; }

            public override string ToString()
            {
                return $"M\t{Mask}T{NumberOfTruths}\tW{MinWordLength}";
            }
        }
    }
}