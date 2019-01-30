using GameLib.Animations;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameLib.Interfaces
{
    public interface IReinforcementRule
    {
        ReinforceAnimation Reinforce(Board b, Player p, int reinforcements);
    }
}
