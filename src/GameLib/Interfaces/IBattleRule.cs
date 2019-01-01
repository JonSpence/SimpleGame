using System;
using System.Collections.Generic;
using System.Text;

namespace GameLib.Interfaces
{
    public interface IBattleRule
    {
        bool Attack(Board b, Zone attacker, Zone defender);
    }
}
