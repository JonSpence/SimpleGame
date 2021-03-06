﻿using GameLib.Bots;
using GameLib.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinDesktop
{
    public partial class NewGameDialog : Form
    {
        

        public NewGameDialog()
        {
            InitializeComponent();
            cbxNumPlayers.SelectedIndex = 0;
            cbxGameWidth.SelectedIndex = 0;
            cbxGameHeight.SelectedIndex = 0;
            cbxGameDifficulty.SelectedIndex = 1;
        }

        /// <summary>
        /// Allow access to the private number of new players through a public method
        /// </summary>
        /// <returns></returns>
        public int NewNumPlayers()
        {
            
            return Int32.Parse(cbxNumPlayers.SelectedItem.ToString()); 
        }

        /// <summary>
        /// Allow access to the private width of new game board through a public method
        /// </summary>
        /// <returns></returns>
        public int NewBoardWidth()
        {
            return Int32.Parse(cbxGameWidth.SelectedItem.ToString());
        }

        /// <summary>
        /// Allow access to the private height of new game board through a public method
        /// </summary>
        /// <returns></returns>
        public int NewBoardHeight()
        {
            return Int32.Parse(cbxGameHeight.SelectedItem.ToString());
        }



        public string[] difficultyText = new string[]
        {
            "Easy","Medium","Hard","Very Hard"
        };

        public Type[] difficultyBot = new Type[]
        {
            typeof (RandomBot), typeof (CautiousBot), typeof (TurtleBot), typeof (BorderShrinkBot)
        };


        /// <summary>
        /// Allow access to the private difficulty of new game board through a public method
        /// </summary>
        /// <returns></returns>
        public IBot NewGameDifficulty()
        {

            for (int i = 0; i < difficultyText.Length; i++)
            {
                if (string.Equals(cbxGameDifficulty.SelectedItem.ToString(), difficultyText[i],StringComparison .OrdinalIgnoreCase))
                {
                    var bot = Activator.CreateInstance(difficultyBot[i]) as IBot;
                    Console.WriteLine($"Difficulty selected was {difficultyText[i]}, Bot is {difficultyBot[i]}");
                    return bot;
                }
            }

            return new RandomBot();
        }


        private void cbxGameHeight_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }
    }
}
