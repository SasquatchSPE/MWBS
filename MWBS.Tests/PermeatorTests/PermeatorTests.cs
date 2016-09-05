using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace MWBS.Tests.PermeatorTests
{
    [TestFixture]
    public class PermeatorTests
    {
        [Test]
        [TestCase("bad",2,3,"ba","ab","bd","db","ad","da","bad","bda","abd","adb","dab","dba")]
        [TestCase("asdfghjkl", 2, 4, "agj")]
        [TestCase("asdfghjksdflzxcv", 2, 4, "agjx")]
        public void Permeate_Test(string input, int minLength, int maxLength, params string[] expectedResults)
        {
            var perm = new Permeator.Permeator(input.Length);
            List<string> results = perm.Permeate(input.ToCharArray(), minLength, maxLength);
            foreach (string expectedResult in expectedResults)
            {
                Assert.Contains(expectedResult, results);
            }
        }

        [Test]
        [TestCase("asdf", 3)]
        [TestCase("asdf", 2)]
        [TestCase("asdf", 1)]
        [TestCase("asdf", 0)]
        [TestCase("aassdfddd", 0)]
        [TestCase("aassdfddd", 1)]
        [TestCase("aassdfddd", 2)]
        [TestCase("aassdfddd", 3)]
        [TestCase("ad", 0)]
        [TestCase("ad", 1)]
        [TestCase("a", 0)]
        public void GetAllButIndex_Test(string letters, int index)
        {
            char letterToRemove = letters[index];
            int letterCount = letters.Count(l => l == letterToRemove);
            int previousLength = letters.Length;

            var perm = new Permeator.Permeator(letters.Length);
            char[] removed = perm.GetAllButIndex(letters.ToCharArray(), index);

            Assert.AreEqual(previousLength - 1, removed.Length, "Expected length of {0} but got {1}", previousLength - 1, removed.Length);
            Assert.AreEqual(letterCount - 1, removed.Count(r => r == letterToRemove), "Expected {0} instances of {1} but found {2} instead", letterCount - 1, letterToRemove, removed.Count(r => r == letterToRemove));
        }

        [Test]
        [TestCase("a", "a")]
        [TestCase("as","as","sa")]
        [TestCase("ads", "ads", "asd", "das", "dsa", "sad", "sda")]
        [TestCase("wdd", "wdd", "dwd", "ddw")]
        public void GetAllCombos_Test(string word, params string[] expectedWords)
        {
            int length = word.Length;
            int totalWordCount = Factorial(length);

            var perm = new Permeator.Permeator(word.Length);
            List<string> combos = perm.GetAllCombos(word.ToCharArray()).Select(c => new string(c.ToArray())).ToList();

            Assert.AreEqual(totalWordCount, combos.Count, "Expected {0} words but got {1}", totalWordCount, combos.Count);
            foreach (string expectedWord in expectedWords)
            {
                Assert.IsTrue(combos.Contains(expectedWord), "Expected to find {0} but did not", expectedWord);
            }
        }

        private int Factorial(int input)
        {
            int factorial = 1;
            for (int spot = 2; spot <= input; spot++)
            {
                factorial *= spot;
            }
            return factorial;
        }

        [Test]
        [TestCase(3, 0, 0)]
        [TestCase(3, 1, 1, 2, 4)]
        [TestCase(3, 2, 3, 5, 6)]
        [TestCase(3, 3, 7)]
        [TestCase(1, 1, 1)]
        [TestCase(1, 0, 0)]
        [TestCase(4, 2, 3, 5, 6, 9, 10, 12)]
        public void GetIncludeInputs_Test(int length, int numOfTruths, params int[] expectedResults)
        {
            var perm = new Permeator.Permeator(length);
            List<int> inputs = perm.GetIncludeInputs(length, numOfTruths);

            decimal expectedCount = Choose(length, numOfTruths);
            Assert.AreEqual(expectedCount, inputs.Count);
            foreach (int expectedResult in expectedResults)
            {
                Assert.Contains(expectedResult, inputs);
            }
        }

        private decimal Choose(int n, int k)
        {
            decimal result = 1;
            for (int i = 1; i <= k; i++)
            {
                result *= n - (k - i);
                result /= i;
            }
            return result;
        }

        [Test]
        [TestCase("as", 1, "s")]
        [TestCase("as", 2, "a")]
        [TestCase("as", 3, "as")]
        [TestCase("asd", 4, "a")]
        [TestCase("asd", 5, "ad")]
        [TestCase("asd", 6, "as")]
        [TestCase("asd", 7, "asd")]
        [TestCase("asdf", 8, "a")]
        [TestCase("asdf", 9, "af")]
        [TestCase("asdf", 10, "ad")]
        [TestCase("asdf", 11, "adf")]
        [TestCase("asdf", 12, "as")]
        [TestCase("asdf", 13, "asf")]
        [TestCase("asdf", 14, "asd")]
        [TestCase("asdf", 15, "asdf")]
        public void GetInputBit_Test(string word, int includeInput, string expectedResult)
        {
            var perm = new Permeator.Permeator(word.Length);
            string result = perm.GetInput(word.ToCharArray(), includeInput);
            Assert.AreEqual(expectedResult, result);
        }
    }
}
