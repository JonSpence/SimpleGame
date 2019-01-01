using System;

namespace GameLib
{
    public class Zone
    {
        public int X { get; set; }
        public int Y { get; set; }
        public Player Owner { get; set; }
        public int Strength { get; set; }
    }
}
