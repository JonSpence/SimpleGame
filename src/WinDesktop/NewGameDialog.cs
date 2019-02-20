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

        /// <summary>
        /// Allow access to the private difficulty of new game board through a public method
        /// </summary>
        /// <returns></returns>
        public string NewGameDifficulty()
        {
            //return Int32.Parse(cbxGameHeight.SelectedItem.ToString());
            return cbxGameDifficulty.SelectedItem.ToString();
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
