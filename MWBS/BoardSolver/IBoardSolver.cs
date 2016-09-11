using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MWBS.Models;

namespace MWBS.BoardSolver
{
    public interface IBoardSolver
    {
        PuzzleSolution SolvePuzzle(Puzzle puzzle);
    }
}
