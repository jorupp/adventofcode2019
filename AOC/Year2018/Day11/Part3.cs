using System;
using System.Linq;

namespace AoC.Year2018.Day11
{
    public class Part3 : BasePart
    {
        // algorithm from: https://www.reddit.com/r/adventofcode/comments/a53r6i/2018_day_11_solutions/ebjogd7/
        // runtime:
        //    Day11-Part3-initial-1 completed in 00:00:00.1259053
        //    Day11-Part3-initial-2 completed in 00:00:00.0986240
        //    Day11-Part3-part1 completed in 00:00:00.0991518
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

                // only track the sum of the power levels from the top-left corner to each point
                var sums = new int[301, 301];
                 
                for (var x = 1; x <= 300; x++)
                {
                    for (var y = 1; y <= 300; y++)
                    {
                        sums[x, y] = sums[x-1, y] + sums[x, y-1] - sums[x-1, y-1] + GetPowerLevel(serialNumber, x, y);
                    }
                }

                // a square bounded by TL, TR, BL, BR has the same power as BR - BL - TR + TL
                var maxX = -1;
                var maxY = -1;
                var maxSize = -1;
                var maxLevel = -10000;
                for(var s=0; s < 300; s++)
                {
                    for (var x = 1; x <= 300 - s; x++)
                    {
                        for (var y = 1; y <= 300 - s; y++)
                        {
                            var level = sums[x + s, y + s] - sums[x - 1, y + s] - sums[x + s, y - 1] +
                                        sums[x - 1, y - 1];
                            if (level > maxLevel)
                            {
                                maxX = x;
                                maxY = y;
                                maxSize = s + 1;
                                maxLevel = level;
                            }
                        }
                    }
                }

                Console.WriteLine($"{maxX},{maxY},{maxSize}");
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
            RunScenario("initial-1", @"18");
            RunScenario("initial-2", @"42");
            //return;
            RunScenario("part1", @"3031");

        }
    }
}
