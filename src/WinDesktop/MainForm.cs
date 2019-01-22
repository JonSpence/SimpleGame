using GameLib;
using GameLib.Bots;
using GameLib.Messages;
using GameLib.Rules;
using SkiaSharp.Views.Desktop;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Media;
using System.Text;
using System.Windows.Forms;

namespace WinDesktop
{
    public partial class MainForm : Form
    {
        public GameViewController Controller { get; set; }
        public SKControl skcGame { get; set; }


        public MainForm()
        {
            InitializeComponent();
            ResizeRedraw = true;
            DoubleBuffered = true;

            // Add SKCanvas
            skcGame = new SKControl()
            {
                Top = this.DisplayRectangle.Top + this.menuStrip1.Height,
                Left = this.DisplayRectangle.Left,
                Width = this.DisplayRectangle.Width,
                Height = this.DisplayRectangle.Height - this.menuStrip1.Height
            };
            skcGame.Click += Skc_Click;
            skcGame.PaintSurface += Skc_PaintSurface;
            this.Controls.Add(skcGame);

            // Start basic game
            StartNewGame();
        }

        private void Skc_Click(object sender, EventArgs e)
        {
            var m = e as MouseEventArgs;
            if (m != null)
            {
                HandleResult(Controller.HandleTouch(sender, new SkiaSharp.SKPoint(m.X, m.Y)));
            }
        }

        private void Skc_PaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            Controller.Paint(sender, e.Surface.Canvas);
        }

        private void MainForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            // End Turn command
            if (e.KeyChar == 'R' || e.KeyChar == 'r')
            {
                if (Controller.GameBoard.CurrentPlayer.IsHuman)
                {
                    Controller.GameBoard.EndTurn();
                    skcGame.Invalidate();
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            // If the current player is a bot, do another attack
            if (!Controller.GameBoard.CurrentPlayer.IsHuman)
            {
                HandleResult(Controller.TakeBotAction());
            }
        }

        private void HandleResult(GameViewController.GameAttackResult gameAttackResult)
        {
            switch (gameAttackResult)
            {
                case GameViewController.GameAttackResult.GameOver:
                    MessageBox.Show("Winner: " + Controller.GameBoard.Winner.Color.ToString(), "Game Over", MessageBoxButtons.OK);
                    timer1.Enabled = false;
                    break;
                case GameViewController.GameAttackResult.Invalid:
                    SystemSounds.Beep.Play();
                    break;
            }
            skcGame.Invalidate();
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            if (skcGame != null)
            {
                skcGame.Width = this.DisplayRectangle.Width;
                skcGame.Height = this.DisplayRectangle.Height - this.menuStrip1.Height;
                skcGame.Invalidate();
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void newGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StartNewGame();
        }

        private void StartNewGame()
        {
            // Basic setup
            Controller = new GameViewController();
            Controller.GameBoard = Board.NewBoard(10, 10, 6, Themes.RAINBOW_THEME);
            Controller.GameBoard.BattleRule = new RankedDice();
            Controller.GameBoard.ReinforcementRule = new RandomBorder();
            Controller.GameBoard.Players[1].Bot = new BorderShrinkBot();

            // Start the timer for bot stuff
            timer1.Enabled = true;
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("(C) Ted Spence 2019");
        }
    }
}
