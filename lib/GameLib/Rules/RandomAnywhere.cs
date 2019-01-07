using GameLib.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameLib.Rules
{
    public class RandomAnywhere : IReinforcementRule
    {
        public void Reinforce(Board b, Player p, int reinforcements)
        {
            b.TryReinforce(p.Zones, reinforcements);
        }
    }
}
