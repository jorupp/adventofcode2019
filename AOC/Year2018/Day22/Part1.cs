using System;
using System.Linq;
using System.Text;

namespace AoC.Year2018.Day22
{
    public class Part1 : BasePart
    {
        protected void RunScenario(string title, string input)
        {
            RunScenario(title, () =>
            {
                var lines = input.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

                var depth = lines[0].Numbers()[0];
                var tx = lines[1].Numbers()[0];
                var ty = lines[1].Numbers()[1];

                var erosionLevels = new long[tx + 1, ty + 1];
                var geologicIndexes = new long[tx + 1, ty + 1];
                var types = new char[tx + 1, ty + 1];

                void DumpMap()
                {
                    var sb = new StringBuilder();
                    for (var y = 0; y <= ty; y++)
                    {
                        for (var x = 0; x <= tx; x++)
                        {
                            sb.Append(types[x, y]);
                        }

                        sb.AppendLine();
                    }
                    Console.WriteLine(sb.ToString());
                }

                long GetRiskLevel(int sx, int sy, int ex, int ey)
                {
                    long sum = 0;
                    for (var y = sy; y <= ey; y++)
                    {
                        for (var x = sx; x <= ex; x++)
                        {
                            sum += types[x, y] == ROCKY ? 0 : types[x, y] == WET ? 1 : 2;
                        }
                    }

                    return sum;
                }

                for (long y = 0; y <= ty; y++)
                {
                    for (long x = 0; x <= tx; x++)
                    {
                        if (x == 0 && y == 0)
                        {
                            geologicIndexes[x, y] = 0;
                        }
                        else if (x == tx && y == ty)
                        {
                            geologicIndexes[x, y] = 0;
                        }
                        else if (y == 0)
                        {
                            geologicIndexes[x, y] = x * 16807;
                        }
                        else if (x == 0)
                        {
                            geologicIndexes[x, y] = y * 48271;
                        }
                        else
                        {
                            geologicIndexes[x, y] = erosionLevels[x - 1, y] * erosionLevels[x, y - 1];
                        }

                        erosionLevels[x, y] = (geologicIndexes[x, y] + depth) % 20183;
                        var levelMod3 = erosionLevels[x, y] % 3;
                        types[x, y] = levelMod3 == 0 ? ROCKY : levelMod3 == 1 ? WET : NARROW;
                    }
                }

                //DumpMap();
                Console.WriteLine(GetRiskLevel(0, 0, tx, ty));
            });
        }

        public const char ROCKY = '.';
        public const char WET = '=';
        public const char NARROW = '|';

        public override void Run()
        {
            RunScenario("initial", @"depth: 510
target: 10,10");
            //return;
            RunScenario("part1", @"depth: 8787
target: 10,725");

        }
    }
}
