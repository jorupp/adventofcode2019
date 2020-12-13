using System;
using System.Linq;

namespace AoC.Year2020.Day13
{
    public class Part2 : BasePart
    {
        protected void RunScenario(string title, string input, long guess)
        {
            RunScenario(title, () =>
            {
                var lines = input.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                var busses = lines[0].Split(',').Select(i => i == "x" ? (long?)null : long.Parse(i));

                var x = busses.Select((i, ix) =>
                {
                    if (i == null) return null; 
                    return $"t mod {i} = {(i - ix + i) % i}";
                }).ToList();

                foreach (var xx in x)
                {
                    if (null == xx)
                        continue;
                    Console.WriteLine(xx);
                }

                var formulas = busses.Select((i, ix) =>
                {
                    if (i == null)
                        return null;
                    return (Func<long, bool>) (t => t % i == (i - ix + i) % i);
                }).Where(i => i != null).ToList();

                var rnd = new Random();

                long min = guess / 2;
                long max = guess * 2;
                long mod = max - min;
                long baseMod = busses.First().Value;

                long tries = 0;
                while (true)
                {
                    tries++;

                    var g = (rnd.Next() * rnd.Next()) % mod + min;
                    if (formulas.All(i => i(g)))
                    {
                        Console.WriteLine(g);
                        return;
                    }

                    if (tries % 1000000 == 0)
                    {
                        Console.WriteLine($"  {tries} tries");
                    }
                }


            });
        }

        public override void Run()
        {
            RunScenario("initial", @"7,13,x,x,59,x,31,19", 1068788);
            RunScenario("initial", @"17,x,13,19", 3500);
            RunScenario("initial", @"67,7,59,61", 700000);
            RunScenario("initial", @"67,x,7,59,61", 700000);
            RunScenario("initial", @"67,7,x,59,61", 1200000);
            RunScenario("initial", @"1789,37,47,1889", 1200000000);
            //return;
            RunScenario("part1", @"23,x,x,x,x,x,x,x,x,x,x,x,x,41,x,x,x,x,x,x,x,x,x,383,x,x,x,x,x,x,x,x,x,x,x,x,13,17,x,x,x,x,19,x,x,x,x,x,x,x,x,x,29,x,503,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,37", 150000000000000);

        }
    }
}