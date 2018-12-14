using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC.Year2018.Day14
{
    public class Part1 : BasePart
    {
        protected void RunScenario(string title, string input)
        {
            RunScenario(title, () =>
            {
                var lines = input.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                var outputOn = lines.Select(i => i.Numbers().Single()).ToArray();
                var end = outputOn.Max();

                var elves = new int[] {0, 1};
                var recipies = new List<int>() {3, 7};

                while(recipies.Count < end + 10)
                {
                    var result = recipies[elves[0]] + recipies[elves[1]];
                    if (result >= 10)
                    {
                        recipies.Add(result / 10);
                    }
                    recipies.Add(result % 10);
                    elves[0] = (elves[0] + recipies[elves[0]] + 1) % recipies.Count;
                    elves[1] = (elves[1] + recipies[elves[1]] + 1) % recipies.Count;
                }

                foreach (var output in outputOn)
                {
                    Console.WriteLine(string.Join("", recipies.Skip(output).Take(10)));
                }
            });
        }

        public override void Run()
        {
            RunScenario("part1", @"9
5
18
2018
890691");
            //return;
            //RunScenario("part1", @"ff2f323f");

        }
    }
}
