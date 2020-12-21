using System;
using System.Linq;

namespace AoC.Year2020.Day22
{
    public class Part1 : BasePart
    {
        protected void RunScenario(string title, string input)
        {
            RunScenario(title, () =>
            {
                var lines = input.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

                Console.WriteLine(lines.Length);
            });
        }

        public override void Run()
        {
            RunScenario("initial", @"asdfasdf");
            //return;
            RunScenario("part1", @"ff2f323f");

        }
    }
}
