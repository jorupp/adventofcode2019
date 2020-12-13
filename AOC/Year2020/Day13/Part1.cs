using System;
using System.Linq;

namespace AoC.Year2020.Day13
{
    public class Part1 : BasePart
    {
        protected void RunScenario(string title, string input)
        {
            RunScenario(title, () =>
            {
                var lines = input.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                var start = int.Parse(lines[0]);
                var busses = lines[1].Split(',').Select(i => i == "x" ? (int?)null : int.Parse(i));

                var times = busses.Select(i =>
                {
                    if (i == null)
                    {
                        return null;
                    }

                    var early = start % i.Value;
                    if (early == 0)
                    {
                        return new
                        {
                            bus = i.Value,
                            wait = 0,
                        };
                    }

                    return new
                    {
                        bus = i.Value,
                        wait = i.Value - early,
                    };
                }).ToList();

                var x = times.Where(i => i != null).OrderBy(i => i.wait).First();

                Console.WriteLine($"{start}, {x.bus}, {x.wait}");
                Console.WriteLine(x.bus * x.wait);
            });
        }

        public override void Run()
        {
            RunScenario("initial", @"939
7,13,x,x,59,x,31,19");
            //return;
            RunScenario("part1", @"1000390
23,x,x,x,x,x,x,x,x,x,x,x,x,41,x,x,x,x,x,x,x,x,x,383,x,x,x,x,x,x,x,x,x,x,x,x,13,17,x,x,x,x,19,x,x,x,x,x,x,x,x,x,29,x,503,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,37");

        }
    }
}
