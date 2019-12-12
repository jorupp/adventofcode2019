using System;
using System.Linq;

namespace AoC.Year2019.Day12
{
    public class Part2 : BasePart
    {
        protected void RunScenario(string title, int[][] position, int stepCount)
        {
            RunScenario(title, () =>
            {
                var moons = position.Select(i => new Moon(i, new[] { 0, 0, 0 })).ToList();

                Console.WriteLine($"After {0} steps");
                foreach (var moon in moons)
                {
                    Console.WriteLine(moon.ToString());
                }
                Console.WriteLine($"Energy: {moons.Sum(i => i.Energy)}");
                Console.WriteLine();
                for (var step = 1; step <= stepCount; step++)
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

                    //for (var i = 0; i < moons.Count; i++)
                    //{
                    //    for (var j = i + 1; j < moons.Count; j++)
                    //    {
                    //        moons[i].ApplyGravity(moons[j]);
                    //    }
                    //}
                    //for (var i = 0; i < moons.Count; i++)
                    //{
                    //    moons[i].ApplyVelocity();
                    //}

                }

                Console.WriteLine($"After {stepCount} steps");
                foreach (var moon in moons)
                {
                    Console.WriteLine(moon.ToString());
                }
                Console.WriteLine($"Energy: {moons.Sum(i => i.Energy)}");
                Console.WriteLine();
            });
        }

        private class Moon
        {
            private int[] p;
            private int[] v;

            public Moon(int[] p, int[] v)
            {
                this.p = p;
                this.v = v;
            }

            public void ApplyGravity(Moon other)
            {
                for (var i = 0; i < this.p.Length; i++)
                {
                    if (other.p[i] > this.p[i])
                    {
                        other.v[i]--;
                        this.v[i]++;
                    }
                    else if (other.p[i] < this.p[i])
                    {
                        other.v[i]++;
                        this.v[i]--;
                    }
                }
            }

            public void ApplyVelocity()
            {
                for (var i = 0; i < this.p.Length; i++)
                {
                    this.p[i] += this.v[i];
                }
            }

            public override string ToString()
            {
                return $"pos=<x={p[0]}, y={p[1]}, z={p[2]}>, vel=<x={v[0]},y={v[1]},z={v[2]}>";
            }

            public int Energy => p.Sum(Math.Abs) * v.Sum(Math.Abs);
        }

        public override void Run()
        {
            RunScenario("initial", new[]
            {
                new [] { -1, 0, 2},
                new [] { 2, -10, -7},
                new [] { 4, -8, 8},
                new [] { 3, 5, -1},
            }, 4000000);
            RunScenario("initial", new[]
            {
                new [] { -8, -10, 0},
                new [] { 5, 5, 10},
                new [] { 2, -7, 3},
                new [] { 9, -8, -3},
            }, 4000000);
            //return;
            RunScenario("real", new[]
            {
                new [] { 1, -4, 3 },
                new [] { -14, 9, -4},
                new [] { -4, -6, 7 },
                new [] { 6, -9, -11},
            }, 4000000);

        }
    }
}
