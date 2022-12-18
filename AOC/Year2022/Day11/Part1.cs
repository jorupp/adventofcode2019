using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AoC;

namespace AOC.Year2022.Day11
{
    public class Part1 : BasePart
    {
        private const bool debug = false;
        // 11:30:15 -> 12:17:25
        protected void RunScenario(string title, Monkey[] monkeys)
        {
            RunScenario(title, () =>
            {
                foreach (var m in monkeys)
                {
                    Console.WriteLine(string.Join(", ", m.items) + " -> " + m.count);
                }
                for (var r = 0; r < 20; r++)
                {
                    foreach(var m in monkeys)
                    {
                        m.TakeTurn(monkeys);
                    }
                    Console.WriteLine($"After round {r + 1}");
                    foreach (var m in monkeys)
                    {
                        Console.WriteLine(string.Join(", ", m.items) + " -> " + m.count);
                    }
                }

                var c = monkeys.Select(i => i.count).OrderByDescending(i => i).ToList();

                Console.WriteLine(c[0] * c[1]);

            });
        }

        public override void Run()
        {
            RunScenario("initial", new[]
            {
                new Monkey(new long[] { 79, 98 }, o => o * 19, i => i %23 == 0  ? 2 : 3),
                new Monkey(new long[] { 54, 65, 75, 74 }, o => o +6, i => i % 19 == 0  ? 2: 0),
                new Monkey(new long[] { 79, 60, 97 }, o => o * o, i => i % 13 == 0  ? 1 : 3),
                new Monkey(new long[] { 74 }, o => o + 3, i => i % 17 == 0  ? 0 : 1),
            });
            //return;
            //RunScenario("confirm loop of 3/2", new[]
            //{
            //    new Monkey(new long[] { 3 }, old => old * 5, i => i % 11 == 0 ? 3 : 4),
            //    new Monkey(new long[] {  }, old => old * old, i => i % 2 == 0 ? 6 : 7),
            //    new Monkey(new long[] {  }, old => old * 7, i => i % 5 == 0 ? 1 : 5),
            //    new Monkey(new long[] {  }, old => old + 1, i => i % 17 == 0 ? 2 : 5),
            //    new Monkey(new long[] { }, old => old + 3, i => i % 19 == 0 ? 2 : 3),
            //    new Monkey(new long[] {  }, old => old + 5, i => i % 7 == 0 ? 1 : 6),
            //    new Monkey(new long[] {  }, old => old + 8, i => i % 3 == 0 ? 0 : 7),
            //    new Monkey(new long[] {  }, old => old + 2, i => i % 13 == 0 ? 4 : 0),
            //});
            //return;
            RunScenario("part1", new[]
            {
                new Monkey(new long[] { 92, 73, 86, 83, 65, 51, 55, 93 }, old => old * 5, i => i % 11 == 0 ? 3 : 4),
                new Monkey(new long[] { 99, 67, 62, 61, 59, 98 }, old => old * old, i => i % 2 == 0 ? 6 : 7),
                new Monkey(new long[] { 81, 89, 56, 61, 99 }, old => old * 7, i => i % 5 == 0 ? 1 : 5),
                new Monkey(new long[] { 97, 74, 68 }, old => old + 1, i => i % 17 == 0 ? 2 : 5),
                new Monkey(new long[] { 78, 73 }, old => old + 3, i => i % 19 == 0 ? 2 : 3),
                new Monkey(new long[] { 50 }, old => old + 5, i => i % 7 == 0 ? 1 : 6),
                new Monkey(new long[] { 95, 88, 53, 75 }, old => old + 8, i => i % 3 == 0 ? 0 : 7),
                new Monkey(new long[] { 50, 77, 98, 85, 94, 56, 89 }, old => old + 2, i => i % 13 == 0 ? 4 : 0),
            });

        }

        public class Monkey
        {
            public readonly Queue<long> items;
            private readonly Func<long, long> op;
            private readonly Func<long, long> test;
            public long count = 0;

            public Monkey(long[] startingItems, Func<long, long> op, Func<long, long> test)
            {
                this.items = new Queue<long>(startingItems);
                this.op = op;
                this.test = test;
            }

            public void TakeTurn(Monkey[] monkeys)
            {
                while(items.Any())
                {
                    var item = items.Dequeue();
                    var sb = new StringBuilder();
                    var show = false;
                    if (item < 0) show = true;
                    sb.AppendLine($"Inspecting {item}");
                    item = op(item);
                    if (item < 0) show = true;
                    sb.AppendLine($"Item now {item}");
                    item = item / 3; // rounds down
                    if (item < 0) show = true;
                    sb.AppendLine($"Bored: item now {item}");

                    var target = test(item);
                    sb.AppendLine($"throwing to {target}");
                    monkeys[target].items.Enqueue(item);
                    count++;

                    if (show)
                    {
                        Console.WriteLine(sb.ToString());
                    }

                }
            }

        }

    }
}
