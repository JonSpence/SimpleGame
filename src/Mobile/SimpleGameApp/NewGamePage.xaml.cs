using System;
using System.Collections.Generic;
using GameLib;
using GameLib.Bots;
using GameLib.Interfaces;
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

            ddlDiceRule.SelectedItem = ddlDiceRule.Items[2];
            ddlDimensions.SelectedItem = ddlDimensions.Items[3];
            ddlReinforcements.SelectedItem = ddlReinforcements.Items[2];
            ddlDifficulty.SelectedItem = ddlDifficulty.Items[0];
        }

        void StartGame_Clicked(object sender, System.EventArgs e)
        {

            // Figure out size of board
            Board b = null;
            switch (ddlDimensions.SelectedIndex)
            {
                case 0: b = Board.NewBoard(3, 6, 3); break;
                case 1: b = Board.NewBoard(4, 9, 4); break;
                case 2: b = Board.NewBoard(5, 11, 5); break;
                default: b = Board.NewBoard(7, 13, 6); break;
            }

            // Figure out dice rule
            switch (ddlDiceRule.SelectedIndex)
            {
                case 0: b.BattleRule = new AlwaysWin(); break;
                case 1: b.BattleRule = new HighestRoll(); break;
                default: b.BattleRule = new RankedDice(); break;
            }

            // Figure out reinforcement rule
            switch (ddlReinforcements.SelectedIndex)
            {
                case 0: b.ReinforcementRule = new RandomAnywhere(); break;
                case 1: b.ReinforcementRule = new RandomWithinLargestArea(); break;
                default: b.ReinforcementRule = new RandomBorder(); break;
            }

            // Full auto play means all players are bots
            if (!cbxAutoPlay.IsToggled)
            {
                b.Players[0].IsHuman = true;
            }

            // Computer Difficulty
            IBot bot = null;
            switch (ddlDifficulty.SelectedIndex)
            {
                case 0: bot = new BorderShrinkBot(); break;
                case 1: bot = new TurtleBot(); break;
                default: bot = new RandomBot(); break;
            }
            foreach (var p in b.Players)
            {
                if (!p.IsHuman)
                {
                    p.Bot = bot;
                }
            }

            // Start game
            Navigation.PushModalAsync(new MainPage(b));
        }
    }
}
