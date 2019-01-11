using GameLib.Interfaces;
using GameLib.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameLib.Rules
{
    public class RankedDice : IBattleRule
    {
        public BattleResult Attack(Board b, Player p, AttackPlan plan)
        {
            // Basic tests
            if (b.AttackIsInvalid(plan)) { 
                return BattleResult.INVALID;
            }

            // Roll one die per strength counter, sort by highest rolls descending
            var attacker_rolls = (from x in b.MultiRoll(plan.Attacker.Strength - 1, 6) orderby x descending select x).ToList();
            var defender_rolls = (from x in b.MultiRoll(plan.Defender.Strength, 6) orderby x descending select x).ToList();

            // Compare each die from attacker to defender
            // Defender wins ties
            // Attacker wins any excess attackers that were not paired to a defender
            int attacker_losses = 0, defender_losses = 0;
            for (int i = 0; i < attacker_rolls.Count; i++)
            {
                if (i < defender_rolls.Count)
                {
                    if (attacker_rolls[i] > defender_rolls[i])
                    {
                        defender_losses++;
                    }
                    else
                    {
                        attacker_losses++;
                    }
                }
                else
                {
                    defender_losses++;
                }
            }

            // Attacker wins if defender is reduced to zero
            var wins = (defender_losses >= plan.Defender.Strength);

            // Handle results
            return new BattleResult()
            {
                AttackWasInvalid = false,
                Plan = plan,
                AttackSucceeded = wins,
                UpdateBoardTask = new System.Threading.Tasks.Task(() =>
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
                        plan.Defender.Strength = plan.Attacker.Strength - attacker_losses - 1;
                        plan.Attacker.Strength = 1;
                    }
                    else
                    {
                        plan.Attacker.Strength = plan.Attacker.Strength - attacker_losses;
                        plan.Defender.Strength = plan.Defender.Strength - defender_losses;
                    }
                })
            };
        }
    }
}
