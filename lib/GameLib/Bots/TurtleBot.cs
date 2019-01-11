using System;
using System.Linq;
using GameLib.Interfaces;
using GameLib.Messages;

namespace GameLib.Bots
{
    public class TurtleBot : IBot
    {
        public AttackPlan PickNextAttack(Board b, Player p)
        {
            var plans = b.GetPossibleAttacks(p);
            if (plans == null) return null;

            // Only attack when we are at max strength
            return (from plan
                in plans
                    where plan.Attacker.Strength == plan.Attacker.MaxStrength
                    select plan).FirstOrDefault();
        }
    }
}
