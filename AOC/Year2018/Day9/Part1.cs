using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC.Year2018.Day9
{
    public class Part1 : BasePart
    {
        protected void RunScenario(string title, string input)
        {
            RunScenario(title, () =>
            {
                var lines = input.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                var parts = lines[0].Numbers();
                var players = parts[0];
                var lastMarble = parts[1];

                var circle = new List<int>() {0};
                var scores = new long[players];
                var current = 0;
                var currentPlayer = 0;
                for (var marble = 1; marble <= lastMarble; marble++)
                {
                    if (marble % 23 == 0 && marble > 0)
                    {
                        scores[currentPlayer] += marble;
                        var toRemove = (current - 7 + circle.Count) % circle.Count;
                        scores[currentPlayer] += circle[toRemove];
                        circle.RemoveAt(toRemove);
                        current = toRemove % circle.Count;
                    }
                    else
                    {
                        current = (current + 1) % circle.Count;
                        current++;
                        circle.Insert(current, marble);
                    }
                    //Console.WriteLine($"Current: {current}, Count: {circle.Count}");
                    //Console.WriteLine(string.Join(" ", circle));
                    currentPlayer++;
                    currentPlayer %= players;

                }

                Console.WriteLine(scores.Max());
            });
        }

        public override void Run()
        {
            RunScenario("initial-0", @"9,25");
            RunScenario("initial-1", @"10,1618");
            RunScenario("initial-2", @"13,7999");
            RunScenario("initial-3", @"17,1104");
            RunScenario("initial-4", @"21,6111");
            RunScenario("initial-5", @"30,5807");
            //return;
            RunScenario("part1-71657", @"476,71657");

            // this algorithm doesn't work well for large numbers...
            RunScenario("part2-200000", @"476,200000");
            RunScenario("part2-400000", @"476,400000");
            RunScenario("part2-500000", @"476,500000");
            RunScenario("part2-600000", @"476,600000");
            RunScenario("part2-716570", @"476,716570");
            RunScenario("part2-7165700", @"476,7165700");

        }
    }
}
