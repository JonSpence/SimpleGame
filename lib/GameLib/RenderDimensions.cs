using System;
using System.Collections.Generic;
using SkiaSharp;

namespace GameLib
{
    public class RenderDimensions
    {
        private int _width, _height;
        private Board _board;

        public float CellHeight { get; private set; }
        public float TextSize { get; private set; }
        public float StatusHeight { get; private set; }
        public float BoardHeight { get; private set; }
        public float PlayerWidth { get; private set; }
        public float PlayerStatusPadding { get; private set; }
        public float PlayerBoxPadding { get; private set; }
        public float PlayerBoxBottom { get; private set; }
        public float CellWidth { get; private set; }
        public SKRoundRect EndTurnRect { get; private set; }
        public Dictionary<Zone, SKRect> ZoneToRect { get; private set; }

        public RenderDimensions(int width, int height, Board board)
        {
            _width = width;
            _height = height;
            _board = board;

            // Calculated values
            CellHeight = (height / (board.Height + 3));
            TextSize = CellHeight * 0.8f;
            StatusHeight = CellHeight * 1.5f;
            BoardHeight = height - StatusHeight;
            PlayerWidth = (width / (board.Players.Count));
            PlayerStatusPadding = CellHeight * 0.1f;
            PlayerBoxPadding = CellHeight * 0.2f;
            PlayerBoxBottom = CellHeight - PlayerBoxPadding;
            CellWidth = width / board.Width;

            // End turn rounded rectangle
            var r = new SKRect(0 + PlayerStatusPadding, height - (CellHeight * 1.5f) + PlayerStatusPadding, width - PlayerStatusPadding, height - PlayerStatusPadding);
            EndTurnRect = new SKRoundRect(r, PlayerStatusPadding, PlayerStatusPadding);

            // Precompute all zone rectangles
            ZoneToRect = new Dictionary<Zone, SKRect>();
            foreach (var z in board.Zones) { 
                r = new SKRect();
                r.Left = CellWidth * z.X;
                r.Top = StatusHeight + CellHeight * z.Y;
                r.Right = r.Left + CellWidth;
                r.Bottom = r.Top + CellHeight;
                ZoneToRect[z] = r;
            }
        }

        public bool Match(int width, int height, Board board)
        {
            return (width == _width && _height == height && _board == board);
        }
    }
}
