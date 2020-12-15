using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC.Year2020.Day15
{
    public class Part1 : BasePart
    {
        protected void RunScenario(string title, string input)
        {
            RunScenario(title, () =>
            {
                var lines = input.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToList();
                var memory = new Dictionary<long, long>();

                long last = 0;
                for(var i=0; i <2020; i++)
                {
                    long newOne = 0;
                    if (i < lines.Count)
                    {
                        newOne = lines[i];
                    }
                    else
                    {
                        if (memory.TryGetValue(last, out var v))
                        {
                            //Console.WriteLine($"have seen before: {i} - {v}");
                            newOne = i - v;
                        }
                        else
                        {
                            newOne = 0;
                        }
                    }

                    //Console.WriteLine($"Turn {i}: said {newOne}");
                    if (i > 0)
                    {
                        //Console.WriteLine($"Recorded {last} as {i}");
                        memory[last] = i;
                    }
                    last = newOne;
                }

                Console.WriteLine(last);
            });
        }

        public override void Run()
        {
            RunScenario("initial", @"0,3,6");
            RunScenario("initial", @"1,3,2");
            //return;
            RunScenario("initial", @"2,1,3");
            RunScenario("initial", @"1,2,3");
            RunScenario("initial", @"2,3,1");
            RunScenario("initial", @"3,2,1");
            RunScenario("initial", @"3,1,2");
            //return;
            RunScenario("part1", @"14,1,17,0,3,20");

        }
    }
}
