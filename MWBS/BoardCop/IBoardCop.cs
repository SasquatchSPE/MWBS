using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MWBS.Models;

namespace MWBS.BoardCop
{
    public interface IBoardCop
    {
        bool AreValidGuesses(Board board, params Word[] guesses);
    }
}