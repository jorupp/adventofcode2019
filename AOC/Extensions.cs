using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AoC
{
    public static class Extensions
    {
        public static IEnumerable<T> SelectDeep<T>(this T thing, Func<T, IEnumerable<T>> selector)
        {
            yield return thing;
            foreach (var t in selector(thing))
            {
                foreach (var d in SelectDeep(t, selector))
                {
                    yield return d;
                }
            }
        }

        private static Regex NumberRegex = new Regex(@"(\d+)", RegexOptions.Compiled);
        public static int[] Numbers(this string input)
        {
            return NumberRegex.Matches(input).Select(i => i.Captures[0].Value).Select(int.Parse).ToArray();
        }

        public static int[][] NumbersLines(this string input)
        {
            var lines = input.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            return lines.Select(i => i.Numbers()).ToArray();
        }

        private static Regex AlphaNumericRegex = new Regex(@"([0-9A-Za-z]+)", RegexOptions.Compiled);
        public static string[] AlphaNumeric(this string input)
        {
            return AlphaNumericRegex.Matches(input).Select(i => i.Captures[0].Value).ToArray();
        }

        public static string[][] AlphaNumericLines(this string input)
        {
            var lines = input.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            return lines.Select(i => i.AlphaNumeric()).ToArray();
        }
    }
}
