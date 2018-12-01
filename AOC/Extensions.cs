using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    }
}
