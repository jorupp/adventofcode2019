using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace AoC.Year2018.Day6
{
    public class Part2 : BasePart
    {
        protected void RunScenario(string title, string input)
        {
            RunScenario(title, () =>
            {
                var lines = input.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                var data = lines.Select(i => i.Numbers()).ToArray();
                var d = 10000;

                // not 46054
                var start = 0;
                var end = 400;
                var q = (from x in Enumerable.Range(start, end - start)
                        from y in Enumerable.Range(start, end - start)
                        select new {x, y})
                    //.AsParallel()
                    .Select(i => new { i.x, i.y, d = SumDistance(new[] {i.x, i.y}, data) })
                    .OrderBy(i => i.x).ThenBy(i => i.y)
                    .ToArray();

                var sb = new StringBuilder();
                for (var x = start; x < end; x++)
                {
                    for (var y = start; y < end; y++)
                    {
                        if (data.Any(i => i[0] == x && i[1] == y))
                        {
                            sb.Append("X");
                        }
                        else
                        {
                            sb.Append(q[x + y * (end - start)].d < d ? "." : " ");
                        }
                    }
                    sb.AppendLine();
                }

                var output = sb.ToString();

                Console.WriteLine(q.Count(i => i.d < d));
            });
        }

        private long SumDistance(int[] target, int[][] sources)
        {
            return sources.Select(i => Distance(target, i)).Sum();
        }

        private long Distance(int[] c1, int[] c2)
        {
            return Math.Abs(c1[0] - c2[0]) + Math.Abs(c1[1] - c2[1]);
        }

        public override void Run()
        {
//            RunScenario("initial", @"1, 1
//1, 6
//8, 3
//3, 4
//5, 5
//8, 9");
            //return;
            RunScenario("part1", @"181, 184
230, 153
215, 179
84, 274
294, 274
127, 259
207, 296
76, 54
187, 53
318, 307
213, 101
111, 71
310, 295
40, 140
176, 265
98, 261
315, 234
106, 57
40, 188
132, 292
132, 312
97, 334
292, 293
124, 65
224, 322
257, 162
266, 261
116, 122
80, 319
271, 326
278, 231
191, 115
277, 184
329, 351
58, 155
193, 147
45, 68
310, 237
171, 132
234, 152
158, 189
212, 100
346, 225
257, 159
330, 112
204, 320
199, 348
207, 189
130, 289
264, 223");

        }
    }
}
