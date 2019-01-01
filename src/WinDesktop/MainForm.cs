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
        public Dictionary<RectangleF, Zone> ViewMap { get; set; }
        public Zone Attacking { get; set; }
        public Zone Defending { get; set; }

        public MainForm()
        {
            InitializeComponent();
            ResizeRedraw = true;
            DoubleBuffered = true;

            // Basic setup
            GameBoard = Board.NewBoard(10, 10, 6);
        }

        private void MainForm_Paint(object sender, PaintEventArgs e)
        {
            Dictionary<RectangleF, Zone> dict = new Dictionary<RectangleF, Zone>();

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
                bool isAttacking = (z == Attacking || z == Defending);

                // Figure out color to use
                Brush brush = null;
                if (z.Owner == null)
                {
                    brush = new SolidBrush(Color.DarkGray);
                }
                else
                {
                    Color c = z.Owner.Color;
                    if (isAttacking)
                    {
                        c = Lighten(c, 0.7f);
                    }
                    brush = new SolidBrush(c);
                }

                // Draw rectangle
                RectangleF r = new RectangleF(cellWidth * z.X, cellHeight * z.Y, cellWidth, cellHeight);
                e.Graphics.FillRectangle(brush, r);

                // Draw border if attacking
                if (isAttacking)
                {
                    e.Graphics.DrawRectangle(new Pen(Color.Black), r.X, r.Y, cellWidth-1, cellHeight-1);
                }

                // Draw strength centered in the box
                e.Graphics.DrawString(z.Strength.ToString(), drawFont, drawBrush, r, drawFormat);

                // Keep track of screen view map
                dict[r] = z;
            }

            // Update viewmap
            ViewMap = dict;
        }

        private void MainForm_Click(object sender, EventArgs e)
        {
            MouseEventArgs m = e as MouseEventArgs;
            if (m != null)
            {
                var zone = GetClickedZone(m.Location);
                //InvalidateZone(this.Attacking);
                this.Attacking = zone;
                //InvalidateZone(this.Attacking);
                this.Invalidate();
            }
        }

        //private void InvalidateZone(Zone z)
        //{
        //    if (z != null)
        //    {
        //        foreach (var kvp in ViewMap)
        //        {
        //            if (kvp.Value == z)
        //            {
        //                this.Invalidate(new Rectangle(((int)kvp.Key.X) - 1, ((int)kvp.Key.Y) - 1, ((int)kvp.Key.Width) + 2, ((int)kvp.Key.Height) + 2));
        //            }
        //        }
        //    }
        //}

        public static Color Lighten(Color color, float pct)
        {
            float red = (float)color.R;
            float green = (float)color.G;
            float blue = (float)color.B;

            red = (255 - red) * pct + red;
            green = (255 - green) * pct + green;
            blue = (255 - blue) * pct + blue;

            return Color.FromArgb(color.A, (int)red, (int)green, (int)blue);
        }

        private Zone GetClickedZone(Point location)
        {
            foreach (var kvp in ViewMap)
            {
                if (kvp.Key.Contains(location))
                {
                    return kvp.Value;
                }
            }
            return null;
        }
    }
}
