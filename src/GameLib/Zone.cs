using System;
using System.Collections.Generic;
using System.Linq;

namespace GameLib
{
    public class Zone
    {
        public int X { get; set; }
        public int Y { get; set; }
        public Player Owner { get; set; }
        public int Strength { get; set; }
        public List<Zone> Neighbors { get; set; }


        public bool IsNeighborOf(Zone neighbor)
        {
            if (neighbor.X == X)
            {
                return Math.Abs(neighbor.Y - Y) == 1;
            }
            else if (neighbor.Y == Y)
            {
                return Math.Abs(neighbor.X - X) == 1;
            }
            return false;
        }

        public bool BordersAnEnemy()
        {
            return (from n in Neighbors where n.Owner != Owner select n).Any();
        }
    }
}
