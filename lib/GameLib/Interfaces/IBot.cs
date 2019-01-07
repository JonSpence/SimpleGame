using GameLib.Messages;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameLib.Interfaces
{
    public interface IBot
    {
        AttackPlan PickNextAttack(Board b, Player p);
    }
}
