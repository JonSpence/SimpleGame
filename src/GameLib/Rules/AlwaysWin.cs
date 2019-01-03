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
        public BattleResult Attack(Board b, Player p, AttackPlan plan)
        {
            // Basic tests
            if (plan.Attacker == null
                || plan.Defender == null
                || plan.Attacker.Owner == null
                || plan.Attacker.Owner != p
                || plan.Attacker.Strength <= 1
                || !plan.Attacker.Neighbors.Contains(plan.Defender)
                || plan.Attacker.Owner == plan.Defender.Owner)
            {
                return BattleResult.INVALID;
            }

            // Always win
            return new BattleResult()
            {
                AttackWasInvalid = false,
                Plan = plan,
                AttackSucceeded = true,
                UpdateBoardTask = new System.Threading.Tasks.Task(() =>
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
                })
            };
        }
    }
}
