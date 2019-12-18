﻿using System;
using System.Collections.Generic;
using System.Linq;
using AoC.GraphSolver;

namespace AoC.Year2019.Day18
{
    public class Part1 : BasePart
    {
        protected void RunScenario(string title, string input)
        {
            RunScenario(title, () =>
            {
                var lines = input.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                var map = lines.SelectMany((i, y) => i.Select((ii, x) => new {x, y, v = ii}))
                    .ToDictionary(i => (i.x, i.y), i => i.v);

                void Dump()
                {
                    var nmx = map.Keys.Min(i => i.Item1);
                    var nmy = map.Keys.Min(i => i.Item2);
                    var mx = map.Keys.Max(i => i.Item1);
                    var my = map.Keys.Max(i => i.Item2);

                    for (var y = nmx; y <= my; y++)
                    {
                        for (var x = nmy; x <= mx; x++)
                        {
                            var value = (map.TryGetValue((x, y), out var v) ? v : ' ');
                            Console.Write(value);
                        }
                        Console.WriteLine();
                    }
                    Console.WriteLine();
                }

                //Dump();

                var startP = map.Single(i => i.Value == '@').Key;
                var start = new MapNode(startP.x, startP.y, 0, map, new HashSet<char>(), map.Values.Count(i => char.IsLower(i)));
                var solution = new RealSolver().Evaluate<MapNode, (int, int, string), int>(start);

                Console.WriteLine(solution.CurrentCost);
            });
        }


        private class MapNode : Node<MapNode, (int, int, string), int>
        {
            private int _x;
            private int _y;
            private int _cost;
            private Dictionary<(int, int), char> _map;
            private HashSet<char> _keys;
            private readonly int _numKeys;

            public MapNode(int x, int y, int cost, Dictionary<(int, int), char> map, HashSet<char> keys, int numKeys)
            {
                _x = x;
                _y = y;
                _cost = cost;
                _map = map;
                _keys = keys;
                _numKeys = numKeys;
            }

            public override IEnumerable<MapNode> GetAdjacent()
            {
                var dirs = new Dictionary<int, (int, int)>()
                {
                    {1, (0, -1)},
                    {2, (0, 1)},
                    {3, (-1, 0)},
                    {4, (1, 0)},
                };
                foreach (var dir in dirs.Values)
                {
                    var nextX = _x + dir.Item1;
                    var nextY = _y + dir.Item2;
                    var nextV = _map.TryGetValue((nextX, nextY), out var v) ? v : '#';
                    if (nextV == '#')
                        continue;

                    if (char.IsUpper(nextV))
                    {
                        if (!_keys.Contains(char.ToLower(nextV)))
                        {
                            //Console.WriteLine($"Don't have key for {nextX},{nextY}");
                            continue;
                        }
                    }

                    var keys = new HashSet<char>(_keys);
                    if (char.IsLower(nextV))
                    {
                        //Console.WriteLine($"Adding key for {nextX},{nextY}");
                        keys.Add(nextV);
                    }

                    //Console.WriteLine($"Looking for {nextX},{nextY}");
                    yield return new MapNode(nextX, nextY, _cost + 1, _map, keys, _numKeys);
                }
            }

            public override bool IsValid
            {
                get { return true; }
            }

            public override bool IsComplete
            {
                get { return _numKeys == _keys.Count; }
            }

            public override int CurrentCost
            {
                get { return _cost; }
            }

            public override int EstimatedCost
            {
                get { return CurrentCost + (_numKeys - _keys.Count); }
            }

            protected override (int, int, string) GetKey()
            {
                return (_x, _y, string.Join(",", _keys.OrderBy(i => i)));
            }

            public override string Description
            {
                get { return $"{_x},{_y},{string.Join(",", _keys.OrderBy(i => i))} = {_cost}"; }
            }
        }


        public override void Run()
        {
            RunScenario("initial", @"#########
#b.A.@.a#
#########");
            RunScenario("initial", @"########################
#f.D.E.e.C.b.A.@.a.B.c.#
######################.#
#d.....................#
########################");
            RunScenario("initial", @"########################
#...............b.C.D.f#
#.######################
#.....@.a.B.c.d.A.e.F.g#
########################");
            RunScenario("initial", @"#################
#i.G..c...e..H.p#
########.########
#j.A..b...f..D.o#
########@########
#k.E..a...g..B.n#
########.########
#l.F..d...h..C.m#
#################");
            RunScenario("initial", @"########################
#@..............ac.GI.b#
###d#e#f################
###A#B#C################
###g#h#i################
########################");
            //return;
            RunScenario("part1", @"#################################################################################
#z..........................#....k#.....#.......#.....#.............#...#.......#
#.###############.#########.#.###.#.###.#Q#######.#.###.#####.#####.#.#.#.#.###.#
#...#...#.....#...#.....#...#.#.#.#...#.#.#.......#..u#..i#.#...#.#.#.#...#...#j#
###W#.#.#####B#.#####.#.#.###.#.#.###.#.#.#.#########.###.#.###.#.#.#.#######.#.#
#.#...#.#...#.#.....#.#.......#.#...#.#.#...#...#.....#.#...#.#.#...#.#.....#.#.#
#.#####.#.#.#.#####.###########.###.#.#.#.###.#.#.###.#.###.#.#.#.#####.###.#.#.#
#...#...#.#.......#.......#.....#...#.#.#...#.#.#.#...#...#.#.#.#.#...#...#...#.#
#.#.#.###.#######.#######.###.###.#####.###.#.#.#.#.###.###.#.#.#.#.#.#.#.#####.#
#.#.#.....#.....#.#.......#...#...#.....#.#.#.#.#.#.....#...#.#.#...#.#.#...#...#
#.#.#######.###.#.#.#######.###.###.###.#.#.#.###.#######N###.#.#####.###.#.#.###
#.#...#...#...#...#.#...#.....#.#.....#.#...#...#.......#.#...#.....#...#.#.#.#.#
###.#.#.#.###.#####.#.#.#.###.#.#.###.#.#.#####.#######.#.###.#####.###.###.#.#.#
#...#.#.#...#.#...#.#.#...#...#.#.#...#.#.#.O...#.....#...#.....#...#.......#...#
#.###.#.#.###.###.#.###.###.###.#.#.###.#.#.#####.###.#.###.###.#.#############.#
#.#...#.#.........#...#...#.....#.#.#.#.#...#.....#...#.#...#.#.#.....#.........#
#.#####.#############.###.#####.###.#.#.###.#.#####.###.#.###.#.#####.#.#########
#.#...#.#.#.........#.#...#...#.....#...#...#.#...#.#.....#.........#.#.#.....#.#
#.#.#.#.#.#.#######.#.#####.#.###########.###.#.###.#################.#.#.###.#.#
#.#.#.#...#.#.....#...#...#.#.#.........#...#.#...#.......#.....#....w#.#.#.#.#.#
#.#.#.###.#.###.#.#.###.#.#.#.#.#######.###.#.#.#.#######.#.###.#.#####.#.#.#.#.#
#...#.#...#...#.#.#.#...#.#.#...#.#...#.#...#.#.#...#.......#.#...#...#...#.#.#.#
#.###.#.#####.#.###.#####.#.#####.#.#.#.#.###.###.#.#.#####.#.#####.#.#####.#.#.#
#...#.#.#...#.#.........#.#.#...#...#...#...#...#.#...#...#.#...#...#...#...#.#.#
#####.#.#.###.#.#######.#.#.###.#.#########.###.#######.#.#####.###.#####.#.#.#.#
#.....#.#...#.#.#...#...#...#...#.#.....#.#...#.#...#...#...#...#...#.....#...#.#
#.#.###.###.#.###.#.#.#######.#.#.#.###.#.###.#.#.#.#.#####.#.###.###M#########.#
#.#.#...#...#.....#.#...#.....#.#.#.#.#.#.#...#...#.#.#.....#.#.#...#.........#.#
#.###.###.#.#######.#####.#.#####.#.#.#.#.#.#######.#.#.###.#.#.#.#.#########.#.#
#.....#...#.#.....#.......#...#...#.#...#.#.#.....#.#.#.#...#.#...#...........#.#
#.#####.#.###.###.###########.#.###.#####.#.###.###.#.#.#####.#.#####.#######.#.#
#.#.....#.#...#.......#.....#...#...#...#.#...#...#.#.#.....#.#.#...#.#.....#...#
#.###.#.###.###.#####.###.#######.#.#.#.#.###.###.#.#.#####.#.###.#.###.###.#####
#...#.#.....#.#.....#...#.........#.#.#.#...#...#.#...#.....#.....#...#...#.....#
###.#######.#.#####.###.###.#########.#.###.###.#.#####.#############.###.#####.#
#.#...#...#...#...#...#...#.....#.....#.#.....#.......#.#...#.......#.....#.D.#.#
#.###.#.#.###.###.###.###.###.#.#.#####.#.###.#####.###.#.#.#.###.#.#########.#.#
#...#...#...#.......#...#.E.#.#...#...#.#...#.#.....#...#.#...#...#.#.........#c#
#.#.#######.###########.###.#######.#.#.#.#.###.#####.###.#####.#####.#####.###.#
#.#.....................#...........#.....#.........#.........#...........#.....#
#######################################.@.#######################################
#.......#.....#.........#...........#.............#.....#...#.......#...........#
#.#####.#.###.#.#####.###.#####.###.###.#.###.###.###.###.#.#.#.###.#.#####.###.#
#.#...#.#.#...#.T...#.....#...#.#.....#.#.#...#.#.....#...#.#.#.#...#....a#...#.#
#.#.#.###.#.#######.#####.#.#.#.#####.#.#.#.###.#####.#.###.#.#.#########.###.#.#
#...#.....#.........#...#...#.#.....#...#.#.#........e#...#...#.S.#.......#...#.#
#####################.#.###########.###.#.#.#.###########.#######.#.#######.###.#
#d..#...#.........C...#.#.......#...#...#.#.#.#.........#.#.#.....#.#...#...#..y#
#.#.#.#.#.#########.###.#.#####.#.###.###.#.###.#######.#.#.#.#####.###.#.###.###
#.#.#.#.......#...#.#...#.#.....#.#...#.#.#...#...#...#...#.#.....#.#...#.#.....#
#.#.#.#########.#.###.###.###.###.#.###.#.###.###.#.#######.#####.#.#.###.#####.#
#.#...#.#.......#.#.G.#.....#.....#...#.#.#...#...#.....#.....#.#...#...#.....#.#
#.#####.#.#######.#.###.###.#########.#.#.#.###.#####.#.#.#.#.#.#######.#####.#.#
#.#.....#.#.........#...#.#.....#...#.#.#.#.#...#...#.#.#.#.#.#.......#.....#.#.#
#Y###.###F#############.#.#####.###.#.#.#.#.#.###.#.#.#.###.#.#####.#.#####.#.#.#
#...#.#...#.....P.....#.......#...#.....#.#.#.....#...#.....#.#.....#.#.....#.#.#
###.#.#.#.#.#########.#######.###.#####.###.#################.#.#####.#.#####.#.#
#...#.#.#.#r#.........#.........#.....#.#...#...#o..........#...#.......#.....#.#
#.###.#.###.#######.###############.#.###.###.#.#.#.#####.###.###########.#####.#
#.#...#...#.......#.....#...#.....#.#...#.....#.#.#.....#.#...#........p#.#...#.#
#.#.#.###.#######.#####.#.#.#.###.#####.#.#####.#.#####.###.###.#######.#.#.###.#
#...#.A.#...X...#.#...#.#.#...#.#.#...#.#..t#.#...#...#...#.#...#...#...#.#.....#
#######.#######.#.#R###.#.#####.#.#.#.#.###.#.#####.#####.#.#####.#.#.###.#######
#.#.....#...#...#...#...#...#.#...#.#...#.#.........#q....#.#...#.#...#.........#
#.#.#####.###.#######V###.#.#.#.###.#####.#########.#.#####.#.#.#.#########.###.#
#.#.#.....#...#.....#...#.#.#...#.......#...#.......#.....#.#.#.#v#.......#...#.#
#.#.#.#.###.#.#.#.#.###.#.#.#.#####.###.#.#.#.###########.#.###.#.###.###.###.#.#
#.#.#.#.#...#.#.#.#.#...#.#.#.#...#...#.#.#.#.......#...#.#b..#.#.#...#.#...#.#.#
#.#.#.#.#.###.#.#.###.#####.#.#.#.#####.###.#.#######.#.#.###.#.#.#.###.###.###.#
#...#.#m#...#.#l#.....#.....#.I.#.#...#.#...#.#.......#.....#...#.#.#g....#.#...#
#.#####.###.#.#.#########.#######.#.#.#.#.###.#.#############.###.#.#####.#.#.#.#
#.#.......#.#.#...#.....#.#.#...#...#.#.#...#.#.....#.........#...#....f#.....#.#
#.#.#####.#.#####.#.#.#.#.#.#.#.#####.#.#.#.#.#####.#.#########L#######.#########
#.#...#.#.#.....H.#.#.#...#...#.#...#.#.#.#.....#...#...#.......#.....#.........#
#.###.#.#.#########.#.#####.###.#.#.#.#.#.#######.#####.#.#######.###.#.#######.#
#.....#.#.#...#.....#.Z...#...#.#.#...#.#.#...#...#.....#...#.#.....#.#.......#.#
#######.#.#.#.#.#####.###.###.#.#.#####.#.#.#.#.#U#.#######.#.#.###.#.#########.#
#..s......#.#.#.#...#...#.#...#.#.......#...#.#.#.#...#.....#.....#.#.....#.....#
#.#########.#.###.#.#####.#.###.#######.#####.#.#####.#.###########.#####.#.###.#
#.......K...#...J.#.......#...#........n#.....#.......#............h#......x#...#
#################################################################################");

        }
    }
}
