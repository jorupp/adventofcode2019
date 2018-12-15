using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Principal;
using System.Text;
using AoC.GraphSolver;

namespace AoC.Year2018.Day15
{
    public class Part1 : BasePart
    {
        protected void RunScenario(string title, string input)
        {
            RunScenario(title, () =>
            {
                var lines = input.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                // bool[y][x]
                var walls = lines.Select(i => i.Select(ii => ii == '#').ToArray()).ToArray();
                var players = lines.SelectMany((i, y) => i.Select((ii, x) => ii == 'E' || ii == 'G' ? new Player()
                {
                    IsElf = ii == 'E',
                    X = x,
                    Y = y,
                } : null).Where(ii => ii != null)).ToArray();

                var round = 0;
                for (;; round++)
                {
                    //Console.WriteLine($"{round} rounds complete");

                    //var sb = new StringBuilder();
                    //for (var y = 0; y < walls.Length; y++)
                    //{
                    //    var linePlayers = new List<Player>();
                    //    for (var x = 0; x < walls[0].Length; x++)
                    //    {
                    //        var player = players.FirstOrDefault(i => i.IsAlive && i.X == x && i.Y == y);
                    //        if (null == player)
                    //        {
                    //            sb.Append(walls[y][x] ? 'X' : '.');
                    //        }
                    //        else
                    //        {
                    //            sb.Append(player.IsElf ? 'E' : 'G');
                    //            linePlayers.Add(player);
                    //        }
                    //    }

                    //    foreach (var player in linePlayers)
                    //    {
                    //        sb.Append($"  {(player.IsElf ? 'E' : 'G')}({player.HP})");
                    //    }
                    //    sb.AppendLine();
                    //}
                    //Console.WriteLine(sb.ToString());


                    var toPlay = players.OrderBy(i => i.Y).ThenBy(i => i.X).ToList();
                    //Console.WriteLine($"    {toPlay.Count} alive");

                    foreach (var player in toPlay)
                    {
                        var otherPlayerLocations = players.Where(i => i.IsAlive && i != player).Select(i => (i.X, i.Y)).ToHashSet();
                        if (!player.IsAlive)
                            continue;
                        var noMove = walls.Select((i, y) => i.Select((ii, x) => ii || otherPlayerLocations.Contains((x,y))).ToArray()).ToArray();
                        var targets = players.Where(i => i.IsAlive && i.IsElf == !player.IsElf).ToList();
                        if (!targets.Any())
                        {
                            goto done;
                        }
                        var moveTargets = targets.SelectMany(i => GetAdjacent(i)).Where(i => !noMove[i.Y][i.X]).Distinct().ToList();
                        if (!moveTargets.Any(i => i.X == player.X && i.Y == player.Y) && moveTargets.Any())
                        {
                            // we move first
                            var paths = moveTargets.Select(i => GetPath(player, i, noMove)).Where(i => i != null).ToList();
                            if (paths.Any(i => i.Length == 0))
                            {

                            }
                            var orderedPaths = paths.OrderBy(i => i.Length).ThenBy(i => i[0].Y).ThenBy(i => i[0].X).ToList();
                            if (round == 0 && player.Y == 1 && player.X == 4)
                            {

                            }
                            var path = orderedPaths.FirstOrDefault();
                            if (null != path)
                            {
                                player.X = path[0].X;
                                player.Y = path[0].Y;
                            }
                        }

                        // now we attack
                        var attackTarget = targets.Where(i => IsAdjacent(player, i)).OrderBy(i => i.HP).ThenBy(i => i.Y).ThenBy(i => i.X).FirstOrDefault();
                        if (null != attackTarget)
                        {
                            attackTarget.HP -= player.Dmg;
                            if (attackTarget.HP <= 0)
                            {
                                attackTarget.IsAlive = false;
                            }
                        }
                    }
                }

                done:
                var hp = players.Where(i => i.IsAlive).Sum(i => i.HP);
                var result = round * hp;

                Console.WriteLine($"{round},{hp},{result}");
            });
        }

        private bool IsBefore(Location l1, Location l2)
        {
            if (l1.Y < l2.Y)
            {
                return true;
            }
            if (l1.Y > l2.Y)
            {
                return false;
            }
            if (l1.X < l2.X)
            {
                return true;
            }
            if (l1.X > l2.X)
            {
                return false;
            }

            return false;
        }

        private Location[] GetPath(Location start, Location end, bool[][] noMove)
        {
            var s = new RealSolver();
            var startNode = new LocationNode(start, null, noMove, end);
            var result = s.Evaluate(startNode, startNode.Key, (newOne, oldOne) => IsBefore(newOne.First, oldOne.First));
            if (null == result)
                return null;
            return result.FullPath.ToArray();
        }

        private class LocationNode : Node<LocationNode, string>
        {
            private readonly Location _location;
            private readonly LocationNode _prev;
            private readonly bool[][] _noMove;
            private readonly Location _target;
            public IEnumerable<Location> FullPath => null == _prev ? Enumerable.Empty<Location>() : _prev.FullPath.Concat(new[] {_location});
            public Location First => FullPath.FirstOrDefault();


            public LocationNode(Location location, LocationNode prev, bool[][] noMove, Location target)
            {
                _location = location;
                _prev = prev;
                _noMove = noMove;
                _target = target;
            }

            public override IEnumerable<LocationNode> GetAdjacent()
            {
                return Part1.GetAdjacent(_location).Where(i => !_noMove[i.Y][i.X]).Select(i => new LocationNode(i, this, _noMove, _target)).ToArray();
            }

            public override bool IsValid { get; } = true;
            public override bool IsComplete => _location.Equals(_target);
            public override decimal CurrentCost => _prev == null ? 1 : _prev.CurrentCost + 1;
            public override decimal EstimatedCost => CurrentCost + Math.Abs(_location.X - _target.X) + Math.Abs(_location.Y - _target.Y);
            protected override string GetKey()
            {
                return $"{_location.X}_{_location.Y}";
            }

            public override string Description => string.Join(" -> ", FullPath.Select(i => $"{i.X},{i.Y}"));
        }

        private static Location[] GetAdjacent(Location l)
        {
            return new[]
            {
                new Location() {X = l.X - 1, Y = l.Y},
                new Location() {X = l.X + 1, Y = l.Y},
                new Location() {X = l.X , Y = l.Y - 1},
                new Location() {X = l.X , Y = l.Y + 1},
            };
        }

        private static bool IsAdjacent(Location l1, Location l2)
        {
            return (Math.Abs(l1.X - l2.X) == 1 && l1.Y == l2.Y) ||
                   (Math.Abs(l1.Y - l2.Y) == 1 && l1.X == l2.X);
        }

        private class Location
        {
            public int X;
            public int Y;

            protected bool Equals(Location other)
            {
                return X == other.X && Y == other.Y;
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                if (ReferenceEquals(this, obj)) return true;
                if (obj.GetType() != this.GetType()) return false;
                return Equals((Location) obj);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    return (X * 397) ^ Y;
                }
            }
        }

        private class Player : Location
        {
            public bool IsElf;
            public int HP = 200;
            public int Dmg = 3;
            public bool IsAlive = true;
        }

        public override void Run()
        {
            RunScenario("initial", @"#########
#G..G..G#
#.......#
#.......#
#G..E..G#
#.......#
#.......#
#G..G..G#
#########
");

            RunScenario("initial", @"#######
#.G...#
#...EG#
#.#.#G#
#..G#E#
#.....#
#######");

            RunScenario("part1", @"#######
#G..#E#
#E#E.E#
#G.##.#
#...#E#
#...E.#
#######
");

            RunScenario("part1", @"#######
#E..EG#
#.#G.E#
#E.##E#
#G..#.#
#..E#.#
#######
");

            RunScenario("part1", @"#######
#E.G#.#
#.#G..#
#G.#.G#
#G..#.#
#...E.#
#######
");

            RunScenario("part1", @"#######
#.E...#
#.#..G#
#.###.#
#E#G#G#
#...#G#
#######
");

            RunScenario("part1", @"#########
#G......#
#.E.#...#
#..##..G#
#...##..#
#...#...#
#.G...G.#
#.....G.#
#########
");

            //return;
            RunScenario("part1", @"################################
#######..G######################
########.....###################
##########....############.....#
###########...#####..#####.....#
###########G..###GG....G.......#
##########.G#####G...#######..##
###########...G.#...############
#####.#####..........####....###
####.....###.........##.#....###
####.#................G....#####
####......#.................####
##....#G......#####........#####
########....G#######.......#####
########..G.#########.E...######
########....#########.....######
#######.....#########.....######
#######...G.#########....#######
#######...#.#########....#######
####.G.G.....#######...#.#######
##...#...G....#####E...#.#######
###..#.G.##...E....E.......###.#
######...................#....E#
#######...............E.########
#G###...#######....E...#########
#..##.######.E#.#.....##########
#..#....##......##.E...#########
#G......###.#..##......#########
#....#######....G....E.#########
#.##########..........##########
#############.###.......########
################################");

        }
    }
}
