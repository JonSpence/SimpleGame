using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace GameLib
{
    public class Board
    {
        public int Width { get; private set; }
        public int Height { get; private set; }
        public int NumPlayers { get; private set; }
        public List<Zone> Zones { get; set;  }
        public List<Player> Players { get; set; }

        public static Color[] COLOR_LIST = new Color[] { Color.ForestGreen, Color.CornflowerBlue, Color.Red, Color.Yellow, Color.MediumPurple, Color.Orange, Color.RosyBrown, Color.GhostWhite };

        /// <summary>;
        /// Initialize a new game board
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public static Board NewBoard(int width, int height, int players)
        {
            Board b = new Board();
            b.Width = width;
            b.Height = height;
            b.NumPlayers = players;
            b.Players = new List<Player>();
            b.Zones = new List<Zone>();

            // Setup players
            for (int i = 0; i < Math.Min(players, COLOR_LIST.Length); i++)
            {
                Player p = new Player()
                {
                    Color = COLOR_LIST[i],
                    Number = i,
                    Zones = new List<Zone>()
                };
                b.Players.Add(p);
            }

            // Construct basic zones
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    Zone z = new Zone()
                    {
                        X = i,
                        Y = j,
                        Owner = null
                    };
                    b.Zones.Add(z);
                }
            }

            // Randomly assign zones to players
            List<Zone> unassigned = new List<Zone>(b.Zones);
            int nextPlayer = 0;
            Random r = new Random();
            while (unassigned.Count > 0)
            {
                var picked = r.Next(unassigned.Count - 1);
                var zone = unassigned[picked];
                zone.Owner = b.Players[nextPlayer];
                unassigned.RemoveAt(picked);
                nextPlayer++;
                if (nextPlayer >= b.Players.Count) nextPlayer = 0;
            }
            return b;
        }
    }
}
