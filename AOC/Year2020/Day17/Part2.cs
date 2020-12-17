using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC.Year2020.Day17
{
    public class Part2 : BasePart
    {
        private const char active = '#';
        private const char inactive = '.';

        public Part2()
        {
            directions = (from x in Enumerable.Range(-1, 3)
                          from y in Enumerable.Range(-1, 3)
                          from z in Enumerable.Range(-1, 3)
                          from w in Enumerable.Range(-1, 3)
                          where x != 0 || y != 0 || z != 0 || w != 0
                          select (x, y, z, w)).ToArray();
        }


        protected void RunScenario(string title, string input)
        {

            RunScenario(title, () =>
            {
                var lines = input.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                var state = new Dictionary<(int, int, int, int), bool>();
                for (var y = 0; y < lines.Length; y++)
                {
                    for (var x = 0; x < lines[y].Length; x++)
                    {
                        state[(x, y, 0, 0)] = lines[y][x] == active;
                    }
                }

                //Dump(state);

                for (var i = 0; i < 6; i++)
                {
                    state = Simulate(state);
                    //Console.WriteLine(i);
                    //Dump(state);
                }


                Console.WriteLine(state.Values.Count(i => i));
            });
        }

        private readonly (int, int, int, int)[] directions;

        private void Dump(Dictionary<(int, int, int, int), bool> input)
        {
            var mx = input.Keys.Select(i => i.Item1).Min();
            var xx = input.Keys.Select(i => i.Item1).Max();
            var my = input.Keys.Select(i => i.Item2).Min();
            var xy = input.Keys.Select(i => i.Item2).Max();
            var mz = input.Keys.Select(i => i.Item3).Min();
            var xz = input.Keys.Select(i => i.Item3).Max();
            var mw = input.Keys.Select(i => i.Item4).Min();
            var xw = input.Keys.Select(i => i.Item4).Max();

            for (var w = mw; w <= xw; w++)
            {
                for (var z = mz; z <= xz; z++)
                {
                    for (var y = my; y <= xy; y++)
                    {
                        for (var x = mx; x <= xx; x++)
                        {
                            Console.Write((input.TryGetValue((x, y, z, w), out var v) && v) ? active : inactive);
                        }
                        Console.WriteLine();
                    }

                    Console.WriteLine();
                }
                Console.WriteLine();
            }
        }

        private Dictionary<(int, int, int, int), bool> Simulate(Dictionary<(int, int, int, int), bool> input)
        {
            var output = new Dictionary<(int, int, int, int), bool>();

            var mx = input.Keys.Select(i => i.Item1).Min() - 1;
            var xx = input.Keys.Select(i => i.Item1).Max() + 1;
            var my = input.Keys.Select(i => i.Item2).Min() - 1;
            var xy = input.Keys.Select(i => i.Item2).Max() + 1;
            var mz = input.Keys.Select(i => i.Item3).Min() - 1;
            var xz = input.Keys.Select(i => i.Item3).Max() + 1;
            var mw = input.Keys.Select(i => i.Item4).Min() - 1;
            var xw = input.Keys.Select(i => i.Item4).Max() + 1;

            for (var x = mx; x <= xx; x++)
            {
                for (var y = my; y <= xy; y++)
                {
                    for (var z = mz; z <= xz; z++)
                    {
                        for (var w = mw; w <= xw; w++)
                        {
                            var state = input.TryGetValue((x, y, z, w), out var v1) && v1;
                            var count = 0;
                            foreach (var dir in directions)
                            {
                                var x1 = x + dir.Item1;
                                var y1 = y + dir.Item2;
                                var z1 = z + dir.Item3;
                                var w1 = w + dir.Item4;
                                if (input.TryGetValue((x1, y1, z1, w1), out var v) && v)
                                {
                                    count++;
                                }
                            }

                            if (state)
                            {
                                if (count == 2 || count == 3)
                                {
                                    output[(x, y, z, w)] = true;
                                }
                            }
                            else
                            {
                                if (count == 3)
                                {
                                    output[(x, y, z, w)] = true;
                                }
                            }
                        }
                    }
                }
            }

            return output;
        }

        public override void Run()
        {
            RunScenario("initial", @".#.
..#
###");
            //return;
            RunScenario("part1", @".##...#.
.#.###..
..##.#.#
##...#.#
#..#...#
#..###..
.##.####
..#####.");

        }
    }
}
