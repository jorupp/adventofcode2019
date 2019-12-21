using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection.Metadata;
using AoC.GraphSolver;

namespace AoC.Year2019.Day20
{
    public class Part2 : BasePart
    {
        protected void RunScenario(string title, string input)
        {
            RunScenario(title, () =>
            {
                var lines = input.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

                var portals = new Dictionary<string, (int, int)>();
                var map = new Dictionary<(int, int), string>();

                for (var y = 0; y < lines.Length; y++)
                {
                    for (var x = 0; x < lines[y].Length; x++)
                    {
                        var c = lines[y][x];
                        if (c == ' ')
                        {
                            c = '#';
                        }
                        map[(x, y)] = c.ToString();
                    }
                }


                for (var y = 0; y < lines.Length; y++)
                {
                    for (var x = 0; x < lines[y].Length; x++)
                    {
                        var v = map.TryGetValue((x, y), out var v1) ? v1 : "#";
                        //Console.WriteLine($"checking {x},{y}");
                        if (v.Length == 1 && char.IsLetter(v[0]))
                        {
                            if (y == 0)
                            {
                                map[(x, y + 1)] = map[(x, y)] + map[(x, y + 1)] + "O";
                                map.Remove((x, y));
                            }
                            else if (y == lines.Length - 1)
                            {
                                map[(x, y - 1)] = map[(x, y - 1)] + map[(x, y)] + "O";
                                map.Remove((x, y));
                            }
                            else if (x == 0)
                            {
                                map[(x + 1, y)] = map[(x, y)] + map[(x + 1, y)] + "O";
                                map.Remove((x, y));
                            }
                            else if (x == lines[0].Length - 1)
                            {
                                map[(x - 1, y)] = map[(x - 1, y)] + map[(x, y)] + "O";
                                map.Remove((x, y));
                            }
                            else if (char.IsLetter((map.TryGetValue((x, y + 1), out var v2) ? v2 : "#")[0]))
                            {
                                if (map[(x, y - 1)] == ".")
                                {
                                    map[(x, y)] = map[(x, y)] + map[(x, y + 1)] + ((y + 1 == lines.Length - 1) ? "O" : "I");
                                    map.Remove((x, y + 1));
                                }
                                else
                                {
                                    map[(x, y + 1)] = map[(x, y)] + map[(x, y + 1)] + "I";
                                    map.Remove((x, y));
                                }
                            }
                            else if (char.IsLetter((map.TryGetValue((x + 1, y), out var v3) ? v3 : "#")[0]))
                            {
                                if (map[(x - 1, y)] == ".")
                                {
                                    map[(x, y)] = map[(x, y)] + map[(x + 1, y)] + ((x + 1 == lines[0].Length - 1) ? "O" : "I");
                                    map.Remove((x + 1, y));
                                }
                                else
                                {
                                    map[(x + 1, y)] = map[(x, y)] + map[(x + 1, y)] + "I";
                                    map.Remove((x, y));
                                }
                            }
                        }
                    }
                }

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
                            var value = (map.TryGetValue((x, y), out var v) ? v : "#");
                            if (value.Length == 2)
                            {
                                Console.Write(" ");
                            }
                            if (value.Length == 1)
                            {
                                Console.Write(value);
                                Console.Write(value);
                            }
                            Console.Write(value);
                        }
                        Console.WriteLine();
                    }
                    Console.WriteLine();
                }

                //Dump();

                var importantPoints = map.Where(i => i.Value.Length > 1)
                    .Select(i => i.Key)
                    .ToList();

                int? getDistance((int, int) p1, (int, int) p2)
                {
                    var start = new SimpleMapNode(p1.Item1, p1.Item2, p2, 0, map);
                    var solution = new RealSolver().Evaluate<SimpleMapNode, (int, int), int>(start);

                    return solution?.CurrentCost;
                }

                var edges = importantPoints.ToDictionary(p1 => GetWalkableAdjacent(map, p1), p1 =>
                {
                    var p1Adj = GetWalkableAdjacent(map, p1);
                    var moves = importantPoints
                        .Where(p2 => p1 != p2)
                        .Select(p2 => new { p2, d = getDistance(p1Adj, GetWalkableAdjacent(map, p2)) })
                        .Where(i => i.d.HasValue)
                        .ToDictionary(i => GetWalkableAdjacent(map, i.p2), i => (i.d.Value, 0, $"{map[p1]}->{map[i.p2]}"));

                    var portal = map[p1];
                    if (portal != "AAO" && portal != "ZZO")
                    {
                        //Console.WriteLine($"Finding match for {portal}");
                        var otherPortalPoint = map.Single(i => i.Value.Length == 3 && i.Value.Substring(0, 2) == portal.Substring(0, 2) && i.Key != p1);

                        var direction = portal.EndsWith("O") ? -1 : 1;

                        moves[GetWalkableAdjacent(map, otherPortalPoint.Key)] = (1, direction, $"{portal}->{otherPortalPoint.Value}");
                    }

                    return moves;
                });

                var missing = edges.Values.SelectMany(i => i.Keys).Distinct().Except(edges.Keys).ToList();
                if (missing.Count > 0)
                {

                }

                var startP = GetWalkableAdjacent(map, map.Single(i => i.Value == "AAO").Key);
                var endP = GetWalkableAdjacent(map, map.Single(i => i.Value == "ZZO").Key);

                var worstCostYet = 0;
                var start = new MapNode(startP.Item1, startP.Item2, 0, map, edges, endP ,0, new string[0]);
                var solution = new RealSolver().Evaluate<MapNode, string, int>(new[] { start }, null, (n) =>
                {
                    if (worstCostYet < n.CurrentCost)
                    {
                        worstCostYet = n.CurrentCost;
                        //Console.WriteLine($"Evaluating node with cost {n.CurrentCost} -> {n.EstimatedCost} on level {n.Level}");
                        //Console.WriteLine($"Evaluating node with cost {n.CurrentCost} -> {n.EstimatedCost} on level {n.Level} @ {string.Join(" -> ", n.Path)}");
                    }
                    //Console.WriteLine($"Evaluating node with cost {n.CurrentCost} -> {n.EstimatedCost} on level {n.Level} @ {string.Join(" -> ", n.Path)}");
                });

                Console.WriteLine(solution.CurrentCost);
                return;


                //if (char.IsLetter(c))
                //{
                //    if (y == 0)
                //    {
                //        map[(x, y)] = c.ToString() + lines[y + 1][x];
                //    }
                //    else if (y == lines.Length - 1)
                //    {
                //        map[(x, y)] = lines[y - 1][x] + c.ToString();
                //    }
                //    else if (x == 0)
                //}
                //else
                //{

                Console.WriteLine(lines.Length);
            });
        }

        private class MapNode : Node<MapNode, string, int>
        {
            private int _x;
            private int _y;
            private (int, int) _target;
            private int _cost;
            private Dictionary<(int, int), string> _map;
            private readonly Dictionary<(int, int), Dictionary<(int, int), (int, int, string)>> _edges;
            private int _level;
            public int Level => _level;
            private string[] _path;
            public string[] Path => _path;

            public MapNode(int x, int y, int cost, Dictionary<(int, int), string> map, Dictionary<(int, int), Dictionary<(int, int), (int, int, string)>> edges, (int, int) target, int level, string[] path)
            {
                _x = x;
                _y = y;
                _cost = cost;
                _map = map;
                _edges = edges;
                _target = target;
                _level = level;
                _path = path;
            }

            public override IEnumerable<MapNode> GetAdjacent()
            {
                var edges = _edges.TryGetValue((_x, _y), out var e) ? e : null;
                if (null == edges)
                {
                    Console.WriteLine($"Cannot find edges for {_x},{_y}");
                    yield break;
                }

                foreach (var edge in edges)
                {
                    var nextX = edge.Key.Item1;
                    var nextY = edge.Key.Item2;

                    if (_level == 0 && edge.Value.Item2 < 0)
                    {
                        continue;
                    }

                    yield return new MapNode(nextX, nextY, _cost + edge.Value.Item1, _map, _edges, _target, _level + edge.Value.Item2, _path.Append(edge.Value.Item3).ToArray());
                }
            }

            public override bool IsValid
            {
                get { return true; }
            }

            public override bool IsComplete
            {
                get { return _target.Item1 == _x && _target.Item2 == _y && _level == 0; }
            }

            public override int CurrentCost
            {
                get { return _cost; }
            }

            public override int EstimatedCost
            {
                get
                {
                    return CurrentCost + _level;
                }
            }

            protected override string GetKey()
            {
                return $"{_x},{_y},{_level}";
            }

            public override string Description
            {
                get { return $"{_x},{_y} = {_cost}"; }
            }
        }

        private (int, int) GetWalkableAdjacent(Dictionary<(int, int), string> map, (int, int) point)
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
                var nextX = point.Item1 + dir.Item1;
                var nextY = point.Item2 + dir.Item2;

                var nextV = map.TryGetValue((nextX, nextY), out var v) ? v : "#";
                if (nextV != ".")
                    continue;

                //Console.WriteLine($"Walkable from {point.Item1},{point.Item2} to {nextX},{nextY}");
                return (nextX, nextY);
            }

            throw new Exception("Can't find an adjacent walkable tile");
        }

        private class SimpleMapNode : Node<SimpleMapNode, (int, int), int>
        {
            private int _x;
            private int _y;
            private (int, int) _target;
            private int _cost;
            private Dictionary<(int, int), string> _map;

            public SimpleMapNode(int x, int y, (int, int) target, int cost, Dictionary<(int, int), string> map)
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

                    var nextV = _map.TryGetValue((nextX, nextY), out var v) ? v : "#";
                    if (nextV != ".")
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


        public override void Run()
        {
            RunScenario("initial", @"         A           
         A           
  #######.#########  
  #######.........#  
  #######.#######.#  
  #######.#######.#  
  #######.#######.#  
  #####  B    ###.#  
BC...##  C    ###.#  
  ##.##       ###.#  
  ##...DE  F  ###.#  
  #####    G  ###.#  
  #########.#####.#  
DE..#######...###.#  
  #.#########.###.#  
FG..#########.....#  
  ###########.#####  
             Z       
             Z       ");
            RunScenario("initial", @"             Z L X W       C                 
             Z P Q B       K                 
  ###########.#.#.#.#######.###############  
  #...#.......#.#.......#.#.......#.#.#...#  
  ###.#.#.#.#.#.#.#.###.#.#.#######.#.#.###  
  #.#...#.#.#...#.#.#...#...#...#.#.......#  
  #.###.#######.###.###.#.###.###.#.#######  
  #...#.......#.#...#...#.............#...#  
  #.#########.#######.#.#######.#######.###  
  #...#.#    F       R I       Z    #.#.#.#  
  #.###.#    D       E C       H    #.#.#.#  
  #.#...#                           #...#.#  
  #.###.#                           #.###.#  
  #.#....OA                       WB..#.#..ZH
  #.###.#                           #.#.#.#  
CJ......#                           #.....#  
  #######                           #######  
  #.#....CK                         #......IC
  #.###.#                           #.###.#  
  #.....#                           #...#.#  
  ###.###                           #.#.#.#  
XF....#.#                         RF..#.#.#  
  #####.#                           #######  
  #......CJ                       NM..#...#  
  ###.#.#                           #.###.#  
RE....#.#                           #......RF
  ###.###        X   X       L      #.#.#.#  
  #.....#        F   Q       P      #.#.#.#  
  ###.###########.###.#######.#########.###  
  #.....#...#.....#.......#...#.....#.#...#  
  #####.#.###.#######.#######.###.###.#.#.#  
  #.......#.......#.#.#.#.#...#...#...#.#.#  
  #####.###.#####.#.#.#.#.###.###.#.###.###  
  #.......#.....#.#...#...............#...#  
  #############.#.#.###.###################  
               A O F   N                     
               A A D   M                     ");



            RunScenario("part2", @"                                 T Z     P       J     A       Y           U                             
                                 D Z     C       R     I       Q           D                             
  ###############################.#.#####.#######.#####.#######.###########.###########################  
  #.#.......#...#.................#...#.#.....#.....#.......#.#.......#.....#.....#...................#  
  #.###.#######.###########.#.#.#.###.#.#####.###.#.#.#######.#####.###.#####.#####.#######.###.#.###.#  
  #...#.#...#...#.#.#.......#.#.#.....#.#.......#.#.#.#.....#.......#...................#.#...#.#.#.#.#  
  ###.#.#.#####.#.#.#.#.#.#.###.#######.#.#.#.###.###.#.###.#.#.#.#########.###.#########.#####.###.###  
  #.......#.#.....#.#.#.#.#...#.......#...#.#...#...#.....#.#.#.#.#.......#...#.........#.....#.#.#...#  
  ###.#####.#####.#.#####.#########.###.###.#####.#########.#.#####.#.###.#.###.###.#.###.#######.#.###  
  #...#.....#.#.#...#...#.#.....#.#...#.#...#.....#.....#...#.....#.#...#...#.#...#.#...#...#.....#...#  
  ###.#.###.#.#.###.#.###.###.#.#.###.#.###.#.#.###.#.#.#.#.#.###.#####.#####.#####.#####.###.#######.#  
  #...#.#.#...#.........#.#.#.#.....#.#...#.#.#...#.#.#...#.#.#.#.#...........#.#.....#...#...#...#.#.#  
  ###.###.#.#########.#####.###.###.#.#.#.#######.###.#.#.###.#.#####.#.###.###.#######.#.#.#####.#.#.#  
  #.....#...#.#...#...#.....#.#.#.....#.#.#...#...#...#.#.#.........#.#...#...#.#...#.#.#.....#.#.....#  
  ###.#####.#.###.###.#####.#.#.###.#.###.#.#.#.#####.#####.#######.###.###.###.###.#.###.###.#.###.###  
  #.#.#.#.....#.#.#...........#...#.#...#...#.#.....#.....#.....#...#.#.#.....#.........#...#.#.#.....#  
  #.#.#.###.###.#.#.#########.#.###.#####.#####.#.#####.###.#######.#.#####.###.#########.#####.#####.#  
  #.......#.#.........#.#.#.....#.#.#.#.#...#.#.#...#.#.#.#.#.#...#...#.#.....................#.#.#.#.#  
  ###.###.#.#####.#####.#.#####.#.###.#.#.###.###.###.#.#.###.#.###.###.###.#######.#####.#.###.#.#.#.#  
  #.#...#.#.#.#...#...#...#.#...#.......#...#.#.#...#.....#...#...........#.....#.#...#.#.#.#.........#  
  #.#####.#.#.#####.###.###.###.###.###.#.###.#.###.#.#######.#.#####.#####.#####.#.###.#######.###.###  
  #.#.#.#.........#.#.....#.......#.#.........#.#...#.....#.#...#...#.#.#.........#.#.#.#.....#.#...#.#  
  #.#.#.#.#########.#####.#####.###.#.#.#.#.###.#.#####.#.#.###.#.#.###.#.###.###.###.#.#.###.#.###.#.#  
  #.....#.......#...#.....#.#.....#.#.#.#.#...#.....#...#.....#...#...#.....#.#...#.#.....#...#...#.#.#  
  ###.#####.#######.#####.#.###.#######.#######.#########.###.#.###.#####.#########.###.#####.#####.#.#  
  #...#.#...#.#...#...#.....#.......#.......#.......#.....#...#.#.....#.....#.......#...#.#.#.#.......#  
  #.#.#.###.#.###.#.#####.#####.#########.#######.#######.#########.#####.#####.#######.#.#.#######.###  
  #.#.#.#.......#...#.....#    Y         M       R       H         J     H    #...#.#...#.......#...#.#  
  #.###.#####.###.###.#####    Q         F       C       G         R     Z    ###.#.###.###.#######.#.#  
  #...............#.......#                                                   #.#.......#.....#.......#  
  ###.#.#####.###.#.#.#.###                                                   #.#.#######.#####.#.#.###  
  #...#.#.#...#.....#.#...#                                                 DD....#.#.....#...#.#.#....HG
  ###.###.#####.###.#####.#                                                   ###.#.###.###.#########.#  
DD..#.....#.#.#.#.#...#....CC                                                 #.....#...#.#.#...#.#.#.#  
  #.#.#.###.#.###.#.#######                                                   #.#######.#.#.#.#.#.#.#.#  
  #...#.#.....#...#.#.....#                                                   #.#.#.#.....#...#.#.....#  
  ###########.###.#####.###                                                   #.###.#.#.#####.#######.#  
  #.......#.........#.....#                                                   #.......#...............#  
  #.#####.#.#####.###.###.#                                                   #########################  
  #.#.#.....#...#.#.....#..QE                                               PN........................#  
  ###.###.###.###.#.#######                                                   ###.#.#.###.#.#.#.#.###.#  
  #.....#.#...#...........#                                                   #.#.#.#...#.#.#.#.#...#.#  
  #.#.#.#.###.#####.#.#.###                                                   #.#.###.###.#.#####.###.#  
ER..#.#...#.#.#...#.#.#...#                                                   #.#...#.#...#.#.......#..HZ
  #########.#.###.#########                                                   #.###.#.###.#########.#.#  
  #.....................#..PC                                               FA..#.#.#.#.....#.#.#...#.#  
  #.#####.#.###.#.#.###.#.#                                                   #.#.###########.#.#####.#  
KW....#...#.#...#.#.#...#.#                                                   #.#.#.#.....#.#.......#.#  
  #.###.#####.###.#####.#.#                                                   #.#.#.###.###.#.#########  
  #.#.....#...#.#...#...#.#                                                   #.......#.......#.....#.#  
  #.#.#.#####.#.#.###.#.#.#                                                   #####.#######.#.#.#.###.#  
  #.#.#.#...#.#...#...#...#                                                   #.#...........#...#......ZU
  #######.#####.###########                                                   #.#######.###########.###  
  #...#.#.#.#.....#.......#                                                   #...#.....#...#.....#...#  
  #.###.#.#.###.#.#####.#.#                                                   #.###.###.###.#.###.#####  
XZ....#.#.#.#.#.#.#.#...#.#                                                   #...#.#.#.#.....#.......#  
  #.###.#.#.#.#####.###.#.#                                                   #.#####.#######.###.###.#  
  #...#...#.#.#.#.#.#...#..KW                                               XI..................#...#..CC
  ###.#.#.#.#.#.#.#.###.###                                                   ###################.#.#.#  
  #.....#.................#                                                   #.........#.......#.#.#.#  
  ###.#.#.###.#####.#######                                                   #.#####.#.#.###.#.#######  
RC..#.#.#...#.#.#.....#.#..JC                                               TD..#.....#...#.#.#...#...#  
  #.###########.#######.#.#                                                   #.###.#.#.###.#.#####.###  
  #...#.#.#.........#...#.#                                                   #.#...#.#.#.#...#........PN
  #.###.#.###.###.#.#.#.#.#                                                   #########.#.#.###.#####.#  
  #...........#...#...#...#                                                   #.#.....#.#.#.........#.#  
  #########################                                                   #.#####.###.#############  
  #...............#.......#                                                   #.#.........#.......#....VD
  ###.#.#########.#.###.###                                                   #.###.#.###.#.#.#.#.###.#  
  #.#.#.#...#.........#.#..UD                                               TS....#.#...#...#.#.#.....#  
  #.#.###.#######.#.###.#.#                                                   #.#.#.###.#.###.###.###.#  
  #...#.....#.#...#.#...#.#                                                 AI..#.#...#.#.#.#.#.....#.#  
  #.#####.###.#########.#.#                                                   ###.#.#######.#####.###.#  
FA..#...#.....#...#.#.#...#                                                   #.#.....#...#...#...#....GB
  ###.###.#####.###.#.#####                                                   #.#######.#####.#########  
  #...#.....#..............ZU                                               WA....#...........#.......#  
  #.#.###.#.#.#####.###.#.#                                                   ###.#.#####.###.#.###.###  
  #.#.#...#.#.#.#...#...#.#                                                   #.....#.#.....#.#.#.....#  
  #.#.#.#.#.#.###.#####.###                                                   #.#.###.#######.#.###.#.#  
  #.#.#.#.#.#...#.#.....#.#                                                   #.#.....#.......#...#.#..JC
  #.#.#.###.###.#######.#.#                                                   #.#.###.#.#.###.###.###.#  
TS..#.....#...........#...#                                                   #.#.#.#.#.#...#.....#...#  
  #.#.#######.#.#.#.#######      E         V     X     K             E G      #####.#########.#.#.#.#.#  
  #.#.#.......#.#.#.#.....#      E         D     Z     J             R B      #.#.#.....#.....#.#.#.#.#  
  #.#######.#####.#######.#######.#########.#####.#####.#############.#.#######.#.#.###.###.#####.#####  
  #.#.#.........#...#...#.....#.....#...#.....#...#...#.........#.#...#...............#.#...#.......#.#  
  #.#.#####.###.#####.#####.#####.#.###.#####.#.###.#.#####.#.###.#.#######.#.#.###.###.###.###.#.###.#  
  #.#.........#...#.#.............#.#.....#...#.#...#.#...#.#.#...#...#.....#.#.#...#.#.#...#...#...#.#  
  #.#.###.#.###.###.#####.###.#.#.#####.###.###.#.###.#.#.###.#.#.###.###.#.#.#.#.#.#.###.###.#.###.#.#  
  #.#...#.#.#.....#.......#.#.#.#...#.#...#...#.#.#...#.#.....#.#.....#...#.#.#.#.#.....#.#...#...#...#  
  ###.###.###.###.#########.###.###.#.#.#####.#.#.#.###.#####.#.#.#####.#####.###.#######.#.#######.#.#  
  #.#...#.#...#.....#.............#.#.........#...#...#.#.....#.#.#...#.#.#.#...#...#.#.#.#...#.....#.#  
  #.#####.#######.#############.#.#######.#####.#.###.#.#####.#.###.#.#.#.#.###.#####.#.###########.#.#  
  #.#.....#.............#.#.....#...#.......#.#.#.#.#.#.....#.#.....#.#.....#.#.#.........#.#.#.....#.#  
  #.#.###.###.#.#.#.#####.###.#.###########.#.#####.#.#.#.#########.#.#####.#.###.#########.#.#.#.###.#  
  #...#...#...#.#.#.#.....#...#.......#.......#.#.....#.#.#.#...#...#.#.#.#.................#.#.#.#.#.#  
  ###.###.###.###.#######.#####.#.#########.###.#.#.#.#.###.#.###.###.#.#.#.#.#.#########.###.#####.#.#  
  #.....#.#...#.#.#.............#...#.#.......#.#.#.#.#.......#.#.#.#.#.....#.#...#.#.............#...#  
  #.#.###.#.###.#########.#####.#####.#.#####.#.#.###.###.###.#.###.#.#####.#####.#.#####.#######.#.#.#  
  #.#.#...#...#...#.#.#.#.#.#.#.#...#...#.#.....#.#.....#.#...#...#.......#.#.#.#...#...........#.#.#.#  
  #.#.#.#.#.#####.#.#.#.###.#.#.#.#.#.#.#.#.#######.#.#.###.#.#.#####.#####.#.#.#########.#.#########.#  
  #.#.#.#.#...#.................#.#.#.#.#...#.#...#.#.#.#...#.#...#...#.#.#.#...........#.#.....#.....#  
  ###.###.###########.#########.#.###.#####.#.###.###.#####.#####.###.#.#.#.#.#.###.#.#.###.###.#####.#  
  #.#.#.#.#.........#...#.........#...#...#.....#...#...#...#.#.......#.......#...#.#.#.#.....#.#.....#  
  #.###.#.###.###.#.#####.#.#.###.#####.#####.###.###.#####.#.###.#######.###.#####.#######.###.#####.#  
  #.......#...#.#.#.#...#.#.#.#.....#...........#.....#.....#.....#...#.#...#.....#...#.......#.#.....#  
  #####.###.#.#.###.###.#######.#######.#####.#####.###.###.###.#####.#.###.#.###.#.#.###.#.#######.#.#  
  #.....#...#.#.......................#.....#...#.....#.#.....#.....#.......#.#...#.#...#.#.#.......#.#  
  #################################.#.#####.#########.###.#######.#####.###############################  
                                   X A     K         W   E       M     Q                                 
                                   I A     J         A   E       F     E                                 ");


        }
    }
}
