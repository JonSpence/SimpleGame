using System;
using System.Linq;
using System.Text;
using GameLib;
using GameLib.Bots;
using GameLib.Rules;

namespace ConsoleGame
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            int Wins = 0;
            for (int i = 0; i < 1000; i++)
            {
                Wins += RunGame();
            }
            Console.WriteLine("Total wins by CautiousBot: " + Wins.ToString());
        }

        public static int RunGame()
        { 
            // Run a quick simulation
            Board b = Board.NewBoard(5, 5, 3);
            b.BattleRule = new RankedDice();
            b.Players[0].Bot = new CautiousBot();
            while (b.StillPlaying())
            {
                //Console.WriteLine(PrintStatistics(b));
                TakeBotTurn(b);
            }
            //Console.WriteLine("Final: ");
            //Console.WriteLine(PrintStatistics(b));
            if (!b.Players[0].IsDead) return 1;
            return 0;
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
