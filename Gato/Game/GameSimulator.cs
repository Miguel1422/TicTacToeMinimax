using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Gato.Game.BoardState;

namespace Gato.Game
{
    public class GameSimulator
    {
        public BoardState.Cell CurrentPlayer { get; private set; }
        private BoardState state;
        public GameSimulator() : this(BoardState.Cell.O)
        {
        }
        public GameSimulator(BoardState.Cell player)
        {
            this.CurrentPlayer = player;
            this.state = new BoardState();
        }

        public void Move(int x, int y)
        {
            if (state.Board[x, y] != Cell.Empty) throw new Exception("La celda ya estaba ocupada");
            state = state.Play(new BoardState.Move(x, y, CurrentPlayer));
            this.CurrentPlayer = this.CurrentPlayer == BoardState.Cell.O ? BoardState.Cell.X : BoardState.Cell.O;
        }
        public BoardState.Move BestMove()
        {
            return state.Minimax(CurrentPlayer).bestMove;
        }

        public bool SomeWinner
        {
            get => state.SomeWinner;
        }
        public bool XWins
        {
            get => state.XWins;
        }
        public bool OWins
        {
            get => state.OWins;
        }
        public bool IsTie
        {
            get => state.IsTie;
        }
        public int Plays
        {
            get => state.Plays;
        }
        public int EmptySpaces
        {
            get => state.EmptySpaces;
        }
    }
}
