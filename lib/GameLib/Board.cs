using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using GameLib.Rules;
using System.Linq;
using GameLib.Interfaces;
using GameLib.Bots;
using GameLib.Messages;

namespace GameLib
{
    public class Board
    {
        /// <summary>
        /// Settings fixed on startup
        /// </summary>
        public int Width { get; private set; }
        public int Height { get; private set; }
        public int NumPlayers { get; private set; }
        public Random Random { get; private set; }
        public int Round { get; private set; }

        /// <summary>
        /// Data that changes over the course of the game
        /// </summary>
        public List<Zone> Zones { get; set; }
        public List<Player> Players { get; set; }
        public IBattleRule BattleRule { get; set; }
        public IReinforcementRule ReinforcementRule { get; set; }
        public int CurrentTurn { get; set; }

        public static Color[] COLOR_LIST = new Color[] { Color.ForestGreen, Color.CornflowerBlue, Color.Red, Color.DarkMagenta, Color.MediumPurple, Color.DarkOrange, Color.RosyBrown, Color.DarkOrchid };

        /// <summary>;
        /// Initialize a new game board
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public static Board NewBoard(int width, int height, int players)
        {
            Board b = new Board();
            b.Width = width;
            b.Height = height;
            b.NumPlayers = players;
            b.Players = new List<Player>();
            b.Zones = new List<Zone>();
            b.BattleRule = new HighestRoll();
            b.ReinforcementRule = new RandomBorder();
            b.Random = new Random();
            b.Round = 0;

            // Random start player
            b.CurrentTurn = b.Random.Next(players);

            // Setup players
            for (int i = 0; i < Math.Min(players, COLOR_LIST.Length); i++)
            {
                Player p = new Player()
                {
                    Color = COLOR_LIST[i],
                    Name = COLOR_LIST[i].ToString(),
                    Number = i,
                    IsDead = false,
                    Bot = new RandomBot(),
                    IsHuman = false,
                    Zones = new List<Zone>()
                };
                b.Players.Add(p);
            }

            // Construct basic zones
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    // Zone size?
                    Zone z = new Zone()
                    {
                        X = i,
                        Y = j,
                        Strength = 1,
                        MaxStrength = b.Random.Next(7, 9),
                        Owner = null
                    };
                    b.Zones.Add(z);
                }
            }

            // Figure out neighbors
            foreach (var z in b.Zones)
            {
                z.Neighbors = (from n in b.Zones where z.IsNeighborOf(n) select n).ToList();
            }

            // Randomly assign zones to players
            List<Zone> unassigned = new List<Zone>(b.Zones);
            int nextPlayer = 0;
            while (unassigned.Count > 0)
            {
                var picked = b.Random.Next(unassigned.Count - 1);
                var zone = unassigned[picked];
                zone.Owner = b.Players[nextPlayer];
                zone.Owner.Zones.Add(zone);
                unassigned.RemoveAt(picked);
                nextPlayer++;
                if (nextPlayer >= b.Players.Count) nextPlayer = 0;
            }

            // Randomly reinforce zones
            var rule = new RandomAnywhere();
            int reinforcements = (int)(width * height * 3 / players);
            for (int i = 0; i < players; i++)
            {
                rule.Reinforce(b, b.Players[i], reinforcements);
            }
            return b;
        }

        /// <summary>
        /// Current player
        /// </summary>
        /// <returns></returns>
        public Player CurrentPlayer
        {
            get
            {
                return Players[CurrentTurn];
            }
        }

        /// <summary>
        /// Figure out the largest contiguous area owned by a player
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public List<Zone> GetLargestArea(Player p)
        {
            List<Zone> largestArea = new List<Zone>();
            List<Zone> remaining = new List<Zone>();
            remaining.AddRange(p.Zones);

            // Go through each zone
            List<Zone> currentArea = new List<Zone>();
            while (remaining.Count > 0)
            {
                // Start a new area
                var toExamine = new Stack<Zone>();
                toExamine.Push(remaining[0]);
                currentArea.Add(remaining[0]);
                remaining.RemoveAt(0);

                // Examine all neighbors
                while (toExamine.Count > 0)
                {
                    var e = toExamine.Pop();
                    foreach (var n in e.Neighbors)
                    {
                        if (n.Owner == p && !currentArea.Contains(n))
                        {
                            toExamine.Push(n);
                            currentArea.Add(n);
                            remaining.Remove(n);
                        }
                    }
                }

                // Nothing more to examine
                if (currentArea.Count > largestArea.Count) largestArea = currentArea;
                currentArea = new List<Zone>();
            }

            // This is your largest contiguous zone
            return largestArea;
        }

        /// <summary>
        /// End the current turn
        /// </summary>
        public void EndTurn()
        {
            // Which player was playing?
            var p = Players[CurrentTurn];

            // Figure out reinforcements for this player
            int reinforcements = GetLargestArea(p).Count;
            ReinforcementRule.Reinforce(this, p, reinforcements);

            // Advance to next non-dead player
            for (int i = 0; i < NumPlayers; i++)
            {
                CurrentTurn++;
                if (CurrentTurn >= Players.Count)
                {
                    CurrentTurn = 0;
                    Round++;
                }
                if (!Players[CurrentTurn].IsDead) break;
            }
        }

        /// <summary>
        /// Basic dice implementation
        /// </summary>
        /// <param name="NumDice"></param>
        /// <param name="DieFaces"></param>
        /// <returns></returns>
        public int Roll(int NumDice, int DieFaces)
        {
            int total = 0;
            for (int i = 0; i < NumDice; i++)
            {
                total += Random.Next(DieFaces) + 1;
            }
            return total;
        }

        /// <summary>
        /// Returns true if this attack is invalid
        /// </summary>
        /// <param name="plan"></param>
        /// <returns></returns>
        public bool AttackIsInvalid(AttackPlan plan)
        {
            return (plan == null
                || plan.Attacker == null
                || plan.Defender == null
                || plan.Attacker.Owner == null
                || plan.Attacker.Strength <= 1
                || !plan.Attacker.Neighbors.Contains(plan.Defender)
                || plan.Attacker.Owner == plan.Defender.Owner);
        }

        /// <summary>
        /// Game continues until only one player is alive
        /// </summary>
        public bool StillPlaying()
        {
            return (from p in Players where p.IsDead == false select 1).Sum() > 1;
        }

        /// <summary>
        /// Try to reinforce zones in this list until full.  Returns number of remaining reinforcements
        /// </summary>
        /// <returns>Number of remaining reinforcements that could not be placed<</returns>
        /// <param name="zones">Zones.</param>
        /// <param name="numReinforcements">Number to try to place.</param>
        public int TryReinforce(List<Zone> zones, int numReinforcements)
        {
            List<Zone> remaining = new List<Zone>(zones);
            while (numReinforcements > 0 && remaining.Count > 0)
            {
                var toReinforce = Random.Next(remaining.Count);
                var z = remaining[toReinforce];
                if (z.Strength < z.MaxStrength)
                {
                    z.Strength++;
                    numReinforcements--;
                }
                else
                {
                    remaining.Remove(z);
                }
            }

            // The number that could not be placed
            return numReinforcements;
        }

        /// <summary>
        /// Fetch all possible attacks for a player
        /// </summary>
        /// <returns>The possible attacks.</returns>
        /// <param name="p">P.</param>
        public List<AttackPlan> GetPossibleAttacks(Player p)
        {
            // Pick a territory that has more than one unit
            var zones = (from z in p.Zones where z.Strength > 1 select z).ToList();
            if (zones.Count == 0) return null;

            // Pick a territory where a neighbor can be attacked
            List<AttackPlan> plans = new List<AttackPlan>();
            foreach (var z in zones)
            {
                foreach (var n in z.Neighbors)
                {
                    if (n.Owner != p)
                    {
                        plans.Add(new AttackPlan()
                        {
                            Attacker = z,
                            Defender = n
                        });
                    }
                }
            }
            return plans;
        }
    }
}
