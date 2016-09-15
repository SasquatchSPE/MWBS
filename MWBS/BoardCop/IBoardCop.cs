using System.Collections.Generic;
using MWBS.Models;

namespace MWBS.BoardCop
{
    public interface IBoardCop
    {
        Board RemoveWord(Word word, Board board);
        List<Word> GetPossibleWordPlacement(Board board, string word);
    }
}