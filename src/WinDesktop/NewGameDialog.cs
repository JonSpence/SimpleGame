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

        // Allow access to the private number of new players through a public method
        public int NewNumPlayers()
        {
            
            return Int32.Parse(cbxNumPlayers.SelectedItem.ToString()); 
        }

        // Allow access to the private width of new game board through a public method
        public int NewBoardWidth()
        {

            return Int32.Parse(cbxGameWidth.SelectedItem.ToString());
        }

        // Allow access to the private height of new game board through a public method
        public int NewBoardHeight()
        {

            return Int32.Parse(cbxGameHeight.SelectedItem.ToString());
        }





    }
}
