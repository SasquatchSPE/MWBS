using System.Collections.Generic;
using MWBS.Models;

namespace MWBS.BoardCop
{
    public interface IBoardCop
    {
        bool AreValidGuesses(Board board, params Word[] guesses);
        List<Word> GetPossibleWordPlacement(Board board, string word);
    }
}