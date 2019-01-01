using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace GameLib
{
    public class Player
    {
        public int Number { get; set; }
        public Color Color { get; set; }
        public List<Zone> Zones { get; set; }
    }
}
