using GameLib.Animations;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GameLib.Messages
{
    public class ActionResult
    {
        public List<BaseAnimation> Animations { get; set; }

        /// <summary>
        /// This gets set to false if the user tried to attack something invalid
        /// </summary>
        public bool AttackWasInvalid { get; set; }

        /// <summary>
        /// The original attack plan
        /// </summary>
        public AttackPlan Plan { get; set; }

        /// <summary>
        /// True if the attack succeeded
        /// </summary>
        public bool AttackSucceeded { get; set; }

        /// <summary>
        /// The UI should call this task when the attack has been executed to update the board
        /// </summary>
        public Task UpdateBoardTask { get; set; }

        /// <summary>
        /// Return this result when the attack could not be executed because it was invalid
        /// </summary>
        public static ActionResult INVALID = new ActionResult() { AttackWasInvalid = true };
    }
}
