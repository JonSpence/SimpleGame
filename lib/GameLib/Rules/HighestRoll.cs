using GameLib.Animations;
using GameLib.Interfaces;
using GameLib.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameLib.Rules
{
    public class HighestRoll : IBattleRule
    {
        public ActionResult Attack(Board b, Player p, AttackPlan plan)
        {
            // Basic tests
            if (b.AttackIsInvalid(plan)) { 
                return ActionResult.INVALID;
            }

            // Roll dice based on strength - note that one unit must stay home for attacker
            int attacker_roll = b.Roll(plan.Attacker.Strength - 1, 6);
            int defender_roll = b.Roll(plan.Defender.Strength, 6);
            bool wins = attacker_roll > defender_roll;

            // Handle the best roll
            var r = new ActionResult()
            {
                AttackWasInvalid = false,
                Plan = plan,
                AttackSucceeded = wins,
                Animations = new List<BaseAnimation>()
            };
            r.Animations.Add(new AttackAnimation(b, plan.Attacker, plan.Defender, new System.Threading.Tasks.Task(() =>
                {
                    if (wins)
                    {
                        if (plan.Defender.Owner != null)
                        {
                            plan.Defender.Owner.Zones.Remove(plan.Defender);
                            if (plan.Defender.Owner.Zones.Count == 0)
                            {
                                plan.Defender.Owner.IsDead = true;
                            }
                        }
                        plan.Defender.Owner = plan.Attacker.Owner;
                        plan.Attacker.Owner.Zones.Add(plan.Defender);
                        plan.Defender.Strength = plan.Attacker.Strength - 1;
                    }
                    plan.Attacker.Strength = 1;
                })));
            return r;
        }
    }
}
