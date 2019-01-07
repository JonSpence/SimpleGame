using GameLib.Interfaces;
using GameLib.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameLib.Bots
{
    public class RandomBot : IBot
    {
        public AttackPlan PickNextAttack(Board b, Player p)
        {
            var plans = b.GetPossibleAttacks(p);

            // Pick a random attack
            if (plans?.Count > 0)
            {
                return plans[b.Random.Next(plans.Count)];
            }

            // Nothing
            return null;
        }
    }
}
