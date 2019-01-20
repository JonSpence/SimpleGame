using GameLib.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameLib.Rules
{
    public class RandomBorder : IReinforcementRule
    {
        public void Reinforce(Board b, Player p, int reinforcements)
        {
            // First try reinforcing borders
            var area = b.GetLargestArea(p);
            var borders = (from z in area where z.BordersAnEnemy() select z).ToList();
            int remaining = b.TryReinforce(borders, reinforcements);

            // If there's more, try largest area
            if (remaining > 0) {
                b.TryReinforce(area, remaining);
            }
        }
    }
}
