using System;
using System.Linq;
using System.Text;

namespace AoC.Year2018.Day18
{
    public class Part1 : BasePart
    {
        protected void RunScenario(string title, string input)
        {
            RunScenario(title, () =>
            {
                var data = input.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries).Select(i => i.ToArray()).ToArray();

                char NextRound(int x, int y)
                {
                    var around =
                        (from xi in Enumerable.Range(x - 1, 3)
                            from yi in Enumerable.Range(y - 1, 3)
                            where xi != x || yi != y
                            where xi >= 0 && xi < data[0].Length
                            where yi >= 0 && yi < data.Length
                            select data[yi][xi]).ToArray();
                    var current = data[y][x];
                    switch (current)
                    {
                        //An open acre will become filled with trees if three or more adjacent acres contained trees. Otherwise, nothing happens.
                        case Open:
                            if (around.Count(i => i == Trees) >= 3)
                            {
                                return Trees;
                            }

                            return current;
                        //An acre filled with trees will become a lumberyard if three or more adjacent acres were lumberyards. Otherwise, nothing happens.
                        case Trees:
                            if (around.Count(i => i == Lumberyard) >= 3)
                            {
                                return Lumberyard;
                            }

                            return current;
                        //An acre containing a lumberyard will remain a lumberyard if it was adjacent to at least one other lumberyard and at least one acre containing trees.Otherwise, it becomes open.
                        case Lumberyard:
                            if (around.Count(i => i == Lumberyard) >= 1 && around.Count(i => i == Trees) >= 1)
                            {
                                return Lumberyard;
                            }

                            return Open;
                    }

                    throw new Exception();
                }

                void DumpState()
                {
                    var sb = new StringBuilder();
                    for (var y = 0; y < data.Length; y++)
                    {
                        for (var x = 0; x < data[y].Length; x++)
                        {
                            sb.Append(data[y][x]);
                        }

                        sb.AppendLine();
                    }
                    Console.WriteLine(sb.ToString());
                }

                for (var m = 0; m<10; m++)
                {
                    //DumpState();
                    var nextRound = data.Select((l, y) => l.Select((i, x) => NextRound(x, y)).ToArray()).ToArray();
                    data = nextRound;
                }

                //DumpState();

                // Multiplying the number of wooded acres by the number of lumberyards gives the total resource value after ten minutes: 37 * 31 = 1147.
                var lumber = data.Sum(i => i.Count(ii => ii == Lumberyard));
                var wooded = data.Sum(i => i.Count(ii => ii == Trees));
                var result = lumber * wooded;

                Console.WriteLine(result);
            });
        }

        private const char Open = '.';
        private const char Trees = '|';
        private const char Lumberyard = '#';


        public override void Run()
        {
            RunScenario("initial", @".#.#...|#.
.....#|##|
.|..|...#.
..|#.....#
#.#|||#|#|
...#.||...
.|....|...
||...#|.#|
|.||||..|.
...#.|..|.");

            RunScenario("part1", @".|#.#|..#...|..##||...|#..##..#..|#|....#.#|.|....
.||....#..#...|#....#.||....||...||...|..#|..||..|
......|.|.#.#.#..|.....#.###.....#........|.||..#|
..|.....||...#||#.#|#.....|##.|.|....|#....#|..#.#
|...#.|..#|#.#....|.#.#.|.#...#..#|#.....##|#..#.|
#....|#|......#.|||..#..#..||...#.#...|||##|..|#..
.#||.|....|.......#|##...|.#.....#.##...|.|.#...#|
....#|.|.|...##.......|#.....|..#......#...#|.#..#
...#.|....#.|.#...|||......##..|#|||#..#...|.|#.#.
.#..|...|..#.|##.#.#......#...||.||.#...|.#.#.#|..
|...#.|||...#|.#||.......|.##.....|..||...####...|
.##..|..##|...##.#...#...#.#|.|###...#............
|.....||...#.......|.#..|#.....|.|#.|..||.##.|#|..
.#..##|.#|.|..|.#..#.|.#..#|......##...#.#.......#
...#.##.|..|.#.....#....#..#.............|.|##||..
||.##.||.|.|..|..#.|.|.##|.|...|.#.|#.......#.|...
..|#.##..#|.#|#.#...........|.|........#...#|...#|
....|.#|..|.#|#|...|.|.#..#.....#|##|||##.#....#..
...###.#.....#.||......#|#..##...|#....#....|#|.#.
.##.|.##|.#.||....#|....|.#.|#.|....##..#.##|.....
|...|...#|....#....#...#|#...|..#.#.|.|.....|.#|..
.|.|.#.#.#|.#.|#....|.|###..#......|...|...#.|#...
..|...||.|..##|...|..|#|...|......#.||.#...#..|#.|
........|..#||..|....|.....|.|#..#....|#..|.#.....
#.|.|#....#...|..|....|.#.....||.....|..|........#
...||||....|.#.|....#..#....|###....|...#...##....
|||........|.#|.|......||#.|.....|.||#|.##....#|..
.....|#|#..||#...|##.|..||....####.|#.|..#....|.#.
.||..#||....#.....#.#|.|....|.##|..|.#..|##....##.
.|#.#|#|#|.....||..|.|.|.#......#..|.#..#..|.#||#.
|.|#.......|..#|#|....|.#.#.#.|...|.......##.|||#|
..|.....#...||.|....|##|...#..#.#.....|##|##.##...
.|.|..##.#|..|.|#.......#....#||.|...||#...|......
|.|##.#....|#..|....#..#..|##.|.##..#......#|##|..
..#....#.|#...#.#...|.....|.||.#.#|.#.|###..|..#.#
..|.##...........|..###.||.|.##.|....|.|.#|#.#.|#|
..|....|.|#|...#|#...|.#......#.#||...|.#|...#.|#.
..#.......|.||.....||.|....|#||..........#...|#...
.|..#....|#|||#..##||..#|.......|..|###..|.#...|.|
|..|.#|.#...#....|.....#.....#....#...|..|.|.#.|.#
....###.#....|.#..#...#...###.|.|.....#|...#.....|
..#....##.....##..|.#.||#.|.#|#||..|...#|..|.#....
|#..#.#|||#.|#..#........#......||...#.|..#|....#|
......#|...#.|...#...|.|...|#|#......#|.##.#|.|.#|
#||.#......#.##......#..||.##|.|.||..|#....#..#...
#.#...#.|.#|#||#.#......#....|##|.........##.#|...
.....###...#||....|####..#|||...#..#|.|....#|..#..
......|#..#.#.#..|.#|#||..||.|...#....##...|......
...#...|..#..##.||.#.#.....|.###.....##|#||..#..#|
.#..#||.#....||....|##..|||...|.||...#..##.#....#.");

        }
    }
}
