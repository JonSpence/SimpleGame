using System;
using System.Collections.Generic;
using GameLib;
using GameLib.Rules;
using Xamarin.Forms;

namespace SimpleGameApp
{
    public partial class NewGame : ContentPage
    {
        public NewGame()
        {
            InitializeComponent();

            NavigationPage.SetHasNavigationBar(this, false);

            ddlDiceRule.SelectedItem = ddlDiceRule.Items[0];
            ddlDimensions.SelectedItem = ddlDimensions.Items[0];
            ddlReinforcements.SelectedItem = ddlReinforcements.Items[0];
        }

        void StartGame_Clicked(object sender, System.EventArgs e)
        {
            Board b = Board.NewBoard(5, 10, 6);
            b.BattleRule = new RankedDice();
            Navigation.PushModalAsync(new MainPage(b));
        }
    }
}
