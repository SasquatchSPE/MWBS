using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using MWBS.Models;

namespace MWBS.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public PartialViewResult GetBoardSolution(string board, string wordLengths)
        {
            Thread.Sleep(3000);

            var solution = new PuzzleSolutionModel
            {
                Words = new List<List<string>>
                {
                    new List<string> {board, "fee",  "fi",  "fo" },
                    new List<string> {wordLengths, "foo", "bar" }
                }
            };
            return PartialView("_BoardSolutionView", solution);
        }
    }
}