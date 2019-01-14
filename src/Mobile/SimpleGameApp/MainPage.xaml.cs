using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameLib;
using GameLib.Messages;
using GameLib.Rules;
using Xamarin.Forms;

namespace SimpleGameApp
{
    public partial class MainPage : ContentPage
    {
        public Board GameBoard { get; set; }
        public string ActionMessage { get; set; }
        public Zone Attacking { get; set; }
        public Zone Defending { get; set; }
        public Dictionary<Button, Zone> ButtonToZone { get; set; }
        public Dictionary<Zone, Button> ZoneToButton { get; set; }
        public BattleResult CurrentAttack { get; set; }
        public DateTime? StatusTime { get; set; }

        public MainPage()
        {
            InitializeComponent();

            // Construct new board
            GameBoard = Board.NewBoard(5, 7, 3);
            GameBoard.BattleRule = new RankedDice();
            BuildBoard();
            RedrawBoard();
        }

        private void BuildBoard()
        {
            // Figure out dimensions for the board
            var board_height = this.grdBoard.Height;
            var board_width = this.grdBoard.Width;

            // Figure out cell height and width
            var cellHeight = board_height / GameBoard.Height;
            var cellWidth = board_width / GameBoard.Width;

            // Tell the grid how we're doing things
            grdBoard.ColumnDefinitions.Clear();
            for (int i = 0; i < grdBoard.Width; i++)
            {
                grdBoard.ColumnDefinitions.Add(new ColumnDefinition() { Width = cellWidth });
            }
            for (int i = 0; i < grdBoard.Height; i++)
            {
                grdBoard.RowDefinitions.Add(new RowDefinition() { Height = cellHeight });
            }

            // Create a button for each zone
            ButtonToZone = new Dictionary<Button, Zone>();
            ZoneToButton = new Dictionary<Zone, Button>();
            foreach (var z in GameBoard.Zones)
            {
                Button zb = new Button
                {
                    Text = "Hi",
                    Margin = new Thickness(0, 0, 0, 0),
                    BackgroundColor = Color.DarkGray,
                    TextColor = Color.FromRgb(0, 0, 0),
                    FontSize = 10.0f,
                    Padding = new Thickness(0, 0, 0, 0),
                };
                zb.Clicked += ZoneButton_Clicked;
                Grid.SetColumn(zb, z.X);
                Grid.SetRow(zb, z.Y);
                grdBoard.Children.Add(zb);
                ButtonToZone[zb] = z;
                ZoneToButton[z] = zb;
            }
        }

        void ZoneButton_Clicked(object sender, EventArgs e)
        {
            if (sender is Button button)
            {
                var zone = ButtonToZone[button];
                if (zone != null)
                {
                    // Set an attacker
                    if (this.Attacking == null)
                    {
                        if (zone.Owner.Number != GameBoard.CurrentTurn)
                        {
                            //SystemSounds.Beep.Play();
                            return;
                        }
                        this.Attacking = zone;
                        RedrawBoard();
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
                //SystemSounds.Beep.Play();
            }
            else
            {
                result.UpdateBoardTask.RunSynchronously();
                ActionMessage = $"Battle Result: {result.Plan.Attacker.X + 1}/{result.Plan.Attacker.Y + 1} vs {result.Plan.Defender.X + 1}/{result.Plan.Defender.Y + 1}:";
                if (result.AttackSucceeded)
                {
                    ActionMessage += " Victory!";
                }
                else
                {
                    ActionMessage += " Failed.";
                }
                StatusTime = DateTime.UtcNow;
                CurrentAttack = result;
                RedrawBoard();
            }
            this.Attacking = null;
            RedrawBoard();
        }

        /// <summary>
        /// Regenerate images in the grid panel
        /// </summary>
        private void RedrawBoard()
        {
            // Set status messages
            lblTurn.Text = $"Turn: Player {GameBoard.Players[GameBoard.CurrentTurn].Name}";
            lblStatus.Text = GameBoard.GameStatus();
            lblAction.Text = ActionMessage;

            // Draw each zone
            foreach (var z in GameBoard.Zones)
            {
                bool isAttacking = (z == Attacking || z == Defending);

                // Fetch button tied to this zone
                var zb = ZoneToButton[z];

                // Figure out color to use
                Color c = Color.DarkGray;
                if (z.Owner != null)
                {
                    c = Color.FromRgb(z.Owner.Color.R, z.Owner.Color.G, z.Owner.Color.B);
                    if (isAttacking)
                    {
                        c = Lighten(c, 0.7f);
                    }
                }
                zb.BackgroundColor = c;

                // Draw border if attacking
                if (isAttacking)
                {
                    //zb.BorderColor = Color.White;
                    //e.Graphics.DrawRectangle(new Pen(Color.Black), r.X, status_height + r.Y, cellWidth - 1, cellHeight - 1);
                }

                // Draw strength centered in the box
                zb.Text = z.Strength.ToString();
            }
        }

        public static Color Lighten(Color color, float pct)
        {
            var red = (255.0 - color.R) * pct + color.R;
            var green = (255.0 - color.G) * pct + color.G;
            var blue = (255.0 - color.B) * pct + color.B;

            return Color.FromRgb(red, green, blue);
        }

        void EndTurn_Clicked(object sender, System.EventArgs e)
        {
            GameBoard.EndTurn();
            RedrawBoard();
        }
    }
}
