using GameLib;
using GameLib.Bots;
using GameLib.Messages;
using GameLib.Rules;
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
        public Board GameBoard { get; set; }
        public Dictionary<RectangleF, Zone> ViewMap { get; set; }
        public Zone Attacking { get; set; }
        public Zone Defending { get; set; }
        public string StatusMessage { get; set; }
        public BattleResult CurrentAttack { get; set; }
        public DateTime? StatusTime { get; set; }


        public MainForm()
        {
            InitializeComponent();
            ResizeRedraw = true;
            DoubleBuffered = true;
            StatusMessage = "";
            StatusTime = DateTime.UtcNow;

            // Basic setup
            GameBoard = Board.NewBoard(10, 10, 6);
            GameBoard.BattleRule = new RankedDice();
            GameBoard.ReinforcementRule = new RandomBorder();
            GameBoard.Players[0].Bot = new BorderShrinkBot();
        }

        private void MainForm_Paint(object sender, PaintEventArgs e)
        {
            Dictionary<RectangleF, Zone> dict = new Dictionary<RectangleF, Zone>();

            // Font info
            Font drawFont = new Font("Arial", 16);
            SolidBrush drawBrush = new SolidBrush(Color.Black);

            // Reserve room for status information
            var status_height = 150;
            var board_height = this.ClientSize.Height - status_height;
            var board_width = this.ClientSize.Width;

            // Draw status
            e.Graphics.DrawString($"Turn: Player {GameBoard.Players[GameBoard.CurrentTurn].Name}", drawFont, drawBrush, new PointF(0, 0));
            e.Graphics.DrawString(StatusMessage, drawFont, drawBrush, new PointF(0, 30));

            // Figure out cell height and width
            float cellHeight = board_height * 1.0f / GameBoard.Height;
            float cellWidth = board_width * 1.0f / GameBoard.Width;

            // Text settings
            StringFormat drawFormat = new StringFormat();
            drawFormat.Alignment = StringAlignment.Center;
            drawFormat.LineAlignment = StringAlignment.Center;

            // Draw each zone
            foreach (var z in GameBoard.Zones)
            {
                bool isAttacking = (z == Attacking || z == Defending);

                // Figure out color to use
                Brush brush = null;
                if (z.Owner == null)
                {
                    brush = new SolidBrush(Color.DarkGray);
                }
                else
                {
                    Color c = z.Owner.Color;
                    if (isAttacking)
                    {
                        c = Lighten(c, 0.7f);
                    }
                    brush = new SolidBrush(c);
                }

                // Draw rectangle
                RectangleF r = new RectangleF(cellWidth * z.X, status_height + cellHeight * z.Y, cellWidth, cellHeight);
                e.Graphics.FillRectangle(brush, r);

                // Draw border if attacking
                if (isAttacking)
                {
                    e.Graphics.DrawRectangle(new Pen(Color.Black), r.X, status_height + r.Y, cellWidth-1, cellHeight-1);
                }

                // Draw strength centered in the box
                e.Graphics.DrawString(z.Strength.ToString(), drawFont, drawBrush, r, drawFormat);

                // Keep track of screen view map
                dict[r] = z;
            }

            // Update viewmap
            ViewMap = dict;
        }

        private void MainForm_Click(object sender, EventArgs e)
        {
            MouseEventArgs m = e as MouseEventArgs;
            if (m != null)
            {
                var zone = GetClickedZone(m.Location);
                if (zone != null)
                {
                    // Set an attacker
                    if (this.Attacking == null)
                    {
                        if (zone.Owner.Number != GameBoard.CurrentTurn)
                        {
                            SystemSounds.Beep.Play();
                            return;
                        }
                        this.Attacking = zone;
                        this.Invalidate();
                        return;
                    }

                    // Setup attack plan
                    var plan = new AttackPlan()
                    {
                        Attacker = Attacking,
                        Defender = zone
                    };

                    // Set a defender
                    ExecuteAttackPlan(plan);
                    return;
                }
            }
        }

        private void ExecuteAttackPlan(AttackPlan plan)
        {
            var result = GameBoard.BattleRule.Attack(GameBoard, GameBoard.Players[GameBoard.CurrentTurn], plan);
            if (result.AttackWasInvalid)
            {
                SystemSounds.Beep.Play();
            }
            else
            {
                StatusMessage = $"Battle Result: {result.Plan.Attacker.X + 1}/{result.Plan.Attacker.Y + 1} vs {result.Plan.Defender.X + 1}/{result.Plan.Defender.Y + 1}:";
                if (result.AttackSucceeded)
                {
                    StatusMessage += " Victory!";
                } else
                {
                    StatusMessage += " Failed.";
                }
                StatusTime = DateTime.UtcNow;
                CurrentAttack = result;
                Invalidate();
            }
            this.Attacking = null;
            this.Invalidate();
            return;
        }

        public static Color Lighten(Color color, float pct)
        {
            float red = (float)color.R;
            float green = (float)color.G;
            float blue = (float)color.B;

            red = (255 - red) * pct + red;
            green = (255 - green) * pct + green;
            blue = (255 - blue) * pct + blue;

            return Color.FromArgb(color.A, (int)red, (int)green, (int)blue);
        }

        private Zone GetClickedZone(Point location)
        {
            foreach (var kvp in ViewMap)
            {
                if (kvp.Key.Contains(location))
                {
                    return kvp.Value;
                }
            }
            return null;
        }

        private void MainForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            // End Turn command
            if (e.KeyChar == 'R' || e.KeyChar == 'r')
            {
                GameBoard.EndTurn();
                Invalidate();
            }
            else if (e.KeyChar == 'A' || e.KeyChar == 'a')
            {
                var bot = new RandomBot();
                var plan = bot.PickNextAttack(GameBoard, GameBoard.CurrentPlayer);
                if (plan == null)
                {
                    MessageBox.Show("No attacks!");
                }
                else
                {
                    ExecuteAttackPlan(plan);
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (StatusTime != null) {
                var ts = DateTime.UtcNow - StatusTime.Value;
                if (ts.TotalMilliseconds > 250)
                {
                    // Was there a current attack?
                    if (CurrentAttack != null)
                    {
                        CurrentAttack.UpdateBoardTask.RunSynchronously();
                    }

                    // Clear all status
                    StatusMessage = "";
                    StatusTime = null;
                    CurrentAttack = null;

                    // If the current player is a bot, do another attack
                    if (!GameBoard.CurrentPlayer.IsHuman)
                    {
                        var plan = GameBoard.CurrentPlayer.Bot.PickNextAttack(GameBoard, GameBoard.CurrentPlayer);
                        if (plan == null)
                        {
                            GameBoard.EndTurn();
                            StatusTime = DateTime.UtcNow;
                        }
                        else
                        {
                            ExecuteAttackPlan(plan);
                        }
                        Invalidate();

                    }
                }
            }
        }
    }
}
