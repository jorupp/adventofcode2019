using System;
using System.Linq;

namespace AoC.Year2020.Day13
{
    public class Part2 : BasePart
    {
        protected void RunScenario(string title, string input)
        {
            RunScenario(title, () =>
            {
                var lines = input.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                var busses = lines[0].Split(',').Select(i => i == "x" ? (long?)null : long.Parse(i));

                var withFormulas = busses.Select((i, ix) =>
                {
                    if (i == null)
                        return null;
                    var v = i.Value;
                    
                    // t % v is the position of our bus at time t
                    // we need this bus to arrive ix minutes later than t, so (t + ix) % v will be 0
                    Console.WriteLine($"(t + {ix}) mod {v} = 0");
                    return new { increment = v, formula = (Func<long, bool>) (t => (t + ix) % v == 0)};
                }).Where(i => i != null).ToList();

                // we solve this problem incrementally.
                // if we only had bus 0, time 0 would solve the problem.
                // since that obviously won't work, we need a higher number, but whatever it is,
                //    it must be value + N * lcm
                // where lcm is the least common multiple of the bus numbers so far
                long value = 0;
                long lcm = withFormulas[0].increment;
                var trial = 0;

                foreach (var withFormula in withFormulas.Skip(1))
                {
                    Console.WriteLine($"LCM: {lcm}");

                    // all the busses we've solved up to this point will work with a solution of "value"
                    // but they will also work for any value + N * LCM, where N can be any integer
                    // so we repeatedly increment value by LCM looking for a value that works for the formula for this new bus
                    while (!withFormula.formula(value))
                    {
                        value += lcm;
                        trial++;
                    }

                    // value works for this bus.  to make sure our next solutions work for this bus, we need to merge this bus's increment with the LCM we have already
                    // ie. if we solve for bus 25, then solve for bus 15, when we solve for the next bus, we need to increment by LCM(25,15)
                    //   if we incremented by 25*15 will miss some viable options
                    lcm = determineLCM(withFormula.increment, lcm);
                }

                Console.WriteLine(value);
                Console.WriteLine($"trials: {trial}");

            });
        }

        public static long determineLCM(long a, long b)
        {
            long num1, num2;
            if (a > b)
            {
                num1 = a; num2 = b;
            }
            else
            {
                num1 = b; num2 = a;
            }

            for (long i = 1; i < num2; i++)
            {
                long mult = num1 * i;
                if (mult % num2 == 0)
                {
                    return mult;
                }
            }
            return num1 * num2;
        }


        public override void Run()
        {
            RunScenario("initial", @"7,13,x,x,59,x,31,19");
            RunScenario("initial", @"17,x,13,19");
            RunScenario("initial", @"67,7,59,61");
            RunScenario("initial", @"67,x,7,59,61");
            RunScenario("initial", @"67,7,x,59,61");
            RunScenario("initial", @"1789,37,47,1889");
            //return;
            RunScenario("part1", @"23,x,x,x,x,x,x,x,x,x,x,x,x,41,x,x,x,x,x,x,x,x,x,383,x,x,x,x,x,x,x,x,x,x,x,x,13,17,x,x,x,x,19,x,x,x,x,x,x,x,x,x,29,x,503,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,37");

        }
    }
}