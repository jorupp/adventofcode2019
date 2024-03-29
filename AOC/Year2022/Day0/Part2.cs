﻿using System;
using System.Linq;
using AoC;

namespace AOC.Year2022.Day0
{
    public class Part2 : BasePart
    {
        protected void RunScenario(string title, string input)
        {
            RunScenario(title, () =>
            {
                var lines = input.Replace("\r\n", "\n").Split("\n", StringSplitOptions.RemoveEmptyEntries);

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
