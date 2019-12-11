using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC.Year2019.Day10
{
    public class Part1 : BasePart
    {
        protected void RunScenario(string title, string input)
        {
            RunScenario(title, () =>
            {
                var lines = input.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

                var points = lines.Select((iy, y) => iy.Select((ix, x) => new {x, y, ix})).SelectMany(i => i).Where(i => i.ix == '#')
                    .Select(i => (i.x, i.y)).ToList();

                var q = (from p1 in points
                        from p2 in points
                        where p1 != p2
                        select new {p1, p2, angle = GetAngle(p1, p2)})
                    .GroupBy(i => i.p1)
                    .ToDictionary(i => i.Key, i => i.Select(ii => (p2: ii.p2, angle: ii.angle)).ToList());

                var canSee = new Dictionary<(int x, int y), int>();
                foreach (var x in q)
                {
                    var list = new List<((int x, int y) p2, (int x, int y) angle)>();
                    foreach (var xx in x.Value)
                    {
                        if (!list.Any(i => AreEqual(i.angle, xx.angle)))
                        {
                            list.Add(xx);
                        }
                    }
                    canSee.Add(x.Key, list.Count);

                    //for (var iy = 0; iy < lines.Length; iy++)
                    //{
                    //    for (var ix = 0; ix < lines[0].Length; ix++)
                    //    {
                    //        if (x.Key == (ix, iy))
                    //        {
                    //            Console.Write("#");
                    //        }
                    //        else if (list.Any(i => i.p2 == (ix, iy)))
                    //        {
                    //            Console.Write("A");
                    //        }
                    //        else if (x.Value.Any(i => i.p2 == (ix, iy)))
                    //        {
                    //            Console.Write("a");
                    //        }
                    //        else
                    //        {
                    //            Console.Write(" ");
                    //        }
                    //    }
                    //    Console.WriteLine();
                    //}
                    //Console.WriteLine(string.Join(" - ", list.Select(i => $"{i.p2.x},{i.p2.y}")));
                    //Console.WriteLine($"{list.Count} - {x.Value.Count} - {x.Key.x},{x.Key.y}");
                    //Console.WriteLine();
                }

                Console.WriteLine(canSee.Values.Max());
            });
        }

        private int SignOf(int i)
        {
            return i > 0 ? 1 : i == 0 ? 0 : -1;
        }

        private bool AreEqual((int x, int y) a1, (int x, int y) a2)
        {
            if (SignOf(a1.y) != SignOf(a2.y))
                return false;
            if (SignOf(a1.x) != SignOf(a2.x))
                return false;

            if (a1.y > a2.y)
            {
                return AreEqual(a2, a1);
            }

            //var factor = a1.y == 0 ? 0 : a2.y / a1.y;
            //if (a1.y * factor != a2.y)
            //    return false;
            //if (a1.x * factor != a2.x)
            //    return false;

            //return true;

            //if (a1.y == 0 || a2.y == 0)
            //{
            //    if (a1.y == 0 && a2.y == 0)
            //    {
            //        return true;
            //    }

            //    return false;
            //}



            var aa1 = (a1.x * a2.y, a1.y * a2.y);
            var aa2 = (a2.x * a1.y, a2.y * a1.y);
            return aa1.Item1 == aa2.Item1;
        }

        private (int x, int y) GetAngle((int x, int y) p1, (int x, int y) p2)
        {
            return (p2.x - p1.x, p2.y - p1.y);
        }

        public override void Run()
        {
            RunScenario("simple", @".#..#
.....
#####
....#
...##");
            //return;
            RunScenario("initial", @"......#.#.
#..#.#....
..#######.
.#.#.###..
.#..#.....
..#....#.#
#..#....#.
.##.#..###
##...#..#.
.#....####");
            RunScenario("initial", @"#.#...#.#.
.###....#.
.#....#...
##.#.#.#.#
....#.#.#.
.##..###.#
..#...##..
..##....##
......#...
.####.###.");
            RunScenario("initial", @".#..#..###
####.###.#
....###.#.
..###.##.#
##.##.#.#.
....###..#
..#.#..#.#
#..#.#.###
.##...##.#
.....#.#..");
            RunScenario("initial", @".#..##.###...#######
##.############..##.
.#.######.########.#
.###.#######.####.#.
#####.##.#.##.###.##
..#####..#.#########
####################
#.####....###.#.#.##
##.#################
#####.##.###..####..
..######..##.#######
####.##.####...##..#
.#####..#.######.###
##...#.##########...
#.##########.#######
.####.#.###.###.#.##
....##.##.###..#####
.#.#.###########.###
#.#.#.#####.####.###
###.##.####.##.#..##");
            //return;
            RunScenario("part1", @"#....#.....#...#.#.....#.#..#....#
#..#..##...#......#.....#..###.#.#
#......#.#.#.....##....#.#.....#..
..#.#...#.......#.##..#...........
.##..#...##......##.#.#...........
.....#.#..##...#..##.....#...#.##.
....#.##.##.#....###.#........####
..#....#..####........##.........#
..#...#......#.#..#..#.#.##......#
.............#.#....##.......#...#
.#.#..##.#.#.#.#.......#.....#....
.....##.###..#.....#.#..###.....##
.....#...#.#.#......#.#....##.....
##.#.....#...#....#...#..#....#.#.
..#.............###.#.##....#.#...
..##.#.........#.##.####.........#
##.#...###....#..#...###..##..#..#
.........#.#.....#........#.......
#.......#..#.#.#..##.....#.#.....#
..#....#....#.#.##......#..#.###..
......##.##.##...#...##.#...###...
.#.....#...#........#....#.###....
.#.#.#..#............#..........#.
..##.....#....#....##..#.#.......#
..##.....#.#......................
.#..#...#....#.#.....#.........#..
........#.............#.#.........
#...#.#......#.##....#...#.#.#...#
.#.....#.#.....#.....#.#.##......#
..##....#.....#.....#....#.##..#..
#..###.#.#....#......#...#........
..#......#..#....##...#.#.#...#..#
.#.##.#.#.....#..#..#........##...
....#...##.##.##......#..#..##....");

        }
    }
}
