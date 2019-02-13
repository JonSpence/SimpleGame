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
        //public int newNumPlayers = 3;

        public NewGameDialog()
        {
            InitializeComponent();
        }

        // Allow access to the private number of new players through a public method
        public static int NewNumPlayers()
        {
            //int numPlayers = comboBox1_SelectedIndexChanged(); // NOT WORKING
            int numPlayers = 4; // WORKING
            return numPlayers;
        }

        // Allow access to the private width of new game board through a public method
        public static int NewBoardWidth()
        {
            //int width = comboBox2_SelectedIndexChanged(); // NOT WORKING
            int width = 10; // WORKING
            return width;
        }

        // Allow access to the private height of new game board through a public method
        public static int NewBoardHeight()
        {
            //int height = comboBox3_SelectedIndexChanged(); // NOT WORKING
            int height = 10; // WORKING
            return height;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}
