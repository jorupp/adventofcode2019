using System;
using System.Linq;

namespace AoC.Year2019.Day4
{
    public class Part2 : BasePart
    {
        protected void RunScenario(string title, string input)
        {
            RunScenario(title, () =>
            {
                var lines = input.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                var range = lines[0].Split('-').Select(int.Parse).ToList();
                var min = range[0];
                var max = range[1];
                var count = 0;
                for (var i = min; i <= max; i++)
                {
                    if (IsValid(i))
                        count++;

                }

                Console.WriteLine(count);
            });
        }

        private bool IsValid(int input)
        {
            var v = input.ToString();
            var same = v.Take(v.Length - 1).Select((i, ix) =>
            {
                if (ix != 0)
                {
                    if (i == v[ix - 1])
                    {
                        return false;
                    }
                }

                if (i != v[ix + 1])
                {
                    return false;
                }

                if (ix != v.Length - 2)
                {
                    if (i == v[ix + 2])
                    {
                        return false;
                    }
                }

                return true;
            }).Any(ii => ii);
            //Console.WriteLine(same);
            if (!same) return false;
            var increase = new string(v.OrderBy(ii => ii).ToArray()) == v;
            //Console.WriteLine(increase);
            return increase;
        }

        public override void Run()
        {
            RunScenario("112233", @"112233-112233");
            RunScenario("123444", @"123444-123444");
            RunScenario("111122", @"111122-111122");
            //return;
            RunScenario("part1", @"256310-732736");

        }
    }
}