using System;
using System.Collections.Generic;

namespace GameLib
{
    public class Zone
    {
        public int X { get; set; }
        public int Y { get; set; }
        public Player Owner { get; set; }
        public int Strength { get; set; }
        public List<Zone> Neighbors { get; set; }


        public bool IsNeighbor(Zone neighbor)
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
    }
}
