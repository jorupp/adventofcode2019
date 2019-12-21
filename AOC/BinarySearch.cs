using System;
using System.Collections.Generic;
using System.Text;

namespace AOC
{
    public static class BinarySearch
    {
        /// <summary>
        /// Finds the smallest value where <see cref="test"/> returns true.  All values must be positive.
        /// </summary>
        /// <param name="test">The test value - this *must* be continuous, or this algorithm can get stuck in a local false -> true transition</param>
        /// <param name="guess">initial guess to start at</param>
        /// <param name="min">minimum value - <see cref="test"/> must return false for this</param>
        /// <param name="max">maximum value - <see cref="test"/> must return true for this</param>
        /// <returns></returns>
        public static long GetMin(Func<long, bool> test, long guess = 0, long min = 0, long max = long.MaxValue)
        {
            //if (!test(max))
            //{
            //    throw new ArgumentException("test much return true", nameof(max));
            //}
            //if (test(min))
            //{
            //    throw new ArgumentException("test much return false", nameof(min));
            //}
            var currentGuess = guess;
            var currentMin = min;
            var currentMax = max;
            while (true)
            {
                if (currentMin + 1 == currentMax)
                {
                    return currentMax;
                }
                //Console.WriteLine($"Testing {currentGuess}");
                if (test(currentGuess))
                {
                    currentMax = currentGuess;
                    if (currentMin == min)
                    {
                        currentGuess = Math.Max(currentGuess / 2, min);
                        //Console.WriteLine($"New guess A {currentGuess} - {min}");
                    }
                    else
                    {
                        currentGuess -= Math.Max(1, (currentGuess - currentMin) / 2);
                        //Console.WriteLine($"New guess B {currentGuess} - {currentMin}");
                    }
                }
                else
                {
                    currentMin = currentGuess;
                    if (currentMax == max)
                    {
                        currentGuess = Math.Min(Math.Max(currentGuess * 2, 1), max);
                        //Console.WriteLine($"New guess C {currentGuess} - {max}");
                    }
                    else
                    {
                        currentGuess += Math.Max(1, (currentMax - currentGuess) / 2);
                        //Console.WriteLine($"New guess D {currentGuess} - {currentMax}");
                    }
                }
            }
        }

        /// <summary>
        /// Finds the largest value where <see cref="test"/> returns true.  All values must be positive.
        /// </summary>
        /// <param name="test">The test value - this *must* be continuous, or this algorithm can get stuck in a local false -> true transition</param>
        /// <param name="guess">initial guess to start at</param>
        /// <param name="min">minimum value - <see cref="test"/> must return true for this</param>
        /// <param name="max">maximum value - <see cref="test"/> must return false for this</param>
        /// <returns></returns>
        public static long GetMax(Func<long, bool> test, long guess = 0, long min = 0, long max = long.MaxValue)
        {
            return GetMin(i => !test(i), guess, min, max) - 1;
        }
    }
}
