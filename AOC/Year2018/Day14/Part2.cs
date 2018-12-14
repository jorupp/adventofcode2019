using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC.Year2018.Day14
{
    public class Part2 : BasePart
    {
        protected void RunScenario(string title, string input)
        {
            RunScenario(title, () =>
            {
                var lines = input.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                var toFind = lines.Select(i => i.Select(ii => int.Parse(ii.ToString())).ToArray()).ToList();

                var elves = new int[] { 0, 1 };
                var recipies = new List<int>() { 3, 7 };

                while (toFind.Count > 0)
                {
                    var result = recipies[elves[0]] + recipies[elves[1]];
                    if (result >= 10)
                    {
                        recipies.Add(result / 10);
                        Find(toFind, recipies);
                    }
                    recipies.Add(result % 10);
                    Find(toFind, recipies);
                    elves[0] = (elves[0] + recipies[elves[0]] + 1) % recipies.Count;
                    elves[1] = (elves[1] + recipies[elves[1]] + 1) % recipies.Count;
                }
            });
        }

        private void Find(List<int[]> toFind, List<int> recipies)
        {
            var count = recipies.Count;
            foreach(var find in toFind)
            {
                if (find.Length > count)
                {
                    continue;
                }
                for (var i = 0; i < find.Length; i++)
                {
                    if (find[find.Length - 1 - i] != recipies[count - i - 1])
                    {
                        goto cont;
                    }
                }
                Console.WriteLine(count - find.Length);
                toFind.Remove(find);
                return;

                cont:
                continue;
            }
        }

        public override void Run()
        {
            RunScenario("part1", @"51589
01245
92510
59414
890691");
            //return;
            //RunScenario("part1", @"ff2f323f");

        }
    }
}
