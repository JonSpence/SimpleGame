using GameLib.Animations;
using GameLib.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameLib.Rules
{
    public class RandomAnywhere : IReinforcementRule
    {
        public ReinforceAnimation Reinforce(Board b, Player p, int reinforcements)
        {
            ReinforceAnimation anim = new ReinforceAnimation(b);
            b.TryReinforce(p.Zones, reinforcements, anim);
            return anim;
        }
    }
}
