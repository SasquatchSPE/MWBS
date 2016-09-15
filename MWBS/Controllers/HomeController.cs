using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using MWBS.BoardSolver;
using MWBS.Models;
using MWBS.Plant;

namespace MWBS.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public PartialViewResult GetBoardSolution(string board, string wordLengths)
        {
            ModelState.Clear();

            var solutionModel = new PuzzleSolutionModel();
            string[] splitLengths = wordLengths.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries);

            int dummy;
            if (splitLengths.Any(sl => !Int32.TryParse(sl, out dummy)))
            {
                solutionModel.ErrorMessage = "Word lengths must be comma separated numbers.";
                return PartialView("_BoardSolutionView", solutionModel);
            }

            int[] lengths = splitLengths.Select(w => Convert.ToInt32(w)).ToArray();
            string message = PuzzlePlant.IsValidPuzzle(board, lengths);
            if (!string.IsNullOrWhiteSpace(message))
            {
                solutionModel.ErrorMessage = message;
                return PartialView("_BoardSolutionView", solutionModel);
            }

            IBoardSolver boardSolver = Plant.Plant.BoardSolver;
            Puzzle puzzle = PuzzlePlant.BuildPuzzle(board, lengths);
            PuzzleSolution solution = boardSolver.SolvePuzzle(puzzle);

            solutionModel.Words = solution.Solutions.Select(words => words.Select(w => w.Letters).ToList()).ToList();
            return PartialView("_BoardSolutionView", solutionModel);
        }
    }
}