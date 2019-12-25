using System;
using System.Collections.Generic;
using System.Linq;
using AOC.Year2019.Day15;

namespace AoC.Year2019.Day25
{
    public class Part1 : BasePart
    {
        protected void RunScenario(string title, string input)
        {
            RunScenario(title, () =>
            {
                var lines = input.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                //var initialData = lines[0].Split(',').Select(long.Parse).ToArray();
                //var data = initialData.Select((i, ii) => new { i, ii }).ToDictionary(i => i.ii, i => i.i);
                //var start = IntCodeSimulationState.Start(data);

                //var state = new Dictionary<(int, int), bool>().WithDefault(false);

                //for (var y = 0; y < lines.Length; y++)
                //{
                //    for (var x = 0; x < lines[0].Length; x++)
                //    {
                //        state[(x, y)] = lines[y][x] == '#';
                //    }
                //}

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
