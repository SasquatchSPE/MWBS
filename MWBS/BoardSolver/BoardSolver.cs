using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Ajax.Utilities;
using MWBS.BoardCop;
using MWBS.Dictionary;
using MWBS.Models;
using MWBS.Permeator;

namespace MWBS.BoardSolver
{
    public class BoardSolver : IBoardSolver
    {
        private readonly IPermeator _permeator;
        private readonly IBoardCop _boardCop;
        private readonly IWordDictionary _wordDictionary;

        public BoardSolver(IPermeator permeator, IBoardCop boardCop, IWordDictionary wordDictionary)
        {
            _permeator = permeator;
            _boardCop = boardCop;
            _wordDictionary = wordDictionary;
        }

        public PuzzleSolution SolvePuzzle(Puzzle puzzle)
        {
            char[] allLetters = puzzle.Board.Where(lp => lp.Letter.HasValue).Select(lp => lp.Letter.Value).ToArray();
            List<string> allPermeators = _permeator.Permeate(allLetters);
            List<string> validWords = allPermeators.Where(p => _wordDictionary.Contains(p)).ToList();
            List<List<Word>> possibleGuesses = GetPossibleGuesses(validWords, puzzle);

            var solution = new PuzzleSolution();
            foreach (List<Word> validLengthGuess in possibleGuesses)
            {
                bool isValid = _boardCop.AreValidGuesses(puzzle.Board, validLengthGuess.ToArray());
                if (isValid)
                {
                    solution.Solutions.Add(validLengthGuess.ToArray());
                }
            }

            return solution;
        }

        private List<List<Word>> GetPossibleGuesses(List<string> validWords, Puzzle puzzle)
        {
            List<string> validLengthWords = validWords.Where(w => puzzle.WordLengths.Contains(w.Length)).ToList();
            Dictionary<string, List<Word>> possiblePlacements = validLengthWords.ToDictionary(vw => vw, vw => _boardCop.GetPossibleWordPlacement(puzzle.Board, vw));
            List<List<string>> possibleCombinations = GetPossibleCombinations(validLengthWords, puzzle.WordLengths).DistinctBy(list => string.Join(",", list)).ToList();
            List<List<Word>> posssibleGuesses = possibleCombinations.SelectMany(pc => PlacePossibleCombo(pc, possiblePlacements)).ToList();

            return posssibleGuesses;
        }

        private List<List<Word>> PlacePossibleCombo(List<string> possibleCombo, Dictionary<string, List<Word>> possiblePlacements)
        {
            if (!possibleCombo.Any())
            {
                return new List<List<Word>>();
            }

            string word = possibleCombo.First();

            List<string> remaining = new List<string>(possibleCombo);
            remaining.Remove(word);

            List<Word> placements = possiblePlacements[word];

            if (!remaining.Any())
            {
                var trees = placements.Select(p => new List<Word> {p}).ToList();
                return trees;
            }

            var guesses = new List<List<Word>>();
            
            foreach (Word placement in placements)
            {
                List<List<Word>> subPlacements = PlacePossibleCombo(remaining, possiblePlacements);
                subPlacements.ForEach(sp =>
                {
                    sp.Insert(0, placement);
                    guesses.Add(sp);
                });   
            }

            return guesses;
        }

        private List<List<string>> GetPossibleCombinations(List<string> validLengthWords, List<int> wordLengths)
        {
            return GetPossibleCombinationsRec(validLengthWords, wordLengths, new List<string>());
        }

        private List<List<string>> GetPossibleCombinationsRec(List<string> words, List<int> wordLengths, List<string> usedWords)
        {
            if (!wordLengths.Any())
            {
                return new List<List<string>>();
            }

            int nextWordLength = wordLengths.First();
            var combos = new List<List<string>>();
            
            List<string> validLengthStrings = words.Where(vlw => vlw.Length == nextWordLength && !usedWords.Contains(vlw)).ToList();
            if (!validLengthStrings.Any())
            {
                throw new ApplicationException("Board is unsolvable");
            }

            var newWordLengths = new List<int>(wordLengths);
            newWordLengths.Remove(nextWordLength);

            if (!newWordLengths.Any())
            {
                List<List<string>> trees = validLengthStrings.Select(vls => new List<string> {vls}).ToList();
                return trees;
            }

            foreach (string validLengthString in validLengthStrings)
            {
                var updatedUsedWords = new List<string>(usedWords) {validLengthString};
                List<List<string>> possibleSubCombos = GetPossibleCombinationsRec(words, newWordLengths, updatedUsedWords);
                
                possibleSubCombos.ForEach(psb =>
                {
                    psb.Insert(0, validLengthString);
                    combos.Add(psb);
                });
            }

            return combos;
        }
    }
}