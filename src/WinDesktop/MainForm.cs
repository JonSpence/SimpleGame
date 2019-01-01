using GameLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace WinDesktop
{
    public partial class MainForm : Form
    {
        public Board GameBoard { get; set; }

        public MainForm()
        {
            InitializeComponent();

            // Basic setup
            GameBoard = Board.NewBoard(10, 10, 6);
        }

        private void MainForm_Paint(object sender, PaintEventArgs e)
        {
            // Figure out cell height and width
            float cellHeight = this.ClientSize.Height * 1.0f / GameBoard.Height;
            float cellWidth = this.ClientSize.Width * 1.0f / GameBoard.Width;

            // Text settings
            Font drawFont = new Font("Arial", 16);
            SolidBrush drawBrush = new SolidBrush(Color.Black);
            StringFormat drawFormat = new StringFormat();
            drawFormat.Alignment = StringAlignment.Center;
            drawFormat.LineAlignment = StringAlignment.Center;

            // Draw each zone
            foreach (var z in GameBoard.Zones)
            {
                // Figure out color to use
                Brush brush = null;
                if (z.Owner == null)
                {
                    brush = new SolidBrush(Color.DarkGray);
                } else {
                    brush = new SolidBrush(z.Owner.Color);
                }

                // Draw rectangle
                RectangleF r = new RectangleF(cellWidth * z.X, cellHeight * z.Y, cellWidth, cellHeight);
                e.Graphics.FillRectangle(brush, r);
                //r.X, r.Y, cellWidth, cellHeight);

                // Draw strength centered in the box
                e.Graphics.DrawString(z.Strength.ToString(), drawFont, drawBrush, r, drawFormat);
            }
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            this.Invalidate();
        }
    }
}
