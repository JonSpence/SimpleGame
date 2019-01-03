using System;
using System.Collections.Generic;
using System.Text;

namespace GameLib.Messages
{
    public class AttackPlan
    {
        public Zone Attacker { get; set; }
        public Zone Defender { get; set; }
    }
}
