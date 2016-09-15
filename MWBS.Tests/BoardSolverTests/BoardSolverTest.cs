using System.Collections.Generic;
using System.Linq;
using MWBS.BoardCop;
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
        
            IWordDictionary dictionary = GetDictionary();
            IBoardCop boardCop = new BoardCop.BoardCop();
            IBoardSolver solver = new BoardSolver.BoardSolver(boardCop, dictionary);

            PuzzleSolution ps = solver.SolvePuzzle(puzzle);
            Assert.IsNotNull(ps);
        }

        [Test]
        public void SolveBoard_9_Test()
        {
            Puzzle puzzle = GeneratePuzzle(3, "flfooirde", 4, 5); // roof field

            IWordDictionary dictionary = GetDictionary();
            IBoardCop boardCop = new BoardCop.BoardCop();
            IBoardSolver solver = new BoardSolver.BoardSolver(boardCop, dictionary);

            PuzzleSolution solutions = solver.SolvePuzzle(puzzle);
            Assert.IsNotNull(solutions);
        }

        [Test]
        public void SolveBoard_9_Test_WrongFirstChoice()
        {
            Puzzle puzzle = GeneratePuzzle(3, "efdidlrie", 4, 5); // ride field

            IWordDictionary dictionary = GetDictionary();
            IBoardCop boardCop = new BoardCop.BoardCop();
            IBoardSolver solver = new BoardSolver.BoardSolver(boardCop, dictionary);

            PuzzleSolution solutions = solver.SolvePuzzle(puzzle);
            Assert.IsNotNull(solutions);
        }

        [Test]
        public void SolveBoard_16_Test()
        {
            Puzzle puzzle = GeneratePuzzle(4, "aynedhecrtcaibkl", 8, 8); // cupboard roof oval

            IWordDictionary dictionary = GetDictionary();
            IBoardCop boardCop = new BoardCop.BoardCop();
            IBoardSolver solver = new BoardSolver.BoardSolver(boardCop, dictionary);
            PuzzleSolution solutions = solver.SolvePuzzle(puzzle);
            Assert.IsNotNull(solutions);
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
