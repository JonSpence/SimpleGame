using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameLib;
using GameLib.Messages;
using GameLib.Rules;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using Xamarin.Forms;

namespace SimpleGameApp
{
    public partial class MainPage : ContentPage
    {
        public GameViewController Controller { get; set; }

        public MainPage(Board game)
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);

            // Construct new board
            Controller = new GameViewController()
            {
                GameBoard = game,
            };

            // Start timer for automatic updates
            Xamarin.Forms.Device.StartTimer(TimeSpan.FromMilliseconds(250), TimerFunc);
        }

        private void OnPainting(object sender, SKPaintSurfaceEventArgs e)
        {
            Controller.Paint(sender, e);
            this.btnEndTurn.IsEnabled = Controller.GameBoard.CurrentPlayer.IsHuman;
        }

        /// <summary>
        /// If we're playing a bot, take one turn every 250 ms
        /// </summary>
        bool TimerFunc()
        {
            try
            {
                // Clear all status
                Controller.ActionMessage = "";
                Controller.CurrentAttack = null;

                // If the current player is a bot, do another attack
                if (!Controller.GameBoard.CurrentPlayer.IsHuman)
                {
                    var plan = Controller.GameBoard.CurrentPlayer.Bot.PickNextAttack(Controller.GameBoard, Controller.GameBoard.CurrentPlayer);
                    if (plan == null)
                    {
                        Controller.GameBoard.EndTurn();
                        Controller.StatusTime = DateTime.UtcNow;
                        cvGameCanvas.InvalidateSurface();
                    }
                    else
                    {
                        ExecuteAttackPlan(plan);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }
            return true;
        }

        private void ExecuteAttackPlan(AttackPlan plan)
        {
            switch (Controller.ExecuteAttackPlan(plan))
            {
                case GameViewController.GameAttackResult.GameOver:
                    Controller.ActionMessage = "Winner: " + Controller.GameBoard.Winner.Color.ToString();
                    var pg = new GameOverPage(Controller.GameBoard);
                    Navigation.PushModalAsync(pg);
                    break;
                default:
                    cvGameCanvas.InvalidateSurface();
                    break;
            }
        }

        void EndTurn_Clicked(object sender, System.EventArgs e)
        {
            Controller.GameBoard.EndTurn();
            cvGameCanvas.InvalidateSurface();
        }

        private void CvGameCanvas_Touch(object sender, SKTouchEventArgs e)
        {
            Controller.HandleTouch(sender, e);
            cvGameCanvas.InvalidateSurface();
        }
    }
}
