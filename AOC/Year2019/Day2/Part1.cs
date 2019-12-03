using System;
using System.Linq;

namespace AoC.Year2019.Day2
{
    // 9:31:00 (1:58)
    // 9:36:31 -> 5:31 (7:29 -> 5:31) -> #58
    // 9:43:31 -> 12:31 (11:00 -> 10:33 if I hadn't screwed it up, would have been ~94)
    public class Part1 : BasePart
    {
        protected void RunScenario(string title, string input)
        {
            RunScenario(title, () =>
            {
                var lines = input.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                var data = lines[0].Split(',').Select(int.Parse).ToArray();

                var ip = 0;

                data[1] = 12;
                data[2] = 2;

                while (true)
                {
                    var i = data[ip];
                    switch (i)
                    {
                        case 1:
                            data[data[ip + 3]] = data[data[ip + 1]] + data[data[ip + 2]];
                            break;
                        case 2:
                            data[data[ip + 3]] = data[data[ip + 1]] * data[data[ip + 2]];
                            break;
                        case 99:
                            goto done;
                        default:
                            throw new NotImplementedException();
                    }

                    ip+=4;
                }
                done:

                Console.WriteLine(string.Join(",", data));
                Console.WriteLine(data[0]);
            });
        }

        public override void Run()
        {
            //RunScenario("initial", @"1,0,0,0,99");
            //RunScenario("initial", @"2,3,0,3,99");
            //RunScenario("initial", @"2,4,4,5,99,0");
            //RunScenario("initial", @"1,1,1,4,99,5,6,0,99");
            //return;
            RunScenario("part1", @"1,0,0,3,1,1,2,3,1,3,4,3,1,5,0,3,2,13,1,19,1,6,19,23,2,6,23,27,1,5,27,31,2,31,9,35,1,35,5,39,1,39,5,43,1,43,10,47,2,6,47,51,1,51,5,55,2,55,6,59,1,5,59,63,2,63,6,67,1,5,67,71,1,71,6,75,2,75,10,79,1,79,5,83,2,83,6,87,1,87,5,91,2,9,91,95,1,95,6,99,2,9,99,103,2,9,103,107,1,5,107,111,1,111,5,115,1,115,13,119,1,13,119,123,2,6,123,127,1,5,127,131,1,9,131,135,1,135,9,139,2,139,6,143,1,143,5,147,2,147,6,151,1,5,151,155,2,6,155,159,1,159,2,163,1,9,163,0,99,2,0,14,0");

        }
    }
}
