using GameLib.Messages;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameLib.Interfaces
{
    public interface IBattleRule
    {
        ActionResult Attack(Board b, Player p, AttackPlan plan);
    }
}
