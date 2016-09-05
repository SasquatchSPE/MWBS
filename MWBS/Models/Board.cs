using System.Collections;
using System.Collections.Generic;

namespace MWBS.Models
{
    public class Board : IEnumerable<LetterPoint>
    {
        private char?[,] Letters { get; set; }
        public Board(int size)
        {
            Letters = new char?[size,size];
        }

        public int Size => Letters.Length;

        public IEnumerator<LetterPoint> GetEnumerator()
        {
            int length = Letters.Length;

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