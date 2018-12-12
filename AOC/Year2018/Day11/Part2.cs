using System;
using System.Linq;

namespace AoC.Year2018.Day11
{
    public class Part2 : BasePart
    {
        protected void RunScenario(string title, string input)
        {
            RunScenario(title, () =>
            {
                var lines = input.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                var serialNumber = lines[0].Numbers().First();
                //Find the fuel cell's rack ID, which is its X coordinate plus 10.
                //    Begin with a power level of the rack ID times the Y coordinate.
                //    Increase the power level by the value of the grid serial number(your puzzle input).
                //Set the power level to itself multiplied by the rack ID.
                //    Keep only the hundreds digit of the power level(so 12345 becomes 3; numbers with no hundreds digit become 0).
                //Subtract 5 from the power level.

                var cells = new int[301, 301];
                 
                for (var x = 1; x <= 300; x++)
                {
                    for (var y = 1; y <= 300; y++)
                    {
                        cells[x, y] = GetPowerLevel(serialNumber, x, y);
                    }
                }

                var q = Enumerable.Range(1, 300).AsParallel().Select(s =>
                {
                    var maxX = -1;
                    var maxY = -1;
                    var maxSize = -1;
                    var maxLevel = -10000;
                    for (var x = 1; x <= 301 - s; x++)
                    {
                        for (var y = 1; y <= 301 - s; y++)
                        {
                            var level = (from x1 in Enumerable.Range(x, s)
                                    from y1 in Enumerable.Range(y, s)
                                    select cells[x1, y1]
                                ).Sum();
                            if (level > maxLevel)
                            {
                                maxX = x;
                                maxY = y;
                                maxSize = s;
                                maxLevel = level;
                            }
                        }
                    }
                    Console.WriteLine(s);
                    return (maxLevel, maxX, maxY, maxSize);
                }).ToList();

                var (l, x2, y2, size) = q.OrderByDescending(i => i.Item1).First();

                Console.WriteLine($"{x2},{y2},{size}");
            });
        }

        private int GetPowerLevel(int serialNumber, int x, int y)
        {
            var rackid = x + 10;
            var level = rackid * y;
            level += serialNumber;
            level *= rackid;
            level = (level / 100) % 10;
            level -= 5;
            return level;
        }

        public override void Run()
        {
            //RunScenario("initial-1", @"18");
            //RunScenario("initial-2", @"42");
            //return;
            RunScenario("part1", @"3031");

        }
    }
}
