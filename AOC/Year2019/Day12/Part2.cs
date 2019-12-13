using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC.Year2019.Day12
{
    public class Part2 : BasePart
    {
        protected void RunScenario(string title, int[][] position)
        {
            RunScenario(title, () =>
            {
                var parts = Enumerable.Range(0, 3)
                    .Select(ii => SimulateDimension(position.Select(i => i[ii]).ToArray())).ToList();

                var lcm = parts.Aggregate((a, i) => determineLCM(a, i));
                Console.WriteLine($"Repeats in {lcm} steps");
            });
        }

        private static long SimulateDimension(int[] position)
        {
            var moons = position.Select(i => new Moon(i, 0)).ToList();
            var seen = new Dictionary<State, long>();

            seen[new State(moons)] = 0;

            long step = 1;
            for (; ; step++)
            {
                moons[0].ApplyGravity(moons[1]);
                moons[0].ApplyGravity(moons[2]);
                moons[0].ApplyGravity(moons[3]);
                moons[1].ApplyGravity(moons[2]);
                moons[1].ApplyGravity(moons[3]);
                moons[2].ApplyGravity(moons[3]);

                moons[0].ApplyVelocity();
                moons[1].ApplyVelocity();
                moons[2].ApplyVelocity();
                moons[3].ApplyVelocity();

                var state = new State(moons);
                if (seen.ContainsKey(state))
                {
                    break;
                }

                seen[state] = step;
            }

            Console.WriteLine($"After {step} steps, found a loop back to {seen[new State(moons)]}");
            Console.WriteLine();
            return step;
        }

        // https://stackoverflow.com/a/13569863/12502
        public static long determineLCM(long a, long b)
        {
            if (a < b) return determineLCM(b, a);

            for (long i = 1; i < b; i++)
            {
                if ((a * i) % b == 0)
                {
                    return i * a;
                }
            }
            return a * b;
        }


        private class State
        {
            private List<int> values;

            public State(ICollection<Moon> moons)
            {
                this.values = moons.SelectMany(i => new [] { i.p, i.v }).ToList();

            }

            public override bool Equals(object obj)
            {
                var s = (State) obj;
                return this.values.Zip(s.values, (i1, i2) => i1 == i2).All(i => i);
            }

            public override int GetHashCode()
            {
                return this.values.Aggregate((a, i) => a ^ i.GetHashCode());
            }
        }

        private class Moon
        {
            public int p;
            public int v;

            public Moon(int p, int v)
            {
                this.p = p;
                this.v = v;
            }

            public void ApplyGravity(Moon other)
            {
                if (other.p > this.p)
                {
                    other.v--;
                    this.v++;
                }
                else if (other.p < this.p)
                {
                    other.v++;
                    this.v--;
                }
            }

            public void ApplyVelocity()
            {
                    this.p += this.v;
            }


            //public override string ToString()
            //{
            //    return $"pos=<x={p[0]}, y={p[1]}, z={p[2]}>, vel=<x={v[0]},y={v[1]},z={v[2]}>";
            //}

            //public int Energy => p.Sum(Math.Abs) * v.Sum(Math.Abs);
        }

        public override void Run()
        {
            RunScenario("initial", new[]
            {
                new [] { -1, 0, 2},
                new [] { 2, -10, -7},
                new [] { 4, -8, 8},
                new [] { 3, 5, -1},
            });
            RunScenario("initial", new[]
            {
                new [] { -8, -10, 0},
                new [] { 5, 5, 10},
                new [] { 2, -7, 3},
                new [] { 9, -8, -3},
            });
            //return;
            RunScenario("real", new[]
            {
                new [] { 1, -4, 3 },
                new [] { -14, 9, -4},
                new [] { -4, -6, 7 },
                new [] { 6, -9, -11},
            });

        }
    }
}
