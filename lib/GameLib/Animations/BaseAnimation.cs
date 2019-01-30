using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameLib.Animations
{
    public class BaseAnimation
    {
        private DateTime? _start_time = null;
        private DateTime? _end_time = null;
        protected float _lerp = 0.0f;

        /// <summary>
        /// Need to reference our board
        /// </summary>
        /// <param name="b"></param>
        public BaseAnimation(Board b)
        {
            GameBoard = b;
        }

        /// <summary>
        /// Duration in milliseconds
        /// </summary>
        public float DurationMs { get; protected set; }

        /// <summary>
        /// Reference to the board
        /// </summary>
        public Board GameBoard { get; protected set; }

        /// <summary>
        /// Override this to change colors of zones during the animation
        /// </summary>
        /// <param name="z"></param>
        /// <returns></returns>
        public virtual SKColor RecolorZone(Zone z)
        {
            return GameViewController.GetBaseColor(z);
        }

        /// <summary>
        /// This function will be called when the animation is started
        /// </summary>
        protected virtual void StartAnimation()
        {
        }

        /// <summary>
        /// This function will be called when the animation is finished
        /// </summary>
        protected virtual void FinishAnimation()
        {
        }

        /// <summary>
        /// Call this once per frame.
        /// 
        /// When this function returns true, retire this animation and start the next one.
        /// </summary>
        public virtual bool Heartbeat()
        {
            var now = DateTime.UtcNow;

            // Capture start time
            if (_start_time == null)
            {
                _start_time = now;
                _end_time = _start_time.Value.AddMilliseconds(DurationMs);
                _lerp = 0.0f;
                StartAnimation();
            }
            else
            {
                var ts = now - _start_time.Value;
                _lerp = (float)(ts.TotalMilliseconds / DurationMs);
            }

            // Retire this animation when we're past the expiration time
            var finished = (_end_time <= now);
            if (finished)
            {
                FinishAnimation();
            }
            return finished;
        }
    }
}
