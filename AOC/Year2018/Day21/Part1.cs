using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;

namespace AoC.Year2018.Day21
{
    public class Part1 : BasePart
    {
        protected void RunScenario(string title, string input)
        {
            RunScenario(title, () =>
            {
                var lines = input.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

                Console.WriteLine(TryIt(0));
            });
        }

        private long TryIt(int r0)
        {
            var possibles = new Dictionary<int, long>();
            int r1 = 0, r2 = 0, r3 = 0, r4 = 0, r5 = 0, r6 = 0;
            long i = 0;

            s_6: i++; r1 = r4 | 0x10000;
            s_7: i++; r4 = 678134;
            s_8: i++; r5 = r1 & 0xff;
            s_9: i++; r4 += r1 & 0xff;
            s_10: i++; r4 &= 0xffffff;
            s_11: i++; r4 *= 65899;
            s_12: i++; r4 &= 0xffffff;
            s_13: i++;
            if (r1 < 0x100)
            {
                i += 2;
                goto s_28;
            }
            else
            {
                i += 2;
            }
            //Console.WriteLine($"A R4 is {r4}");
            s_17: i++; r5 = 0;
            s_18: i++; r2 = r5 + 1;
            s_19: i++; r2 *= 256;
            s_20: i++;
            //Console.WriteLine($"B R2/R1 is {r2} - {r1}");
            if (r2 > r1)
            {
                i += 2;
                s_26: i++; r1 = r5;
                s_27: i++; goto s_8;
            }
            else
            {
                i += 2;
                s_24: i++; r5++;
                s_25: i++; goto s_18;
            }

            s_28: i++;
            //Console.WriteLine($"R4 is {r4} - comparing to {r0}");
            if (!possibles.ContainsKey(r4))
            {
                possibles.Add(r4, i);
                Console.WriteLine($"Could terminate at {i} with {r4}");
            }
            if (r4 == r0)
            {
                i++;
                return i;
            }
            else
            {
                i += 2;
                goto s_6;
            }
        }

        public override void Run()
        {
            RunScenario("initial", @"asdfasdf");
            ////return;
            //RunScenario("part1", @"ff2f323f");

        }
    }
}
