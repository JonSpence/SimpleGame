using GameLib.Interfaces;
using GameLib.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameLib.Bots
{
    /// <summary>
    /// I was hired to do this tiny bit of work
    /// </summary>
    public class ThisIsTheBotIWasHiredToCreate : IBot
    {
        public AttackPlan PickNextAttack(Board b, Player p)
        {
            var plans = b.GetPossibleAttacks(p);
            if (plans == null) return null;

            return (from plan in plans where plan.Attacker.Strength > 5 && plan.Defender.Strength < 4 select plan).FirstOrDefault();
        }
    }
}
