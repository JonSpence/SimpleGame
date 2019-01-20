using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameLib
{
    public class Render
    {
        public Render(Board b)
        {
            _game_board = b;
            _view_map = new Dictionary<SKRectI, Zone>();
        }

        private Dictionary<SKRectI, Zone> _view_map;
        private Board _game_board;


        private void OnPainting(object sender, SKPaintSurfaceEventArgs e)
        {
            var canvas = e.Surface.Canvas;
            canvas.GetDeviceClipBounds(out SKRectI bounds);
            var new_hit_map = new Dictionary<SKRectI, Zone>();

            // Font info
            //Font drawFont = new Font("Arial", 16);
            //SolidBrush drawBrush = new SolidBrush(Color.Black);

            // Reserve room for status information
            var status_height = 150;
            var board_height = bounds.Height - status_height;
            var board_width = bounds.Width;
            /*
            // Draw status
            e.Graphics.DrawString($"Turn: Player {GameBoard.Players[GameBoard.CurrentTurn].Name}", drawFont, drawBrush, new PointF(0, 0));
            e.Graphics.DrawString(StatusMessage, drawFont, drawBrush, new PointF(0, 30));

            // Figure out cell height and width
            float cellHeight = board_height * 1.0f / GameBoard.Height;
            float cellWidth = board_width * 1.0f / GameBoard.Width;

            // Text settings
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
                RectangleF r = new RectangleF(cellWidth * z.X, status_height + cellHeight * z.Y, cellWidth, cellHeight);
                e.Graphics.FillRectangle(brush, r);

                // Draw border if attacking
                if (isAttacking)
                {
                    e.Graphics.DrawRectangle(new Pen(Color.Black), r.X, status_height + r.Y, cellWidth - 1, cellHeight - 1);
                }

                // Draw strength centered in the box
                e.Graphics.DrawString(z.Strength.ToString(), drawFont, drawBrush, r, drawFormat);

                // Keep track of screen view map
                dict[r] = z;
            }

            // Update viewmap
            ViewMap = dict;
            */
        }
    }
}
