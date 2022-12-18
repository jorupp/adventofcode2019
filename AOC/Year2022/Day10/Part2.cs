using System;
using System.Linq;
using AoC;

namespace AOC.Year2022.Day10
{
    public class Part2 : BasePart
    {
        // 11:11:00 - 11:25:35, time: 14m35s, 100 @ 12m17s
        protected void RunScenario(string title, string input)
        {
            RunScenario(title, () =>
            {
                var lines = input.Replace("\r\n", "\n").Split("\n", StringSplitOptions.RemoveEmptyEntries);

                var x = 1;
                var c = 1; // we are _currently_ executing cycle 1
                var signal = 0;

                void Check()
                {
                    var cx = (c-1) % 40;
                    if (Math.Abs(cx-x) <= 1)
                    {
                        Console.Write("#");
                    } else
                    {
                        Console.Write(".");
                    }

                    if (c % 40 == 0)
                    {
                        Console.WriteLine();
                    }
                }

                foreach (var line in lines)
                {
                    Check();

                    var parts = line.Split(" ");
                    switch (parts[0])
                    {
                        case "addx":
                            var v = int.Parse(parts[1]);
                            c++;

                            Check();
                            x += v;
                            c++;
                            break;
                        case "noop":
                            c++;
                            break;
                    }
                }

                Console.WriteLine(signal);
            });
        }

        public override void Run()
        {
            RunScenario("initial", @"addx 15
addx -11
addx 6
addx -3
addx 5
addx -1
addx -8
addx 13
addx 4
noop
addx -1
addx 5
addx -1
addx 5
addx -1
addx 5
addx -1
addx 5
addx -1
addx -35
addx 1
addx 24
addx -19
addx 1
addx 16
addx -11
noop
noop
addx 21
addx -15
noop
noop
addx -3
addx 9
addx 1
addx -3
addx 8
addx 1
addx 5
noop
noop
noop
noop
noop
addx -36
noop
addx 1
addx 7
noop
noop
noop
addx 2
addx 6
noop
noop
noop
noop
noop
addx 1
noop
noop
addx 7
addx 1
noop
addx -13
addx 13
addx 7
noop
addx 1
addx -33
noop
noop
noop
addx 2
noop
noop
noop
addx 8
noop
addx -1
addx 2
addx 1
noop
addx 17
addx -9
addx 1
addx 1
addx -3
addx 11
noop
noop
addx 1
noop
addx 1
noop
noop
addx -13
addx -19
addx 1
addx 3
addx 26
addx -30
addx 12
addx -1
addx 3
addx 1
noop
noop
noop
addx -9
addx 18
addx 1
addx 2
noop
noop
addx 9
noop
noop
noop
addx -1
addx 2
addx -37
addx 1
addx 3
noop
addx 15
addx -21
addx 22
addx -6
addx 1
noop
addx 2
addx 1
noop
addx -10
noop
noop
addx 20
addx 1
addx 2
addx 2
addx -6
addx -11
noop
noop
noop");
            //return;
            RunScenario("part1", @"addx 1
addx 5
noop
addx -1
noop
addx 3
addx 29
addx -1
addx -21
addx 5
noop
addx -20
addx 21
addx 2
addx 8
addx -1
noop
noop
noop
noop
addx 6
addx -1
addx -37
addx 40
addx -10
addx -25
addx 5
addx 2
addx 5
noop
noop
noop
addx 21
addx -20
addx 2
noop
addx 3
addx 2
addx -5
addx 12
addx 3
noop
addx 2
addx 3
addx -2
addx -37
addx 1
addx 5
addx 3
addx -2
addx 2
addx 29
addx -22
addx 13
noop
addx -8
addx -6
addx 7
addx 2
noop
addx 7
addx -2
addx 5
addx 2
addx -26
addx -11
noop
noop
addx 6
addx 1
addx 1
noop
addx 4
addx 5
noop
noop
addx -2
addx 3
noop
addx 2
addx 5
addx 2
addx -22
addx 27
addx -1
addx 1
addx 5
addx 2
noop
addx -39
addx 22
noop
addx -15
addx 3
addx -2
addx 2
addx -2
addx 9
addx 3
noop
addx 2
addx 3
addx -2
addx 2
noop
noop
noop
addx 5
addx -17
addx 24
addx -7
addx 8
addx -36
addx 2
addx 3
addx 33
addx -32
addx 4
addx 1
noop
addx 5
noop
noop
addx 20
addx -15
addx 4
noop
addx 1
noop
addx 4
addx 6
addx -30
addx 30
noop
noop
noop
noop
noop");

        }
    }
}
