using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AoC.Year2018.Day12
{
    public class Part2 : BasePart
    {
        protected void RunScenario(string title, string input)
        {
            RunScenario(title, () =>
            {
                var lines = input.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

                var stateRe = new Regex("[#.]");
                var initialState = stateRe.Matches(lines[0]).Select(i => i.Groups[0].Value).ToArray();
                var rawRules = lines.Skip(1).Select(l => stateRe.Matches(l).Select(i => i.Groups[0].Value).ToArray()).ToArray();
                var rules = rawRules.ToDictionary(i => i.Take(5).Select((ii, ix) => ii == "#" ? 1 << ix : 0).Sum(), i => i[5] == "#");
                var state = initialState.Select((i, ix) => new { i, ix }).ToDictionary(i => (long)i.ix, i => i.i == "#");

                var seenKeys = new Dictionary<string, (long, long)>();
                var showMore = 10;
                for (long i = 0; i < 50000000000; i++)
                {
                    var key = GetKey(state);
                    var start = state.Where(ii => ii.Value).Min(ii => ii.Key);
                    if (!seenKeys.ContainsKey(key))
                    {
                        seenKeys.Add(key, (i, start));
                    }
                    else
                    {
                        Console.WriteLine($"{i}, {start}, {seenKeys[key]}, {key}, {state.Where(ii => ii.Value).Sum(ii => ii.Key)}");
                        showMore--;
                        if (showMore == 0)
                        {
                            return;
                        }
                    }

                    state = Mutate(state, rules);
                }

                var result = state.Where(i => i.Value).Sum(i => i.Key);

                Console.WriteLine(result);
            });
        }

        private Dictionary<long, bool> Mutate(Dictionary<long, bool> originalState, Dictionary<int, bool> rules)
        {
            var newState = new Dictionary<long, bool>();
            var min = originalState.Keys.Min() - 2;
            var max = originalState.Keys.Max() + 2;
            for (long i = min; i <= max; i++)
            {
                var rulePattern =
                    ((originalState.TryGetValue(i - 2, out var v1) ? v1 : false) ? 1 : 0) +
                    ((originalState.TryGetValue(i - 1, out var v2) ? v2 : false) ? 2 : 0) +
                    ((originalState.TryGetValue(i, out var v3) ? v3 : false) ? 4 : 0) +
                    ((originalState.TryGetValue(i + 1, out var v4) ? v4 : false) ? 8 : 0) +
                    ((originalState.TryGetValue(i + 2, out var v5) ? v5 : false) ? 16 : 0);
                var output = rules.TryGetValue(rulePattern, out var rp) ? rp : false;
                newState[i] = output;
            }

            return newState;
        }

        private string GetKey(Dictionary<long, bool> state)
        {
            return string.Join("", state.OrderBy(i => i.Key).Select(i => i.Value ? "#" : ".")).Trim('.');
        }

        public override void Run()
        {
            RunScenario("initial", @"initial state: #..#.#..##......###...###

...## => #
..#.. => #
.#... => #
.#.#. => #
.#.## => #
.##.. => #
.#### => #
#.#.# => #
#.### => #
##.#. => #
##.## => #
###.. => #
###.# => #
####. => #
");
            //return;
            RunScenario("part1", @"initial state: #.#####.##.###...#...#.####..#..#.#....##.###.##...#####.#..##.#..##..#..#.#.#.#....#.####....#..#

#.#.. => .
..### => .
...## => .
.#### => #
.###. => #
#.... => .
#.#.# => .
###.. => #
#..#. => .
##### => #
.##.# => #
.#... => .
##.## => #
#...# => #
.#.## => .
##..# => .
..... => .
.#.#. => #
#.### => #
....# => .
...#. => #
..#.# => #
##... => #
####. => #
#..## => #
##.#. => #
###.# => .
#.##. => .
..#.. => #
.#..# => .
..##. => .
.##.. => #");

        }
    }
}
