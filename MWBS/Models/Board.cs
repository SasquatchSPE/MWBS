using System;
using System.Collections;
using System.Collections.Generic;

namespace MWBS.Models
{
    public class Board : IEnumerable<LetterPoint>
    {
        public int Size { get; set; }
        private char?[,] Letters { get; }

        public Board(int size)
        {
            Size = size;
            Letters = new char?[Size, Size];
        }

        public Board(string letters)
        {
            Size = GetSize(letters.Length);
            Letters = new char?[Size, Size];
            SetBoard(letters);
        }

        private int GetSize(int numOfLetters)
        {
            double size = Math.Sqrt(numOfLetters);
            if (!(Math.Abs(size % 1) < .00001))
            {
                throw new ApplicationException($"Number of letters need to fit on a square board. {numOfLetters} is incompatible.");
            }
            return (int) size;
        }

        private void SetBoard(string letters)
        {
            int expectedLetterSize = (int)Math.Pow(Size, 2);
            if (expectedLetterSize != letters.Length)
            {
                throw new ApplicationException($"There are {letters.Length} letters but there should be {expectedLetterSize}");
            }

            int index = 0;
            for (int x = 0; x < Size; x++)
            {
                for (int y = 0; y < Size; y++)
                {
                    char letter = letters[index++];
                    this[x, y] = letter;
                }
            }
        }

        public IEnumerator<LetterPoint> GetEnumerator()
        {
            int length = Size;

            for (int x = 0; x < length; x++)
            {
                for (int y = 0; y < length; y++)
                {
                    var point = new Point {X = x, Y = y};
                    LetterPoint letterPoint = this[point];
                    yield return letterPoint;
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public char? this[int x, int y]
        {
            get { return Letters[x, y]; }
            set { Letters[x, y] = value; }
        }

        public LetterPoint this[Point point] => new LetterPoint
        {
            Letter = Letters[point.X, point.Y],
            Point = point
        };

        public Board Clone()
        {
            var board = new Board(Size);
            foreach (LetterPoint letterPoint in this)
            {
                board[letterPoint.Point.X, letterPoint.Point.Y] = letterPoint.Letter;
            }
            return board;
        }
    }
}