using System;
using System.Linq;
using MWBS.Models;

namespace MWBS.Plant
{
    public class PuzzlePlant
    {
        public static string IsValidPuzzle(string board, params int[] wordLengths)
        {
            if (!(Math.Abs(Math.Sqrt(board.Length)%1) < .00001))
            {
                return "Number of letters in the word must fit on a square letters.";
            }

            if (wordLengths.Sum() != board.Length)
            {
                return "Word lengths don't add up to number of letters on the board.";
            }

            return string.Empty;
        }

        public static Puzzle BuildPuzzle(string letters, params int[] wordLengths)
        {
            var board = new Board(letters);
            var puzzle = new Puzzle
            {
                Board = board,
                WordLengths = wordLengths.ToList()
            };

            return puzzle;
        }
    }
}