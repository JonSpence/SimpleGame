using System;
using System.Linq;
using GameLib.Interfaces;
using GameLib.Messages;

namespace GameLib.Bots
{
    public class CautiousBot : IBot
    {
        public AttackPlan PickNextAttack(Board b, Player p)
        {
            var plans = b.GetPossibleAttacks(p);
            if (plans == null) return null;

            // Sort attacks by strength
            return (from plan 
                in plans 
                where (plan.Attacker.Strength >= plan.Defender.Strength - 1) || (plan.Attacker.Strength > 5)
                    orderby plan.Attacker.Strength - plan.Defender.Strength descending
                select plan).FirstOrDefault();
        }
    }
}
