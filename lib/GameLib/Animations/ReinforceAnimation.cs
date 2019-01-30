using System;
using System.Collections.Generic;
using System.Text;
using SkiaSharp;

namespace GameLib.Animations
{
    public class ReinforceAnimation : BaseAnimation
    {
        public Dictionary<Zone, int> Reinforcements { get; private set; }

        /// <summary>
        /// Construct an animation to reinforce these zones
        /// </summary>
        /// <param name="ToReinforce"></param>
        public ReinforceAnimation(Board b)
            : base(b)
        {
            Reinforcements = new Dictionary<Zone, int>();
            DurationMs = 500;
        }

        /// <summary>
        /// When animation starts, add strength to all zones
        /// </summary>
        protected override void StartAnimation()
        {
            foreach (var kvp in Reinforcements)
            {
                kvp.Key.Strength += kvp.Value;
            }
        }

        /// <summary>
        /// When animation ends, skip to next non-dead player
        /// </summary>
        protected override void FinishAnimation()
        {
            GameBoard.NextPlayerTurn();
        }

        /// <summary>
        /// Flash the color of the zone and gradually fade back to normal
        /// </summary>
        /// <param name="z"></param>
        /// <returns></returns>
        public override SKColor RecolorZone(Zone z)
        {
            // Determine base color for zone
            var color = base.RecolorZone(z);

            // If it's part of the reinforcements list, lerp its color
            if (Reinforcements.ContainsKey(z))
            {
                return GameViewController.Lighten(color, (1.0f - _lerp));
            }
            return color;
        }

        /// <summary>
        /// Add a unit to a zone for this animation
        /// </summary>
        /// <param name="z"></param>
        public void AddUnit(Zone z)
        {
            if (!Reinforcements.ContainsKey(z))
            {
                Reinforcements[z] = 1;
            }
            else
            {
                Reinforcements[z] = Reinforcements[z] + 1;
            }
        }
    }
}
