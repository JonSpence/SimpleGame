using System;
using System.Collections.Generic;
using System.Text;

namespace GameLib.Interfaces
{
    public interface IReinforcementRule
    {
        void Reinforce(Board b, Player p, int reinforcements);
    }
}
