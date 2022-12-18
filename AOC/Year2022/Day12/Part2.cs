using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using AoC;
using AoC.GraphSolver;

namespace AOC.Year2022.Day12
{
    public class Part2 : BasePart
    {
        // 9:32:12 - 10:11:10: 38m58s, 100 @ 9m46s
        protected void RunScenario(string title, string input)
        {
            RunScenario(title, () =>
            {
                var lines = input.Replace("\r\n", "\n").Split("\n", StringSplitOptions.RemoveEmptyEntries).Select(i => i.ToArray()).ToArray();

                var start = (x: 0, y: 0);
                var ends = new List<(int x, int y)>();

                for (var y = 0; y < lines.Length; y++)
                {
                    for (var x = 0; x < lines[y].Length; x++)
                    {
                        if (lines[y][x] == 'S')
                        {
                            lines[y][x] = 'a';
                        }
                        if (lines[y][x] == 'E')
                        {
                            start = (x, y);
                            lines[y][x] = 'z';
                        }
                        if (lines[y][x] == 'a')
                        {
                            ends.Add((x, y));
                        }
                    }
                }


                var s = new RealSolver();
                var result = s.Evaluate<MapNode, string, decimal>(new MapNode(lines, start, ends, 0, null));

                var path = Enumerable.Range(0, lines.Length).Select(i => Enumerable.Range(0, lines[i].Length).Select(ii => '.').ToArray()).ToArray();
                var map = Enumerable.Range(0, lines.Length).Select(i => Enumerable.Range(0, lines[i].Length).Select(ii => lines[i][ii]).ToArray()).ToArray();
                var t = result;
                while (t != null)
                {
                    var p = t.prior;
                    if (p != null)
                    {
                        if (t.point.x > p.point.x)
                        {
                            path[p.point.y][p.point.x] = '>';
                        }
                        else if (t.point.x < p.point.x)
                        {
                            path[p.point.y][p.point.x] = '<';
                        }
                        else if (t.point.y > p.point.y)
                        {
                            path[p.point.y][p.point.x] = 'v';
                        }
                        else if (t.point.y < p.point.y)
                        {
                            path[p.point.y][p.point.x] = '^';
                        }
                        else
                        {
                            throw new NotImplementedException();
                        }
                    }
                    map[t.point.y][t.point.x] = (char)(map[t.point.y][t.point.x] + 'A' - 'a');
                    t = p;
                }

                for (var y = 0; y < path.Length; y++)
                {
                    for (var x = 0; x < path[y].Length; x++)
                    {
                        Console.Write(path[y][x]);
                    }
                    Console.WriteLine();
                }

                for (var y = 0; y < map.Length; y++)
                {
                    for (var x = 0; x < map[y].Length; x++)
                    {
                        Console.Write(map[y][x]);
                    }
                    Console.WriteLine();
                }

                Console.WriteLine(result.cost);

            });
        }

        private static readonly List<(int x, int y)> directions = new List<(int x, int y)>
        {
            (x:0, y:-1),
            (x:0, y:1),
            (x:-1, y:0),
            (x:1, y:0),
        };

        private class MapNode : Node<MapNode>
        {
            private readonly char[][] map;
            public readonly (int x, int y) point;
            private readonly ICollection<(int x, int y)> targets;
            public readonly int cost;
            public readonly MapNode prior;

            public MapNode(char[][] map, (int x, int y) point, ICollection<(int x, int y)> targets, int cost, MapNode prior)
            {
                this.map = map;
                this.point = point;
                this.targets = targets;
                this.cost = cost;
                this.prior = prior;
            }

            public override object[] Keys => new object[] { point.x, point.y };

            public override bool IsValid => true;

            public override bool IsComplete => targets.Any(target => point.x == target.x && point.y == target.y);

            public override decimal CurrentCost => cost;

            public override decimal EstimatedCost => CurrentCost + targets.Min(target => Math.Abs(point.x - target.x) + Math.Abs(point.y - target.y));

            public override string Description => null;

            public override IEnumerable<MapNode> GetAdjacent()
            {
                foreach (var dir in directions)
                {
                    var p2 = (x: point.x + dir.x, y: point.y + dir.y);
                    if (p2.x < 0 || p2.y < 0 || p2.y >= map.Length || p2.x >= map[p2.y].Length) { continue; }
                    var cH = map[point.y][point.x];
                    var tH = map[p2.y][p2.x];
                    if (tH >= cH - 1)
                    {
                        //Console.WriteLine($"considering {p2.x},{p2.y}");
                        yield return new MapNode(map, p2, targets, cost + 1, this);
                    }
                }
            }
        }

        public override void Run()
        {
            RunScenario("initial", @"Sabqponm
abcryxxl
accszExk
acctuvwj
abdefghi");
            //return;
            RunScenario("part1", @"abccccaaaaaaacccaaaaaaaccccccccccccccccccccccccccccccccccaaaa
abcccccaaaaaacccaaaaaaaaaaccccccccccccccccccccccccccccccaaaaa
abccaaaaaaaaccaaaaaaaaaaaaaccccccccccccccccccccccccccccaaaaaa
abccaaaaaaaaaaaaaaaaaaaaaaacccccccccaaaccccacccccccccccaaacaa
abaccaaaaaaaaaaaaaaaaaacacacccccccccaaacccaaaccccccccccccccaa
abaccccaaaaaaaaaaaaaaaacccccccccccccaaaaaaaaaccccccccccccccaa
abaccccaacccccccccaaaaaacccccccccccccaaaaaaaacccccccccccccccc
abcccccaaaacccccccaaaaaaccccccccijjjjjjaaaaaccccccaaccaaccccc
abccccccaaaaacccccaaaacccccccciiijjjjjjjjjkkkkkkccaaaaaaccccc
abcccccaaaaacccccccccccccccccciiiirrrjjjjjkkkkkkkkaaaaaaccccc
abcccccaaaaaccccccccccccccccciiiirrrrrrjjjkkkkkkkkkaaaaaccccc
abaaccacaaaaacccccccccccccccciiiqrrrrrrrrrrssssskkkkaaaaacccc
abaaaaacaaccccccccccccccccccciiiqqrtuurrrrrsssssskklaaaaacccc
abaaaaacccccccccccaaccccccccciiqqqttuuuurrssusssslllaaccccccc
abaaaaaccccccccaaaaccccccccciiiqqqttuuuuuuuuuuusslllaaccccccc
abaaaaaacccccccaaaaaaccccccciiiqqqttxxxuuuuuuuusslllccccccccc
abaaaaaaccccaaccaaaaacccccchhiiqqtttxxxxuyyyyvvsslllccccccccc
abaaacacccccaacaaaaaccccccchhhqqqqttxxxxxyyyyvvsslllccccccccc
abaaacccccccaaaaaaaacccccchhhqqqqtttxxxxxyyyvvssqlllccccccccc
abacccccaaaaaaaaaaccaaacchhhpqqqtttxxxxxyyyyvvqqqlllccccccccc
SbaaacaaaaaaaaaaaacaaaaahhhhppttttxxEzzzzyyvvvqqqqlllcccccccc
abaaaaaaacaaaaaacccaaaaahhhppptttxxxxxyyyyyyyvvqqqlllcccccccc
abaaaaaaccaaaaaaaccaaaaahhhppptttxxxxywyyyyyyvvvqqqmmcccccccc
abaaaaaaacaaaaaaacccaaaahhhpppsssxxwwwyyyyyyvvvvqqqmmmccccccc
abaaaaaaaaaaaaaaacccaacahhhpppssssssswyyywwvvvvvqqqmmmccccccc
abaaaaaaaacacaaaacccccccgggppppsssssswwywwwwvvvqqqqmmmccccccc
abcaaacaaaccccaaaccccccccgggppppppssswwwwwrrrrrqqqmmmmccccccc
abcaaacccccccccccccccccccgggggpppoosswwwwwrrrrrqqmmmmddcccccc
abccaacccccccccccccccccccccgggggoooosswwwrrrnnnmmmmmddddccccc
abccccccccccccccccccccccccccgggggooossrrrrrnnnnnmmmddddaccccc
abaccccaacccccccccccccccccccccgggfoossrrrrnnnnndddddddaaacccc
abaccaaaaaaccccccccccccccccccccgffooorrrrnnnneeddddddaaaacccc
abaccaaaaaacccccccccccccccccccccfffooooonnnneeeddddaaaacccccc
abacccaaaaaccccccccaaccaaaccccccffffoooonnneeeeccaaaaaacccccc
abcccaaaaacccccccccaaccaaaaccccccffffoooneeeeeaccccccaacccccc
abaccaaaaaccccccccaaaacaaaaccccccafffffeeeeeaaacccccccccccccc
abacccccccccccccccaaaacaaacccccccccffffeeeecccccccccccccccaac
abaaaacccccccaaaaaaaaaaaaaacccccccccfffeeeccccccccccccccccaaa
abaaaacccccccaaaaaaaaaaaaaaccccccccccccaacccccccccccccccccaaa
abaacccccccccaaaaaaaaaaaaaaccccccccccccaacccccccccccccccaaaaa
abaaaccccccccccaaaaaaaaccccccccccccccccccccccccccccccccaaaaaa");

        }
    }
}
