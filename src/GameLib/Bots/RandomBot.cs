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
            // Pick a territory that has more than one unit
            var zones = (from z in p.Zones where z.Strength > 1 select z).ToList();
            if (zones.Count == 0) return null;

            // Pick a territory where a neighbor can be attacked
            List<AttackPlan> plans = new List<AttackPlan>();
            foreach (var z in zones)
            {
                foreach (var n in z.Neighbors)
                {
                    if (n.Owner != p)
                    {
                        plans.Add(new AttackPlan() {
                            Attacker = z,
                            Defender = n
                        });
                    }
                }
            }

            // Pick a random attack
            if (plans.Count > 0)
            {
                return plans[b.Random.Next(plans.Count)];
            }

            // Nothing
            return null;
        }
    }
}
