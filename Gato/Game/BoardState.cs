using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gato.Game
{
    public class BoardState
    {
        public Cell[,] Board { get; private set; }
        public BoardState Parent { get; set; }
        public Move LastMove { get; set; }

        public int Plays
        {
            get
            {
                int count = 0;
                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        if (Board[i, j] != Cell.Empty) count++;
                    }
                }
                return count;
            }
        }
        public int EmptySpaces
        {
            get
            {
                return 9 - Plays;
            }
        }

        public BoardState()
        {
            Board = new Cell[3, 3];
        }
        public BoardState(BoardState from) : this()
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    Board[i, j] = from.Board[i, j];
                }
            }
        }

        public List<BoardState> Neighbors(Cell player)
        {
            List<BoardState> list = new List<BoardState>();
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (Board[i, j] == Cell.Empty)
                    {
                        list.Add(Play(new Move(i, j, player)));
                    }
                }
            }
            return list;
        }

        public BoardState Play(Move move)
        {
            BoardState newBoard = new BoardState(this);
            newBoard.Parent = this;
            newBoard.LastMove = move;
            newBoard.Board[move.X, move.Y] = move.Player;
            return newBoard;
        }


        public Move BestMove(Cell player)
        {
            return Minimax(player).bestMove;
        }



        private const int inf = 0x3f3f3f3;
        private static Hashtable cache = new Hashtable();
        private static Random r = new Random();
        public (int score, Move bestMove) Minimax(Cell player)
        {
            if (this.XWins) return (-1, null);
            if (this.OWins) return (1, null);
            if (this.IsTie) return (0, null);
            if (cache.ContainsKey((this, player))) return ((int, Move))cache[((this, player))];

            int bestScore = player == Cell.X ? inf : -inf;
            Move bestMove = null;
            List<Move> bestMoves = new List<Move>();

            foreach (var neight in this.Neighbors(player))
            {
                Cell other = player == Cell.O ? Cell.X : Cell.O;
                var result = neight.Minimax(other);
                if ((player == Cell.X && bestScore > result.score) || (player == Cell.O && bestScore < result.score))
                {
                    bestMoves.Clear();
                    bestScore = result.score;
                    bestMove = neight.LastMove;
                }
                if ((player == Cell.X && bestScore == result.score) || (player == Cell.O && bestScore == result.score))
                {
                    bestMoves.Add(neight.LastMove);
                }

            }

            // cache.Add((this, player), (bestScore, bestMove));
            return (bestScore, bestMoves[r.Next(0, bestMoves.Count)]);
            return (bestScore, bestMove);
        }


        public bool SomeWinner
        {
            get
            {
                return XWins || OWins;
            }
        }
        public bool XWins
        {
            get
            {
                return Wins(Cell.X);
            }
        }
        public bool OWins
        {
            get
            {
                return Wins(Cell.O);
            }
        }
        public bool IsTie
        {
            get
            {
                if (SomeWinner) return false;
                return EmptySpaces == 0;
            }
        }

        private bool Wins(Cell player)
        {
            for (int i = 0; i < 3; i++)
            {
                if (Board[i, 0] == Board[i, 1] && Board[i, 0] == Board[i, 2]) if (Board[i, 0] == player) return true;
                if (Board[0, i] == Board[1, i] && Board[0, i] == Board[2, i]) if (Board[0, i] == player) return true;
            }
            if (Board[0, 0] == Board[1, 1] && Board[1, 1] == Board[2, 2]) if (Board[1, 1] == player) return true;
            if (Board[2, 0] == Board[1, 1] && Board[1, 1] == Board[0, 2]) if (Board[1, 1] == player) return true;
            return false;
        }

        public override bool Equals(object obj)
        {
            if (obj is BoardState state)
            {
                if (this.GetHashCode() != state.GetHashCode()) return false;
                return true;
            }
            return false;

        }

        private int hashCodeC = -1;
        public override int GetHashCode()
        {
            if (hashCodeC != -1) return hashCodeC;
            int hashCode = 0;
            foreach (var cell in Board)
            {
                hashCode = (hashCode << 2);
                hashCode |= (int)cell;
            }
            return hashCodeC = hashCode;
        }

        public class Move
        {
            public int X { get; set; }
            public int Y { get; set; }
            public Cell Player { get; set; }

            public Move(int x, int y, Cell player)
            {
                X = x;
                Y = y;
                Player = player;
            }
        }

        public enum Cell
        {
            Empty = 0,
            O = 1,
            X = 2
        }
    }
}
