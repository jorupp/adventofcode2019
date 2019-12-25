using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC.Year2019.Day24
{
    public class Part1 : BasePart
    {
        protected void RunScenario(string title, string input)
        {
            RunScenario(title, () =>
            {
                var lines = input.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

                var state = new Dictionary<(int, int), bool>();

                for (var y = 0; y < lines.Length; y++)
                {
                    for (var x = 0; x < lines[0].Length; x++)
                    {
                        state[(x, y)] = lines[y][x] == '#';
                    }
                }

                var adjacent = new List<(int, int)>()
                {
                    {(-1, 0)},
                    {(1, 0)},
                    {(0, -1)},
                    {(0, 1)},
                };

                Dictionary<(int, int), bool> getNextState(Dictionary<(int, int), bool> state)
                {
                    var newState = new Dictionary<(int, int), bool>();
                    for (var y = 0; y < lines.Length; y++)
                    {
                        for (var x = 0; x < lines[0].Length; x++)
                        {
                            var numAdjacent = adjacent.Select(i => state.TryGetValue((x + i.Item1, y + i.Item2), out var v) ? v : false)
                                .Count(i => i);
                            var cell = (x, y);
                            if (state[cell])
                            {
                                if (numAdjacent == 1)
                                {
                                    newState[cell] = true;
                                }
                                else
                                {
                                    newState[cell] = false;
                                }
                            }
                            else
                            {
                                if (numAdjacent == 1 || numAdjacent == 2)
                                {
                                    newState[cell] = true;
                                }
                                else
                                {
                                    newState[cell] = false;
                                }
                            }
                        }
                    }

                    return newState;
                }

                long getRating(Dictionary<(int, int), bool> state)
                {
                    long rating = 0;
                    var ix = 1;
                    for (var y = 0; y < lines.Length; y++)
                    {
                        for (var x = 0; x < lines[0].Length; x++)
                        {
                            var cell = (x, y);
                            if (state[cell])
                            {
                                rating += ix;
                            }

                            ix *= 2;
                        }
                    }

                    return rating;
                }

                // 8m25s - 43rd
                var ratings = new HashSet<long>();
                while (true)
                {
                    var rating = getRating(state);
                    if (ratings.Contains(rating))
                    {
                        Console.WriteLine(rating);
                        return;
                    }

                    ratings.Add(rating);
                    state = getNextState(state);
                }
            });
        }

        public override void Run()
        {
            RunScenario("initial", @"....#
#..#.
#..##
..#..
#....");
            //return;
            RunScenario("part1", @"#.#..
.###.
...#.
###..
#....");

        }
    }
}
