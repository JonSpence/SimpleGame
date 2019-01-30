using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SkiaSharp;

namespace GameLib.Animations
{
    public class AttackAnimation : BaseAnimation
    {
        private Zone _attacker;
        private Zone _defender;
        private Task _update;

        public AttackAnimation(Board b, Zone attacker, Zone defender, Task UpdateBoardTask)
            : base(b)
        {
            _attacker = attacker;
            _defender = defender;
            _update = UpdateBoardTask;
            DurationMs = 250;
        }

        public override SKColor RecolorZone(Zone z)
        {
            var color = base.RecolorZone(z);
            if (z == _attacker)
            {
                return GameViewController.Lighten(color, (1.0f - _lerp) * 0.5f);
            }
            if (z == _defender)
            {
                return GameViewController.Lighten(color, (1.0f - _lerp) * 0.5f);
            }
            return color;
        }

        protected override void StartAnimation()
        {
            _update.RunSynchronously();
        }
    }
}
