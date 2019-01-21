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
            Controller.Paint(sender, e.Surface.Canvas);
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
                    HandleResult(Controller.TakeBotAction());
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }
            return true;
        }

        private void HandleResult(GameViewController.GameAttackResult r)
        {
            switch (r)
            {
                case GameViewController.GameAttackResult.GameOver:
                    Controller.ActionMessage = "Winner: " + Controller.GameBoard.Winner.Color.ToString();
                    var pg = new GameOverPage(Controller.GameBoard);
                    Navigation.PushModalAsync(pg);
                    break;
                case GameViewController.GameAttackResult.Invalid:
                    // What to do here?
                    break;
                default:
                    cvGameCanvas.InvalidateSurface();
                    break;
            }
        }

        private void CvGameCanvas_Touch(object sender, SKTouchEventArgs e)
        {
            HandleResult(Controller.HandleTouch(sender, e.Location));
        }
    }
}
