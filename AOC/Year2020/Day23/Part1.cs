using System;
using System.Linq;

namespace AoC.Year2020.Day23
{
    public class Part1 : BasePart
    {
        protected void RunScenario(string title, string input, int count)
        {
            RunScenario(title, () =>
            {
                var data = input.Select(i => int.Parse(i.ToString())).ToList();

                var currentIx = 0;

                for(var t = 0; t < count; t++)
                {
                    var current = data[currentIx];

                    //var movingIx = Enumerable.Range(currentIx, 3).Select(i => i % data.Count).ToList();
                    //var moving = movingIx.Select(i => data[i]).ToList();
                    //var destination = ((current - 2) + data.Count) % data.Count + 1;
                    //while (moving.Contains(destination))
                    //{
                    //    destination = ((destination - 2) + data.Count) % data.Count + 1;
                    //}
                    //var destinationIx = data.IndexOf(destination);



                    var window = data.Concat(data).Skip(currentIx).Take(data.Count);
                    var moving = window.Skip(1).Take(3).ToList();
                    var left = window.Take(1).Concat(window.Skip(4)).ToList();

                    Console.WriteLine("left " + string.Join("", left));

                    var destination = ((current - 2) + data.Count) % data.Count + 1;
                    while (moving.Contains(destination))
                    {
                        destination = ((destination - 2) + data.Count) % data.Count + 1;
                    }

                    Console.WriteLine("destination " + destination);

                    left.InsertRange(left.IndexOf(destination) + 1, moving);

                    data = left.Concat(left).Skip(data.Count - currentIx).Take(data.Count).ToList();
                    currentIx++;
                    currentIx %= data.Count;

                    Console.WriteLine(string.Join("", data));
                }

                Console.WriteLine("Answer: " + string.Join("", data.Concat(data).SkipWhile(i => i != 1).Take(data.Count)));

                // not 32674859
                // is 24798635
                Console.WriteLine();
            });
        }

        public override void Run()
        {
            RunScenario("initial", @"389125467", 100);
            //return;
            RunScenario("part1", @"362981754", 100);

        }
    }
}
