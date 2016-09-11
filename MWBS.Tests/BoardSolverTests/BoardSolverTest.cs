using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MWBS.BoardSolver;
using MWBS.Dictionary;
using MWBS.Models;
using NUnit.Framework;

namespace MWBS.Tests.BoardSolverTests
{
    [TestFixture]
    public class BoardSolverTest
    {
        [Test]
        public void SolveBoard_4_Test()
        {
            Puzzle puzzle = GeneratePuzzle(2, "cart", 4);
            IBoardSolver solver = new BoardSolver.BoardSolver(new Permeator.Permeator(4), new BoardCop.BoardCop(), GetDictionary(new List<string>
            {
                "cart",
                "artc"
            }));
            PuzzleSolution ps = solver.SolvePuzzle(puzzle);

            Assert.IsNotNull(ps);
        }

        [Test]
        public void SolveBoard_9_Test()
        {
            Puzzle puzzle = GeneratePuzzle(3, "flfooirde", 4, 5);
            IBoardSolver solver = new BoardSolver.BoardSolver(new Permeator.Permeator(9), new BoardCop.BoardCop(), GetDictionary());
            PuzzleSolution ps = solver.SolvePuzzle(puzzle);

            Assert.IsNotNull(ps);
        }


        private Puzzle GeneratePuzzle(int size, string letters, params int[] wordLengths)
        {
            var board = new Board(size);
            board.SetBoard(letters);

            var puzzle = new Puzzle
            {
                Board = board,
                WordLengths = wordLengths.ToList()
            };

            return puzzle;
        }

        private IWordDictionary GetDictionary(List<string> words = null)
        {
            IWordDictionary dictionary = words == null
                ? new WordDictionary()
                : new WordDictionary(words);

            return dictionary;
        }
    }


}
