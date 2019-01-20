using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameLib;
using GameLib.Bots;
using GameLib.Interfaces;
using GameLib.Rules;

namespace ConsoleGame
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            // What bots should we test?
            List<IBot> list = new List<IBot>();
            list.Add(new CautiousBot());
            list.Add(new BorderShrinkBot());
            list.Add(new RandomBot());
            list.Add(new TurtleBot());
            list.Add(new ThisIsTheBotIWasHiredToCreate());
            list.Add(new ExpandoBot());

            // Test all bots in a FFA battle against each other
            Dictionary<string, int> dict = new Dictionary<string, int>();
            for (int i = 0; i < 1000; i++)
            {
                string winner = RunFFA(list); 
                if (!dict.ContainsKey(winner))
                {
                    dict[winner] = 1;
                }
                else
                {
                    dict[winner] = dict[winner] + 1;
                }
            }

            // Print FFA results
            foreach (var kvp in dict)
            {
                Console.WriteLine($"FFA - {kvp.Key}: {kvp.Value}");
            }

            // Test each on 1000 games against 5 other random bots
            foreach (var bot in list) {
                int Wins = 0;
                for (int i = 0; i < 1000; i++)
                {
                    Wins += RunRandomBotsAgainstTarget(bot);
                }
                Console.WriteLine($"Total wins by {bot.GetType().ToString()}: {Wins}");
            }
        }

        private static string RunFFA(List<IBot> list)
        {
            // Run a quick simulation
            Board b = Board.NewBoard(5, 5, list.Count);
            b.BattleRule = new RankedDice();
            //b.ReinforcementRule = new RandomAnywhere();
            for (int i = 0; i < list.Count; i++)
            {
                b.Players[i].Bot = list[i];
            }
            while (b.StillPlaying())
            {
                TakeBotTurn(b);
            }
            return (from p in b.Players where !p.IsDead select p.Bot.GetType().ToString()).FirstOrDefault();
        }

        public static int RunRandomBotsAgainstTarget(IBot to_test)
        { 
            // Run a quick simulation
            Board b = Board.NewBoard(5, 5, 6);
            b.BattleRule = new RankedDice();
            //b.ReinforcementRule = new RandomAnywhere();
            b.Players[0].Bot = to_test;
            while (b.StillPlaying())
            {
                TakeBotTurn(b);
            }
            if (!b.Players[0].IsDead) return 1;
            return 0;
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
