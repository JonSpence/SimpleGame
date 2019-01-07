using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using GameLib.Interfaces;

namespace GameLib
{
    public class Player
    {
        /// <summary>
        /// Just an ID number
        /// </summary>
        public int Number { get; set; }

        /// <summary>
        /// A friendlier name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// True if the player is human, false if it is a bot
        /// </summary>
        public bool IsHuman { get; set; }

        /// <summary>
        /// True if the player is dead (has no territories left)
        /// </summary>
        public bool IsDead { get; set; }

        /// <summary>
        /// The bot AI selected for this player, if a computer
        /// </summary>
        public IBot Bot { get; set; }

        /// <summary>
        /// The official color of this player
        /// </summary>
        public Color Color { get; set; }

        /// <summary>
        /// List of all zones owned by this player, for convenience
        /// </summary>
        public List<Zone> Zones { get; set; }
    }
}
