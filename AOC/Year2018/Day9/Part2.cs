using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC.Year2018.Day9
{
    public class Part2 : BasePart
    {
        protected void RunScenario(string title, string input)
        {
            RunScenario(title, () =>
            {
                var lines = input.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                var parts = lines[0].Numbers();
                var players = parts[0];
                var lastMarble = parts[1];

                var circle = new LinkedList<int>();
                circle.AddFirst(0);
                var scores = new long[players];
                var current = circle.First;
                var currentPlayer = 0;
                for (var marble = 1; marble <= lastMarble; marble++)
                {
                    if (marble % 23 == 0 && marble > 0)
                    {
                        scores[currentPlayer] += marble;
                        var toRemove = MoveBy(circle, current, -7);
                        scores[currentPlayer] += toRemove.Value;
                        current = toRemove.Next;
                        circle.Remove(toRemove);
                    }
                    else
                    {
                        current = MoveBy(circle, current, 1);
                        current = circle.AddAfter(current, marble);
                    }
                    //Console.WriteLine($"Current: {current}, Count: {circle.Count}");
                    //Console.WriteLine(string.Join(" ", circle));
                    currentPlayer++;
                    currentPlayer %= players;

                }

                Console.WriteLine(scores.Max());
            });
        }

        public LinkedListNode<int> MoveBy(LinkedList<int> list, LinkedListNode<int> source, int by)
        {
            while (by > 0)
            {
                if (source.Next == null)
                {
                    source = list.First;
                }
                else
                {
                    source = source.Next;
                }
                by--;
            }
            while (by < 0)
            {
                if (source.Previous == null)
                {
                    source = list.Last;
                }
                else
                {
                    source = source.Previous;
                }
                by++;
            }

            return source;
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
            RunScenario("part1", @"476,71657");
            RunScenario("part2", @"476,7165700");

        }
    }
}
