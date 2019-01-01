using GameLib.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameLib.Rules
{
    public class RandomAnywhere : IReinforcementRule
    {
        public void Reinforce(Board b, Player p, int reinforcements)
        {
            for (int i = 0; i < reinforcements; i++)
            {
                var toReinforce = b.Random.Next(p.Zones.Count);
                var z = p.Zones[toReinforce];
                z.Strength++;
            }
        }
    }
}
