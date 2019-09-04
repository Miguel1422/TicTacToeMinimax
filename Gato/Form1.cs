using Gato.Game;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Gato
{
    public partial class Form1 : Form
    {
        private GameSimulator game;

        private Button[] buttons;
        public Form1()
        {
            InitializeComponent();
            game = new GameSimulator();
            this.buttons = new Button[] {
                button1,
                button2,
                button3,
                button4,
                button5,
                button6,
                button7,
                button8,
                button9,
            };
            foreach (var button in buttons)
            {
                button.Click += buttonClick;
            }
            this.ActiveControl = null;
        }
        private void buttonClick(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            int index = buttons.ToList().FindIndex(el => el == button);
            int xMove = index % 3;
            int yMove = index / 3;
            game.Move(xMove, yMove);

            button.Enabled = false;
            button.Text = "O";

            if (game.OWins)
            {
                MessageBox.Show("O ha ganado");
                reset(true);
                return;
            }
            if (game.IsTie)
            {
                MessageBox.Show("Es un empate");
                reset(false);
                return;
            }


            machineMoves();
            this.ActiveControl = null;
        }

        private void machineMoves()
        {
            var move = game.BestMove();
            game.Move(move.X, move.Y);
            int indexMove = (move.Y * 3 + move.X);
            var buttonMove = this.buttons[indexMove];
            buttonMove.Text = "X";
            buttonMove.Enabled = false;

            if (game.XWins)
            {
                MessageBox.Show("X ha ganado");
                reset(false);
                return;
            }
            if (game.IsTie)
            {
                MessageBox.Show("Es un empate");
                reset(true);
                return;
            }
        }

        private void reset(bool playerStarts)
        {
            foreach (var btn in buttons)
            {
                btn.Enabled = true;
                btn.Text = "";
            }
            if (playerStarts)
                game = new GameSimulator();
            else
            {
                game = new GameSimulator(BoardState.Cell.X);
                machineMoves();
            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.ActiveControl = null;
        }
    }
}
