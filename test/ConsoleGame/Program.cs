using System;
using System.Linq;
using System.Text;
using GameLib;

namespace ConsoleGame
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            // Run a quick simulation
            Board b = Board.NewBoard(10, 10, 6);
            while (b.StillPlaying())
            {
                Console.WriteLine(PrintStatistics(b));
                TakeBotTurn(b);
            }
            Console.WriteLine("Final: ");
            Console.WriteLine(PrintStatistics(b));
        }

        private static string PrintStatistics(Board b)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Round ");
            sb.Append(b.Round);
            sb.Append(": ");
            foreach (var p in b.Players)
            {
                sb.Append(p.Color.Name);
                sb.Append("-");
                sb.Append(((from z in p.Zones select z.Strength).Sum()).ToString());
                sb.Append("   ");
            }
            return sb.ToString();
        }

        private static void TakeBotTurn(Board b)
        {
            int thisTurn = b.Round;

            // If the current player is a bot, do another attack
            while (b.Round == thisTurn)
            {
                if (!b.CurrentPlayer.IsHuman)
                {
                    var plan = b.CurrentPlayer.Bot.PickNextAttack(b, b.CurrentPlayer);
                    if (plan == null)
                    {
                        b.EndTurn();
                    }
                    else
                    {
                        var result = b.BattleRule.Attack(b, b.Players[b.CurrentTurn], plan);
                        result.UpdateBoardTask.RunSynchronously();
                    }
                }
            }
        }
    }
}
