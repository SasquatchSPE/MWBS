﻿using System.Collections.Generic;
using System.Linq;
using MWBS.Models;

namespace MWBS.BoardCop
{
    public class BoardCop : IBoardCop
    {
        public bool AreValidGuesses(Board board, params Word[] guesses)
        {
            Board newBoard = board.Clone();
            foreach (Word guess in guesses)
            {
                if (!IsValidConnection(newBoard, guess))
                {
                    return false;
                }

                RemoveWord(board, guess);
            }

            return true;
        }

        public bool IsValidConnection(Board board, Word word)
        {
            LetterPoint[] letterPoints = word.LetterPoints.ToArray();
            int length = letterPoints.Length;
            for (int index = 0; index < length - 1; index++)
            {
                LetterPoint currentLetterPoint = letterPoints[index];
                LetterPoint nextLetterPoint = letterPoints[index + 1];
                if (!AreNeighbors(currentLetterPoint.Point, nextLetterPoint.Point))
                {
                    return false;
                }
            }

            return true;
        }

        public bool AreNeighbors(Point one, Point two)
        {
            List<Point> neighbors = GetNeighbors(one);
            return neighbors.Any(n => n.X == two.X && n.Y == two.Y);
        }

        public List<Point> GetNeighbors(Point point)
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

        public void RemoveWord(Board board, Word guess)
        {
            foreach (LetterPoint letterPoint in guess.LetterPoints)
            {
                RemoveLetter(board, letterPoint);
            }

            FixLetters(board);
        }

        private void RemoveLetter(Board newBoard, LetterPoint letterPoint)
        {
            newBoard[letterPoint.Point.X, letterPoint.Point.Y] = null;
        }

        private void FixLetters(Board board)
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

            int y = letterPoints.First().Point.Y;
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
            for (int x = currentX; x <= 0; x--)
            {
                board[x, y] = null;
            }
        }

        public IEnumerable<LetterPoint> GetLettersAtOrAbove(Board board, LetterPoint letterPoint)
        {
            IEnumerable<LetterPoint> letterPoints = board.Where(lp => (lp.Point.Y == letterPoint.Point.Y && lp.Point.X > letterPoint.Point.X) || (lp.Point.X == letterPoint.Point.X && lp.Point.Y == letterPoint.Point.Y));
            return letterPoints;
        }
    }
}