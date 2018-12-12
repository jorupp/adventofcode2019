using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AoC.Year2018.Day12
{
    public class Part1 : BasePart
    {
        protected void RunScenario(string title, string input)
        {
            RunScenario(title, () =>
            {
                var lines = input.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

                var baseLine = 50;
                var stateRe = new Regex("[#.]");
                var initialState = stateRe.Matches(lines[0]).Select(i => i.Groups[0].Value).ToArray();
                var rawRules = lines.Skip(1).Select(l => stateRe.Matches(l).Select(i => i.Groups[0].Value).ToArray()).ToArray();
                var rules = rawRules.ToDictionary(i => i.Take(5).Select((ii, ix) =>  ii == "#" ? 1 << ix : 0).Sum(), i => i[5] == "#");
                var state = initialState.Select((i, ix) => new {i, ix}).ToDictionary(i => i.ix, i => i.i == "#");

                for (var i = 0; i < 20; i++)
                {
                    state = Mutate(state, rules);
                }

                var result = state.Where(i => i.Value).Sum(i => i.Key);

                Console.WriteLine(result);
            });
        }

        private Dictionary<int, bool> Mutate(Dictionary<int, bool> originalState, Dictionary<int, bool> rules)
        {
            var newState = new Dictionary<int, bool>();
            var min = originalState.Keys.Min() - 2;
            var max = originalState.Keys.Max() + 2;
            for (var i = min; i <= max; i++)
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
