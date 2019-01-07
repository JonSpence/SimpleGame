using GameLib.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameLib.Rules
{
    public class RandomWithinLargestArea : IReinforcementRule
    {
        public void Reinforce(Board b, Player p, int reinforcements)
        {
            var area = b.GetLargestArea(p);
            for (int i = 0; i < reinforcements; i++)
            {
                var toReinforce = b.Random.Next(area.Count);
                var z = area[toReinforce];
                z.Strength++;
            }
        }
    }
}
