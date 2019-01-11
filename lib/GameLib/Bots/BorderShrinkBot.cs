using System;
using GameLib.Interfaces;
using GameLib.Messages;
using System.Linq;

namespace GameLib.Bots
{
    /// <summary>
    /// This bot aims to shrink its borders as much as possible by prioritizing attacks that give it more defensible space
    /// </summary>
    public class BorderShrinkBot : IBot
    {
        AttackPlan IBot.PickNextAttack(Board b, Player p)
        {
            var plans = b.GetPossibleAttacks(p);
            if (plans == null) return null;

            // We want to attack the zone that has the fewest connections to other players.
            // If the zone has mostly connections back to the border-shrink-bot, attacking
            // it will shrink our border space.
            return (from plan
                in plans

                    // Limit attacks to ones we are likely to win
                    where (plan.Attacker.Strength > 5)

                    orderby ConnectionsToOtherPlayers(plan.Defender, plan.Attacker) ascending
                    select plan).FirstOrDefault();
        }

        /// <summary>
        /// Count connections to someone other than the attacker
        /// </summary>
        /// <returns>The to other players.</returns>
        /// <param name="def">Def.</param>
        /// <param name="atk">Atk.</param>
        public int ConnectionsToOtherPlayers(Zone def, Zone atk)
        {
            return (from n in def.Neighbors where n.Owner != atk.Owner select n).Count();
        }
    }
}
