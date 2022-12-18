using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOC.Utilities
{
    public static class TupleExtensions
    {
        public static ValueTuple<int, int, int> Add(this ValueTuple<int, int, int> v1, ValueTuple<int, int, int> v2)
        {
            return new ValueTuple<int, int, int>(v1.Item1 + v2.Item1, v1.Item2 + v2.Item2, v1.Item3 + v2.Item3);
        }
        public static ValueTuple<int, int, int> Subtract(this ValueTuple<int, int, int> v1, ValueTuple<int, int, int> v2)
        {
            return new ValueTuple<int, int, int>(v1.Item1 - v2.Item1, v1.Item2 - v2.Item2, v1.Item3 - v2.Item3);
        }

        public static ValueTuple<int, int, int> Abs(this ValueTuple<int, int, int> v)
        {
            return new ValueTuple<int, int, int>(Math.Abs(v.Item1), Math.Abs(v.Item2), Math.Abs(v.Item3));
        }
        public static ValueTuple<int, int, int> Sign(this ValueTuple<int, int, int> v)
        {
            return new ValueTuple<int, int, int>(Math.Sign(v.Item1), Math.Sign(v.Item2), Math.Abs(v.Item3));
        }
        public static ValueTuple<int, int, int> MoveOneCloserPreferDiagonal(this ValueTuple<int, int, int> v1, ValueTuple<int, int, int> v2)
        {
            return v1.Add(v2.Subtract(v1).Sign());
        }

        public static T Get<T>(this T[][][] map, ValueTuple<int, int, int> coord)
        {
            return map[coord.Item3][coord.Item2][coord.Item1];
        }

        public static void Set<T>(this T[][][] map, ValueTuple<int, int, int> coord, T value)
        {
            map[coord.Item3][coord.Item2][coord.Item1] = value;
        }
    }
}
