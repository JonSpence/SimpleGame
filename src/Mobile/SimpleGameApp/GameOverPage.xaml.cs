using System;
using System.Collections.Generic;
using GameLib;
using Xamarin.Forms;

namespace SimpleGameApp
{
    public partial class GameOverPage : ContentPage
    {
        public GameOverPage(Board b)
        {
            InitializeComponent();
            lblGameOver.Text = "Game Over - Winner " + b.Winner.Color.ToString();
        }

        void Handle_Clicked(object sender, System.EventArgs e)
        {
            Navigation.PopModalAsync();
            Navigation.PopModalAsync();
        }
    }
}
