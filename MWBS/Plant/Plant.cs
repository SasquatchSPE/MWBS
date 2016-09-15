using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MWBS.BoardCop;
using MWBS.BoardSolver;
using MWBS.Dictionary;

namespace MWBS.Plant
{
    public class Plant
    {
        private static IBoardCop BoardCop { get; } = new BoardCop.BoardCop();
        private static IWordDictionary WordDictionary { get; } = new WordDictionary();
        public static IBoardSolver BoardSolver { get; } = new BoardSolver.BoardSolver(BoardCop, WordDictionary);
    }
}