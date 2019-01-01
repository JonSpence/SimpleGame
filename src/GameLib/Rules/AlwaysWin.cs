using GameLib.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameLib.Rules
{
    public class AlwaysWin : IBattleRule
    {
        public bool Attack(Board b, Zone attacker, Zone defender)
        {
            // Basic tests
            if (attacker == null || defender == null) return false;
            if (attacker.Owner == null) return false;
            if (attacker.Strength <= 1) return false;
            if (!attacker.Neighbors.Contains(defender)) return false;
            if (attacker.Owner == defender.Owner) return false;

            // Always win
            defender.Owner.Zones.Remove(defender);
            defender.Owner = attacker.Owner;
            attacker.Owner.Zones.Add(defender);
            defender.Strength = attacker.Strength - 1;
            attacker.Strength = 1;
            return true;
        }
    }
}
