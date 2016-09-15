using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Ajax.Utilities;
using MWBS.BoardCop;
using MWBS.Dictionary;
using MWBS.Models;

namespace MWBS.BoardSolver
{
    public class BoardSolver : IBoardSolver
    {
        private readonly IBoardCop _boardCop;
        private readonly IWordDictionary _wordDictionary;

        public BoardSolver(IBoardCop boardCop, IWordDictionary wordDictionary)
        {
            _boardCop = boardCop;
            _wordDictionary = wordDictionary;
        }

        public PuzzleSolution SolvePuzzle(Puzzle puzzle)
        {
            List<string> properLengthWords = puzzle.WordLengths.Distinct().SelectMany(wl => _wordDictionary.GetWordsOfLength(wl)).ToList();
            List<string> possibleWords = GetPossibleWords(properLengthWords, puzzle.Board);

            List<List<Word>> possibleGuesses = GeneratePossibleGuesses(possibleWords, puzzle.Board, puzzle.WordLengths);
            List<Word[]> solutions = possibleGuesses.Where(pg => pg.Count == puzzle.WordLengths.Count).DistinctBy(pg => string.Join(",",pg.Select(word => word.Letters))).Select(pg => pg.ToArray()).ToList();

            return new PuzzleSolution { Solutions = solutions };
        }

        private List<string> GetPossibleWords(List<string> properLengthWords, Board board)
        {
            var possibleWords = new List<string>();
            Dictionary<char,int> allLetters = board.Where(lp => lp.Letter.HasValue).Select(lp => lp.Letter.Value).GroupBy(lp => lp).ToDictionary(lp => lp.Key, lp => lp.Count());
            foreach (string properLengthWord in properLengthWords)
            {
                Dictionary<char, int> wordLetters = properLengthWord.GroupBy(lp => lp).ToDictionary(lp => lp.Key, lp => lp.Count());
                if (wordLetters.Keys.All(letter => allLetters.ContainsKey(letter) && allLetters[letter] >= wordLetters[letter]))
                {
                    possibleWords.Add(properLengthWord);
                }
            }
            return possibleWords;
        }

        public List<List<Word>> GeneratePossibleGuesses(List<string> validWords, Board currentBoard, List<int> currentWordLengths)
        {
            if (currentWordLengths.Count == 0)
            {
                return new List<List<Word>>();
            }

            var currentPossibleGuesses = new List<List<Word>>();

            int currentWordLength = currentWordLengths.First();

            List<int> remainingWordLengths = new List<int>(currentWordLengths);
            remainingWordLengths.Remove(currentWordLength);

            List<string> validLengthWords = validWords.Where(w => w.Length == currentWordLength).ToList();
            foreach (string validLengthWord in validLengthWords)
            {
                List<Word> possiblePlacements = _boardCop.GetPossibleWordPlacement(currentBoard, validLengthWord);
                foreach (Word possiblePlacement in possiblePlacements)
                {
                    Board modifiedBoard = _boardCop.RemoveWord(possiblePlacement, currentBoard);
                    List<List<Word>> possibleSubPlacements = GeneratePossibleGuesses(validWords, modifiedBoard, remainingWordLengths);

                    if (possibleSubPlacements.Count == 0)
                    {
                        currentPossibleGuesses.Add(new List<Word> { possiblePlacement });
                    }
                    else
                    {
                        possibleSubPlacements.ForEach(pp =>
                        {
                            pp.Insert(0, possiblePlacement);
                            currentPossibleGuesses.Add(pp);
                        });
                    }
                }
            }

            return currentPossibleGuesses;
        }

        
    }
}