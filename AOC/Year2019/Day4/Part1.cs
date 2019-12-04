using System;
using System.Linq;

namespace AoC.Year2019.Day4
{
    public class Part1 : BasePart
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

        private bool IsValid(int i)
        {
            var v = i.ToString();
            var same = v.Zip(v.Skip(1), (i1, i2) => i1 == i2).Any(ii => ii);
            //Console.WriteLine(same);
            if (!same) return false;
            var increase = new string(v.OrderBy(ii => ii).ToArray()) == v;
            //Console.WriteLine(increase);
            return increase;
        }

        public override void Run()
        {
            RunScenario("111111", @"111111-111111");
            RunScenario("223450", @"223450-223450");
            RunScenario("123789", @"123789-123789");
            RunScenario("111123", @"111123-111123");
            RunScenario("135679", @"135679-135679");
            RunScenario("122345", @"122345-122345");
            //return;
            RunScenario("part1", @"256310-732736");

        }
    }
}
