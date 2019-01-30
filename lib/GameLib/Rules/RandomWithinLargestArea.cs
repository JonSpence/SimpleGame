using GameLib.Animations;
using GameLib.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameLib.Rules
{
    public class RandomWithinLargestArea : IReinforcementRule
    {
        public ReinforceAnimation Reinforce(Board b, Player p, int reinforcements)
        {
            ReinforceAnimation anim = new ReinforceAnimation(b);
            var area = b.GetLargestArea(p);
            b.TryReinforce(area, reinforcements, anim);
            return anim;
        }
    }
}
