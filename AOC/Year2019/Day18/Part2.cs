using System;
using System.Collections.Generic;
using System.Linq;
using AoC.GraphSolver;

namespace AoC.Year2019.Day18
{
    public class Part2 : BasePart
    {
        protected void RunScenario(string title, string input)
        {
            RunScenario(title, () =>
            {
                var lines = input.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                var map = lines.SelectMany((i, y) => i.Select((ii, x) => new { x, y, v = ii }))
                    .ToDictionary(i => (i.x, i.y), i => i.v);

                var importantPoints = map.Where(i => i.Value != '#' && i.Value != '.')
                    .Select(i => i.Key)
                    .ToList();

                int? getDistance((int, int) p1, (int, int) p2)
                {
                    var start = new SimpleMapNode(p1.Item1, p1.Item2, p2, 0, map);
                    var solution = new RealSolver().Evaluate<SimpleMapNode, (int, int), int>(start);

                    return solution?.CurrentCost;
                }

                var edges = importantPoints.ToDictionary(p1 => p1, p1 =>
                {
                    return importantPoints
                        .Where(p2 => p1 != p2)
                        .Select(p2 => new {p1, p2, d = getDistance(p1, p2)})
                        .Where(i => i.d.HasValue)
                        .ToDictionary(i => i.p2, i => i.d.Value);
                });

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

                var worstCostYet = 0;
                var startP = map.Where(i => i.Value == '@').Select(i => i.Key).ToList();
                var startX = startP.Select(i => i.x).ToArray();
                var startY = startP.Select(i => i.y).ToArray();
                var start = new MapNode(startX, startY, 0, map, new HashSet<char>(), map.Where(i => char.IsLower(i.Value)).ToDictionary(i => i.Value, i => i.Key), edges);
                var solution = new RealSolver().Evaluate<MapNode, string, int>(new [] { start }, null, (n) =>
                {
                    if (worstCostYet < n.CurrentCost)
                    {
                        worstCostYet = n.CurrentCost;
                        Console.WriteLine($"Evaluating node with cost {n.CurrentCost} -> {n.EstimatedCost} ({n.KeysLeft} keys left)");
                    }

                });

                //var solution = new ParallelSolver(8).Evaluate(start, start.Key);

                Console.WriteLine(solution.CurrentCost);
            });
        }

        private class SimpleMapNode : Node<SimpleMapNode, (int, int), int>
        {
            private int _x;
            private int _y;
            private (int, int) _target;
            private int _cost;
            private Dictionary<(int, int), char> _map;

            public SimpleMapNode(int x, int y, (int, int) target, int cost, Dictionary<(int, int), char> map)
            {
                _x = x;
                _y = y;
                _target = target;
                _cost = cost;
                _map = map;
            }

            public override IEnumerable<SimpleMapNode> GetAdjacent()
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

                    if (nextX == _target.Item1 && nextY == _target.Item2)
                    {
                        yield return new SimpleMapNode(nextX, nextY, _target, _cost + 1, _map);

                    }

                    var nextV = _map.TryGetValue((nextX, nextY), out var v) ? v : '#';
                    if (nextV != '.')
                        continue;

                    yield return new SimpleMapNode(nextX, nextY, _target, _cost + 1, _map);
                }
            }

            public override bool IsValid
            {
                get { return true; }
            }

            public override bool IsComplete
            {
                get { return _x == _target.Item1 && _y == _target.Item2; }
            }

            public override int CurrentCost
            {
                get { return _cost; }
            }


            public override int EstimatedCost
            {
                get
                {
                    return CurrentCost + Math.Abs(_x - _target.Item1) + Math.Abs(_y - _target.Item2);
                }
            }

            protected override (int, int) GetKey()
            {
                return (_x, _y);
            }

            public override string Description
            {
                get { return $"{_x},{_y} = {_cost}"; }
            }
        }

        private class MapNode : Node<MapNode, string, int>
        {
            private int[] _x;
            private int[] _y;
            private int _cost;
            private Dictionary<(int, int), char> _map;
            private HashSet<char> _keys;
            private readonly Dictionary<char, (int, int)> _allKeys;
            private readonly Dictionary<(int, int), Dictionary<(int, int), int>> _edges;
            private readonly string _key;
            private readonly int _getOtherKeysCost;

            public MapNode(int[] x, int[] y, int cost, Dictionary<(int, int), char> map, HashSet<char> keys, Dictionary<char, (int, int)> allKeys, Dictionary<(int, int), Dictionary<(int, int), int>> edges)
            {
                _x = x;
                _y = y;
                _cost = cost;
                _map = map;
                _keys = keys;
                _allKeys = allKeys;
                _edges = edges;
                _key = string.Concat(new[]
                {
                    string.Join(",", _x),
                    ",",
                    string.Join(",", _y),
                    ",",
                    string.Join(",", _keys.OrderBy(i => i)),
                });
                _getOtherKeysCost =
                    _allKeys.Where(i => !_keys.Contains(i.Key))
                        .Select(i =>
                        {
                            return _x.Zip(_y,
                                    (x, y) => { return Math.Abs(x - i.Value.Item1) + Math.Abs(y - i.Value.Item2); })
                                .Min();
                        }).Sum();
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
                for (var r = 0; r < _x.Length; r++)
                {
                    var edges = _edges.TryGetValue((_x[r], _y[r]), out var e) ? e : null;
                    if (null == edges)
                    {
                        Console.WriteLine($"Cannot find edges for {_x[r]},{_y[r]}");
                        continue;
                    }

                    foreach(var edge in edges)
                    {
                        var nextX = edge.Key.Item1;
                        var nextY = edge.Key.Item2;
                        var nextV = _map.TryGetValue((nextX, nextY), out var v) ? v : '#';
                        if (nextV == '#')
                            continue;

                        if (char.IsUpper(nextV))
                        {
                            if (!_keys.Contains(char.ToLower(nextV)))
                            {
                                continue;
                            }
                        }

                        var keys = new HashSet<char>(_keys);
                        if (char.IsLower(nextV))
                        {
                            keys.Add(nextV);
                        }

                        var nX = _x.ToArray();
                        var nY = _y.ToArray();
                        nX[r] = nextX;
                        nY[r] = nextY;

                        yield return new MapNode(nX, nY, _cost + edge.Value, _map, keys, _allKeys, _edges);
                    }
                }
            }

            public override bool IsValid
            {
                get { return true; }
            }

            public override bool IsComplete
            {
                get { return _allKeys.Count == _keys.Count; }
            }

            public override int CurrentCost
            {
                get { return _cost; }
            }

            public int KeysLeft
            {
                get
                {
                    return _allKeys.Count - _keys.Count;
                }
            }

            public override int EstimatedCost
            {
                get
                {

                    return CurrentCost + _getOtherKeysCost;
                }
            }

            protected override string GetKey()
            {
                return _key;
            }

            public override string Description
            {
                get { return $"{_x},{_y},{string.Join(",", _keys.OrderBy(i => i))} = {_cost}"; }
            }
        }


        public override void Run()
        {
            RunScenario("initial", @"#######
#a.#Cd#
##@#@##
#######
##@#@##
#cB#Ab#
#######");

            RunScenario("initial", @"###############
#d.ABC.#.....a#
######@#@######
###############
######@#@######
#b.....#.....c#
###############");

            RunScenario("initial", @"#############
#DcBa.#.GhKl#
#.###@#@#I###
#e#d#####j#k#
###C#@#@###J#
#fEbA.#.FgHi#
#############");

            RunScenario("initial", @"#############
#g#f.D#..h#l#
#F###e#E###.#
#dCba@#@BcIJ#
#############
#nK.L@#@G...#
#M###N#H###.#
#o#m..#i#jk.#
#############");

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
#.#.....................#...........#..@#@#.........#.........#...........#.....#
#################################################################################
#.......#.....#.........#...........#..@#@........#.....#...#.......#...........#
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
