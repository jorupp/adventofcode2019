using System;
using System.Linq;
using System.Security.Cryptography;

namespace AoC.Year2018.Day6
{
    public class Part1 : BasePart
    {
        protected void RunScenario(string title, string input)
        {
            RunScenario(title, () =>
            {
                var lines = input.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                var data = lines.Select(i => i.Numbers()).ToArray();

                var q = (from x in Enumerable.Range(-200, 900)
                        from y in Enumerable.Range(-200, 900)
                        select new {x, y})
                    .AsParallel()
                    .Select(i => new
                    {
                        r = Closest(new[] {i.x, i.y}, data),
                        isFar = i.x < -100 || i.x > 600 || i.y < -100 || i.y > 600
                    })
                    .Where(i => i.r.HasValue)
                    .ToArray();

                var wholeRange = q.GroupBy(i => i.r).Select(i => new { i.Key, Count = i.Count() }).ToArray();
                var innerRange = q.Where(i => !i.isFar).GroupBy(i => i.r).Select(i => new { i.Key, Count = i.Count() }).ToArray();

                var q1 = (from w in wholeRange
                    join i in innerRange on w.Key equals i.Key
                    where w.Count == i.Count
                    select new { i.Key, i.Count, WholeCount = i.Count }).OrderByDescending(i => i.Count).ToArray();
                var result = q1.First();

                Console.WriteLine(result.Count);
            });
        }

        private int? Closest(int[] target, int[][] sources)
        {
            var ds = sources.Select((i, ix) => new {ix, d = Distance(target, i)}).OrderBy(i => i.d).ToList();
            if (ds[0].d == ds[1].d)
            {
                return null;
            }

            return ds[0].ix;
        }

        private int Distance(int[] c1, int[] c2)
        {
            return Math.Abs(c1[0] - c2[0]) + Math.Abs(c1[1] - c2[1]);
        }

        public override void Run()
        {
            RunScenario("initial", @"1, 1
1, 6
8, 3
3, 4
5, 5
8, 9");
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
