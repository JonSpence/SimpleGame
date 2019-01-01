using GameLib.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameLib.Rules
{
    public class RandomBorder : IReinforcementRule
    {
        public void Reinforce(Board b, Player p, int reinforcements)
        {
            var area = b.GetLargestArea(p);

            // Find border zones within this area
            var borders = (from z in area where z.BordersAnEnemy() select z).ToList();

            // Now add reinforcements
            for (int i = 0; i < reinforcements; i++)
            {
                var toReinforce = b.Random.Next(borders.Count);
                var z = borders[toReinforce];
                z.Strength++;
            }
        }
    }
}
