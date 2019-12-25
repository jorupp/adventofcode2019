using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC.Year2019.Day24
{
    public class Part2 : BasePart
    {
        protected void RunScenario(string title, string input, int cycles)
        {
            RunScenario(title, () =>
            {
                var lines = input.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

                var state = new Dictionary<(int, int, int), bool>();

                for (var y = 0; y < lines.Length; y++)
                {
                    for (var x = 0; x < lines[0].Length; x++)
                    {
                        state[(x, y, 0)] = lines[y][x] == '#';
                    }
                }

                var adjacent = new List<(int, int, int)>()
                {
                    {(-1, 0, 0)},
                    {(1, 0, 0)},
                    {(0, -1, 0)},
                    {(0, 1, 0)},
                };
                var extraMaps = new Dictionary<(int, int), List<(int, int, int)>>()
                {
                    { (2, 1), new List<(int, int, int)>
                        {
                            (0, 0, 1),
                            (1, 0, 1),
                            (2, 0, 1),
                            (3, 0, 1),
                            (4, 0, 1),
                        }
                    },
                    { (2, 3), new List<(int, int, int)>
                        {
                            (0, 4, 1),
                            (1, 4, 1),
                            (2, 4, 1),
                            (3, 4, 1),
                            (4, 4, 1),
                        }
                    },
                    { (1,2), new List<(int, int, int)>
                        {
                            (0, 0, 1),
                            (0, 1, 1),
                            (0, 2, 1),
                            (0, 3, 1),
                            (0, 4, 1),
                        }
                    },
                    { (3,2), new List<(int, int, int)>
                        {
                            (4, 0, 1),
                            (4, 1, 1),
                            (4, 2, 1),
                            (4, 3, 1),
                            (4, 4, 1),
                        }
                    },

                    { (0,0), new List<(int, int, int)>
                        {
                            (1, 2, -1),
                            (2, 1, -1),
                        }
                    },
                    { (1,0), new List<(int, int, int)>
                        {
                            (2, 1, -1),
                        }
                    },
                    { (2,0), new List<(int, int, int)>
                        {
                            (2, 1, -1),
                        }
                    },
                    { (3,0), new List<(int, int, int)>
                        {
                            (2, 1, -1),
                        }
                    },
                    { (4,0), new List<(int, int, int)>
                        {
                            (2, 1, -1),
                            (3, 2, -1),
                        }
                    },
                    { (4,1), new List<(int, int, int)>
                        {
                            (3, 2, -1),
                        }
                    },
                    { (4,2), new List<(int, int, int)>
                        {
                            (3, 2, -1),
                        }
                    },
                    { (4,3), new List<(int, int, int)>
                        {
                            (3, 2, -1),
                        }
                    },
                    { (4,4), new List<(int, int, int)>
                        {
                            (3, 2, -1),
                            (2, 3, -1),
                        }
                    },
                    { (3,4), new List<(int, int, int)>
                        {
                            (2, 3, -1),
                        }
                    },
                    { (2,4), new List<(int, int, int)>
                        {
                            (2, 3, -1),
                        }
                    },
                    { (1,4), new List<(int, int, int)>
                        {
                            (2, 3, -1),
                        }
                    },
                    { (0,4), new List<(int, int, int)>
                        {
                            (2, 3, -1),
                            (1, 2, -1),
                        }
                    },
                    { (0,3), new List<(int, int, int)>
                        {
                            (1, 2, -1),
                        }
                    },
                    { (0,2), new List<(int, int, int)>
                        {
                            (1, 2, -1),
                        }
                    },
                    { (0,1), new List<(int, int, int)>
                        {
                            (1, 2, -1),
                        }
                    },
                };

                Dictionary<(int, int, int), bool> getNextState(Dictionary<(int, int, int), bool> state)
                {
                    var newState = new Dictionary<(int, int, int), bool>();
                    var levels = state.Where(i => i.Value).Select(i => i.Key.Item3).Distinct().ToList();
                    var mnLevel = levels.Min() - 1;
                    var mxLevel = levels.Max() + 1;
                    for (var l = mnLevel; l <= mxLevel; l++)
                    {
                        for (var y = 0; y < lines.Length; y++)
                        {
                            for (var x = 0; x < lines[0].Length; x++)
                            {
                                var numAdjacent = adjacent
                                    .Select(i => (x + i.Item1, y + i.Item2, l))
                                    .Where(i => i.Item1 != 2 || i.Item2 != 2)
                                    .Select(i => state.TryGetValue(i, out var v) ? v : false)
                                    .Count(i => i);

                                if (extraMaps.TryGetValue((x, y), out var extra))
                                {
                                    var numAdjacentOtherLevels = 
                                        extra.Select(i => (i.Item1, i.Item2, l + i.Item3))
                                            .Select(i => state.TryGetValue(i, out var v) ? v : false)
                                            .Count(i => i);
                                    numAdjacent += numAdjacentOtherLevels;
                                }

                                var cell = (x, y, l);
                                if (x == 2 && y == 2)
                                {
                                    newState[cell] = false;
                                } else
                                if (state.TryGetValue(cell, out var c) ? c : false)
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
                    }

                    return newState;
                }

                void Dump(Dictionary<(int, int, int), bool> state)
                {
                    var levels = state.Where(i => i.Value).Select(i => i.Key.Item3).Distinct().ToList();
                    var mnLevel = levels.Min();
                    var mxLevel = levels.Max();
                    for (var l = mnLevel; l <= mxLevel; l++)
                    {
                        Console.WriteLine($"Depth {l}");
                        for (var y = 0; y < lines.Length; y++)
                        {
                            for (var x = 0; x < lines[0].Length; x++)
                            {
                                var cell = (x, y, l);
                                if (state.TryGetValue(cell, out var c) ? c : false)
                                {
                                    Console.Write("#");
                                }
                                else
                                {
                                    Console.Write(".");
                                }
                            }

                            Console.WriteLine();
                        }

                        Console.WriteLine();
                    }
                }

                long getTotal(Dictionary<(int, int, int), bool> state)
                {
                    return state.Count(i => i.Value);
                }

                // 33m20s - 53rd
                for (var i=0; i< cycles; i++)
                {
                    Console.WriteLine($"Calculating cycle {i} - have {state.Where(ii => ii.Value).Select(ii => ii.Key.Item3).Distinct().Count()} levels with something in it");
                    state = getNextState(state);
                    //Dump(state);
                }

                Console.WriteLine(getTotal(state));
            });
        }

        public override void Run()
        {
            RunScenario("initial", @"....#
#..#.
#..##
..#..
#....", 10);
            //return;
            //return;
            RunScenario("part1", @"#.#..
.###.
...#.
###..
#....", 200);

        }
    }
}
