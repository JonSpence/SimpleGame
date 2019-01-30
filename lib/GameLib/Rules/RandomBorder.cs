using GameLib.Animations;
using GameLib.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameLib.Rules
{
    public class RandomBorder : IReinforcementRule
    {
        public ReinforceAnimation Reinforce(Board b, Player p, int reinforcements)
        {
            ReinforceAnimation anim = new ReinforceAnimation(b);

            // First try reinforcing borders
            var area = b.GetLargestArea(p);
            var borders = (from z in area where z.BordersAnEnemy() select z).ToList();
            int remaining = b.TryReinforce(borders, reinforcements, anim);

            // If there's more, try largest area
            if (remaining > 0) {
                b.TryReinforce(area, remaining, anim);
            }
            return anim;
        }
    }
}
