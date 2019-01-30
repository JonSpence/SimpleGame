using GameLib.Animations;
using GameLib.Interfaces;
using GameLib.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameLib.Rules
{
    public class AlwaysWin : IBattleRule
    {
        public ActionResult Attack(Board b, Player p, AttackPlan plan)
        {
            // Basic tests
            if (b.AttackIsInvalid(plan))
            {
                return ActionResult.INVALID;
            }

            // Always win
            var r = new ActionResult()
            {
                AttackWasInvalid = false,
                Plan = plan,
                AttackSucceeded = true,
                Animations = new List<BaseAnimation>()
            };

            // Add battle animation
            r.Animations.Add(new AttackAnimation(b, plan.Attacker, plan.Defender, new System.Threading.Tasks.Task(() =>
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
                    plan.Attacker.Strength = 1;
                })));
            return r;
        }
    }
}
