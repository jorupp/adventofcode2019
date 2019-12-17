using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace AoC.Year2019.Day16
{
    public class Part2 : BasePart
    {
        protected void RunScenario(string title, string input, int numPhases)
        {
            RunScenario(title, () =>
            {
                var lines = input.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

                var numbers = lines[0].Select(i => int.Parse(i.ToString())).ToArray();
                numbers = Enumerable.Range(0, 10000).SelectMany(i => numbers).ToArray();
                var offset = int.Parse(string.Join("", numbers.Take(7)));
                var startPoint = offset - numPhases;
                numbers = numbers.Skip(startPoint).ToArray();
                Console.WriteLine($"Will use offset {offset}, starting at {startPoint}, output will be from {offset - startPoint} of {numbers.Length}");

                //var cache = new Dictionary<(int, int), int>();

                //string GetCalclation(int iteration, int ix)
                //{
                //    if (iteration == 0)
                //    {
                //        return $"Number[{ix}]";
                //    }

                //    var x = new List<string>();
                //    for (var jx = startPoint; jx < numbers.Length; jx++)
                //    {
                //        switch (((jx + 1) / (ix + 1)) % 4)
                //        {
                //            case 1:
                //                x.Add("+" + GetCalclation(iteration - 1, jx));
                //                break;
                //            case 3:
                //                x.Add("-" + GetCalclation(iteration - 1, jx));
                //                break;
                //        }
                //    }

                //    return $"(0{string.Join("", x)})";
                //}

                //Console.WriteLine(GetCalclation(1, offset));
                //return;


                //int GetValue(int iteration, int ix)
                //{
                //    if (iteration == 0)
                //    {
                //        return numbers[ix];
                //    }
                //    var key = (iteration, offset: ix);
                //    if (cache.TryGetValue(key, out var v))
                //    {
                //        return v;
                //    }

                //    //Console.WriteLine($"Calculating {iteration} @ {ix}");

                //    int r = 0;
                //    for (var jx = startPoint; jx < numbers.Length; jx++)
                //    {
                //        switch (((jx + 1) / (ix + 1)) % 4)
                //        {
                //            case 1:
                //                r += GetValue(iteration - 1, jx);
                //                break;
                //            case 3:
                //                r -= GetValue(iteration - 1, jx);
                //                break;
                //        }
                //    }

                //    v = r % 10;
                //    cache[key] = v;

                //    return v;
                //}

                //var basePattern = new long[] { 0, 1, 0, -1 };

                //List<long> getPattern(int ix)
                //{
                //    var length = numbers.Count;
                //    var repeats = ix + 1;

                //    var initialRepeats = (int)Math.Ceiling(((decimal) (length + 1)) / repeats / basePattern.Length);
                //    //Console.WriteLine($"Getpattner {ix} - {initialRepeats} - {repeats}");

                //    return Enumerable.Range(0, initialRepeats).SelectMany(i =>
                //        basePattern.SelectMany(ii => Enumerable.Range(0, repeats).Select(iii => ii)))
                //        .Skip(1).Take(length).ToList();
                //}

                //var patterns = Enumerable.Range(0, numbers.Count).Select(getPattern).ToList();
                //Console.WriteLine("Generated patterns");

                void RunPhase(int[] data, int[] output)
                {
                    var length = data.Length;
                    var r = 0;
                    for (var ix = length - 1; ix >= 0; ix--)
                    {
                        r += data[ix];
                        output[ix] = r % 10;
                    }

                    //Enumerable.Range(0, length).AsParallel().ForAll(ix =>
                    //{
                    //    int r = 0;
                    //    for (var jx = ix; jx < length; jx++)
                    //    {
                    //        r += data[jx];
                    //        //switch (((jx + 1 + startPoint) / (ix + 1 + startPoint)) % 4)
                    //        //{
                    //        //    case 1:
                    //        //        r += data[jx];
                    //        //        break;
                    //        //    case 3:
                    //        //        r -= data[jx];
                    //        //        break;
                    //        //}
                    //    }

                    //    output[ix] = r % 10;
                    //});
                    //for (var ix = 0; ix < length; ix++)
                    //{
                    //    int r = 0;
                    //    for (var jx = ix; jx < length; jx++)
                    //    {
                    //        r += data[jx];
                    //        //switch (((jx + 1 + startPoint) / (ix + 1 + startPoint)) % 4)
                    //        //{
                    //        //    case 1:
                    //        //        r += data[jx];
                    //        //        break;
                    //        //    case 3:
                    //        //        r -= data[jx];
                    //        //        break;
                    //        //}
                    //    }

                    //    output[ix] = r % 10;
                    //}
                    //return data.Select((i, ix) =>
                    //{
                    //    //var pattern = patterns[ix];
                    //    return Math.Abs(
                    //               data.Select((j, jx) =>
                    //               {
                    //                   //var pattern = basePattern[((jx + 1 + startPoint) / (ix + 1 + startPoint)) % 4];
                    //                   //return (j * pattern);
                    //                   switch (((jx + 1 + startPoint) / (ix + 1 + startPoint)) % 4)
                    //                   {
                    //                       case 0:
                    //                       case 2:
                    //                           return 0;
                    //                       case 1:
                    //                           return j;
                    //                       case 3:
                    //                           return -j;
                    //                   }

                    //                   throw new NotImplementedException();
                    //               }).Sum()) % 10;
                    //}).ToList();
                }

                // 32 = 320k * 320k = 102B
                // 16k * 16k = 250M * 100
                // 650 = 6.5M * 6.5M... 42T
                // 500k * 500k = 2.5B * 100...

                //for (var i = 0; i < numPhases; i++)
                //{
                //    Console.WriteLine($"Running phase {i}");
                //    numbers = RunPhase(numbers);
                //}

                for (var i = 0; i < numPhases; i++)
                {
                    //Console.WriteLine($"Running phase {i}");
                    //Console.WriteLine($"really running phase {i}");
                    var output = numbers.ToArray();
                    RunPhase(numbers, output);
                    numbers = output;
                }


                //var lastPhase = numbers.ToArray();
                //for (var i = 0; i < numPhases; i++)
                //{
                //    RunPhase(lastPhase, numbers);
                //    Console.WriteLine($"Running phase {i}");
                //    //numbers = RunPhase(numbers);
                //}

                //Console.WriteLine(string.Join(",", getPattern(0)));
                //Console.WriteLine(string.Join(",", getPattern(1)));
                //Console.WriteLine(string.Join(",", getPattern(2)));
                //Console.WriteLine(string.Join(",", getPattern(3)));

                Console.WriteLine(string.Join("", numbers.Skip(offset - startPoint).Take(8)));

                //Console.WriteLine(string.Join("", Enumerable.Range(offset, 8).Select(i => GetValue(100, i))));
            });
        }

        public override void Run()
        {
            //RunScenario("initial", @"12345678", 100);
            RunScenario("initial", @"03036732577212944063491565474664", 100);
            RunScenario("initial", @"02935109699940807407585447034323", 100);
            RunScenario("initial", @"03081770884921959731165446850517", 100);
            //return;
            RunScenario("part1", @"59734319985939030811765904366903137260910165905695158121249344919210773577393954674010919824826738360814888134986551286413123711859735220485817087501645023012862056770562086941211936950697030938202612254550462022980226861233574193029160694064215374466136221530381567459741646888344484734266467332251047728070024125520587386498883584434047046536404479146202115798487093358109344892308178339525320609279967726482426508894019310795012241745215724094733535028040247643657351828004785071021308564438115967543080568369816648970492598237916926533604385924158979160977915469240727071971448914826471542444436509363281495503481363933620112863817909354757361550", 100);

        }
    }
}
