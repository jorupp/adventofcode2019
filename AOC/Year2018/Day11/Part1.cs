using System;
using System.Linq;

namespace AoC.Year2018.Day11
{
    public class Part1 : BasePart
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
                var levels = new int[299, 299];

                for (var x = 1; x <= 300; x++)
                {
                    for (var y = 1; y <= 300; y++)
                    {
                        cells[x, y] = GetPowerLevel(serialNumber, x, y);
                    }
                }

                var maxX = -1;
                var maxY = -1;
                var maxLevel = -10000;
                for (var x = 1; x <= 298; x++)
                {
                    for (var y = 1; y <= 298; y++)
                    {
                        var level = 
                            cells[x, y] + cells[x + 1, y] + cells[x + 2, y] +
                            cells[x, y + 1] + cells[x + 1, y + 1] + cells[x + 2, y + 1] +
                            cells[x, y + 2] + cells[x + 1, y + 2] + cells[x + 2, y + 2];
                        if (level > maxLevel)
                        {
                            maxX = x;
                            maxY = y;
                            maxLevel = level;
                        }
                    }
                }


                Console.WriteLine($"{maxX},{maxY}");
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
