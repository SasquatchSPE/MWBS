using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MWBS.Manipulator
{
    public interface IWordManipulator
    {
        void CanFormWord(string word, IEnumerable<char> possibleLetters);
    }
}
