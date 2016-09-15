using System.Collections.Generic;
using System.Linq;
using MWBS.Models;

namespace MWBS.BoardCop
{
    public class BoardCop : IBoardCop
    {
        public Board RemoveWord(Word word, Board board)
        {
            Board newBoard = board.Clone();

            foreach (LetterPoint letterPoint in word.LetterPoints)
            {
                RemoveLetter(newBoard, letterPoint);
            }

            FixBoard(newBoard);

            return newBoard;
        }

        public List<Word> GetPossibleWordPlacement(Board board, string word)
        {
            List<Word> possibleWords = GetPossibleWordPlacementRec(board, word, new List<LetterPoint>());
            List<Word> validWords = possibleWords.Where(w => w.LetterPoints.Count == word.Length && IsValidWord(w)).ToList();

            return validWords;
        }

        private List<Word> GetPossibleWordPlacementRec(Board board, string word, List<LetterPoint> usedLetterPoints)
        {
            int length = word.Length;
            if (length == 0)
            {
                return new List<Word>();
            }

            char firstLetter = word[0];

            var words = new List<Word>();

            List<LetterPoint> letterPoints = PossibleLetterPoints(board, firstLetter, usedLetterPoints);
            foreach (LetterPoint letterPoint in letterPoints)
            {
                var newUsed = new List<LetterPoint>(usedLetterPoints) {letterPoint};
                List<Word> possibleSubWordPlacements = GetPossibleWordPlacementRec(board, word.Substring(1), newUsed);
                if (!possibleSubWordPlacements.Any())
                {
                    words.Add(new Word
                    {
                        LetterPoints = new List<LetterPoint>
                        {
                            letterPoint
                        }
                    });
                    continue;
                }

                foreach (Word possibleSubWordPlacement in possibleSubWordPlacements)
                {
                    possibleSubWordPlacement.LetterPoints.Insert(0, letterPoint);
                    words.Add(possibleSubWordPlacement);
                }
            }

            return words;
        }

        private List<LetterPoint> PossibleLetterPoints(Board board, char letter, List<LetterPoint> usedLetterPoints)
        {
            List<LetterPoint> letterPoints = board.Where(lp => lp.Letter.HasValue && lp.Letter.Value == letter).ToList();
            List<LetterPoint> unusedLetterPoints = letterPoints.Where(lp => !usedLetterPoints.Any(ulp => ulp.Point.X == lp.Point.X && ulp.Point.Y == lp.Point.Y)).ToList();
            return unusedLetterPoints;
        }

        private bool IsValidWord(Word word)
        {
            int letterCount = word.LetterPoints.Count;
            for(int index = 0; index < letterCount -1; index++)
            {
                LetterPoint firstLetterPoint = word.LetterPoints[index];
                LetterPoint secondLetterPoint = word.LetterPoints[index+1];
                if (!AreNeighbors(firstLetterPoint.Point, secondLetterPoint.Point))
                {
                    return false;
                }
            }

            return true;
        }

        private bool AreNeighbors(Point one, Point two)
        {
            List<Point> neighbors = GetNeighbors(one);
            return neighbors.Any(n => n.X == two.X && n.Y == two.Y);
        }

        private List<Point> GetNeighbors(Point point)
        {
            var neighbors = new List<Point>
            {
                new Point {X = point.X - 1, Y = point.Y - 1},
                new Point {X = point.X - 1, Y = point.Y},
                new Point {X = point.X - 1, Y = point.Y + 1},
                new Point {X = point.X, Y = point.Y - 1},
                new Point {X = point.X, Y = point.Y + 1},
                new Point {X = point.X + 1, Y = point.Y - 1},
                new Point {X = point.X + 1, Y = point.Y},
                new Point {X = point.X + 1, Y = point.Y + 1}
            };

            return neighbors;
        }

        private void RemoveLetter(Board newBoard, LetterPoint letterPoint)
        {
            newBoard[letterPoint.Point.X, letterPoint.Point.Y] = null;
        }

        private void FixBoard(Board board)
        {
            int maxX = board.Size - 1;
            List<LetterPoint> bottomLetterPoints = board.Where(lp => lp.Point.X == maxX).ToList();
            foreach (LetterPoint bottomLetterPoint in bottomLetterPoints)
            {
                List<LetterPoint> column = GetLettersAtOrAbove(board, bottomLetterPoint).ToList();
                FixColumn(board, column);
            }
        }

        private void FixColumn(Board board, List<LetterPoint> letterPoints)
        {
            LetterPoint[] lettersWithValues = letterPoints.Where(lp => lp.Letter.HasValue).OrderByDescending(lp => lp.Point.X).ToArray();
            int numLettersWithValues = lettersWithValues.Length;

            int y = letterPoints.First().Point.Y; // Getting the correct y - same for all letters
            int maxX = board.Size - 1;
            int currentX = maxX;

            // Shift them all down
            for (int index = 0; index < numLettersWithValues; index++)
            {
                LetterPoint lp = lettersWithValues[index];
                board[currentX, y] = lp.Letter;
                currentX--;
            }

            // Remove the rest
            for (int x = currentX; x >= 0; x--)
            {
                board[x, y] = null;
            }
        }

        private IEnumerable<LetterPoint> GetLettersAtOrAbove(Board board, LetterPoint letterPoint)
        {
            IEnumerable<LetterPoint> letterPoints = board.Where(lp => (lp.Point.Y == letterPoint.Point.Y && lp.Point.X < letterPoint.Point.X) || (lp.Point.X == letterPoint.Point.X && lp.Point.Y == letterPoint.Point.Y));
            return letterPoints;
        }

    }
}