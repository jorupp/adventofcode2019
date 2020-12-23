using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC.Year2020.Day23
{
    public class Part2 : BasePart
    {
        public class Node
        {
            public int Value;
            public Node Previous;
            public Node Next;

            public Node(int value)
            {
                Value = value;
            }
        }


        protected void RunScenario(string title, string input, int padding, int count)
        {
            RunScenario(title, () =>
            {
                var data = input.Select(i => int.Parse(i.ToString())).ToList();
                data = data.Concat(Enumerable.Range(data.Count + 1, padding - data.Count)).ToList();

                var nodes = data.Select(i => new Node(i)).ToList();
                nodes.Zip(nodes.Skip(1), (a, b) =>
                {
                    a.Next = b;
                    b.Previous = a;
                    return a;
                }).ToList();
                var last = nodes.Last();
                nodes[0].Previous = last;
                last.Next = nodes[0];
                var map = nodes.ToDictionary(i => i.Value);

                var current = nodes[0];
                for(var t=0; t<count;t++)
                {
                    // The crab picks up the three cups that are immediately clockwise of the current cup.
                    // They are removed from the circle; cup spacing is adjusted as necessary to maintain the circle.
                    var removed = current.Next;
                    current.Next = removed.Next.Next.Next;
                    removed.Previous = null;
                    removed.Next.Next.Next = null;

                    // The crab selects a destination cup: the cup with a label equal to the current cup's label minus one. 
                    // If this would select one of the cups that was just picked up, the crab will keep subtracting one 
                    // until it finds a cup that wasn't just picked up. If at any point in this process the value goes 
                    // below the lowest value on any cup's label, it wraps around to the highest value on any cup's label instead.
                    var destination = ((current.Value - 2) + data.Count) % data.Count + 1;
                    while (removed.Value == destination || removed.Next.Value == destination || removed.Next.Next.Value == destination)
                    {
                        destination = ((destination - 2) + data.Count) % data.Count + 1;
                    }
                    var destinationNode = map[destination];

                    // The crab places the cups it just picked up so that they are immediately clockwise of the destination cup.
                    // They keep the same order as when they were picked up.
                    removed.Next.Next.Next = destinationNode.Next;
                    destinationNode.Next = removed;

                    // The crab selects a new current cup: the cup which is immediately clockwise of the current cup.
                    current = current.Next;
                }

                var node1 = map[1];

                Console.WriteLine($"After {count} rounds with {padding} numbers");

                long v1 = node1.Next.Value;
                Console.WriteLine($"Followed by {v1}");
                long v2 = node1.Next.Next.Value;
                Console.WriteLine($"Followed by {v2}");

                var answer = v1 * v2;

                Console.WriteLine($"Answer: {answer}");
            });
        }


        public override void Run()
        {
            RunScenario("initial", @"389125467", 9, 100);
            //return;
            RunScenario("part1", @"362981754", 9, 100);

            RunScenario("initial", @"389125467", 1000000, 10000000);
            //return;
            RunScenario("part1", @"362981754", 1000000, 10000000);

        }
    }
}
