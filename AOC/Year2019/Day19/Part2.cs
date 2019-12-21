using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AOC;
using AOC.Year2019.Day15;

namespace AoC.Year2019.Day19
{
    public class Part2 : BasePart
    {
        protected void RunScenario(string title, string input)
        {

            RunScenario(title, () =>
            {
                var lines = input.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                var initialData = lines[0].Split(',').Select(long.Parse).ToArray();
                var data = initialData.Select((i, ii) => new { i, ii }).ToDictionary(i => i.ii, i => i.i);

                var initialState = IntCodeSimulationState.Start(data);

                var workedPoints = new List<(long, long)>();

                //bool works(long num)
                //{
                //    var d = num / 1000000000;
                //    var x = num % 1000000000;
                //    var y = Math.Max(0, d - x);
                //    Console.WriteLine($"Checking {x},{y} ({num})");
                //    var state1 = initialState.Resume(x);
                //    var state2 = state1.Resume(y);
                //    var startWorked = state2.Output.Single() == 1;
                //    if (!startWorked)
                //    {
                //        return false;
                //    }

                //    var p1Worked = initialState.Resume(x).Resume(y + 99).Output.Single() == 1;
                //    var p2Worked = initialState.Resume(x + 99).Resume(y).Output.Single() == 1;
                //    var p3Worked = initialState.Resume(x + 99).Resume(y + 99).Output.Single() == 1;
                //    if (p1Worked && p2Worked && p3Worked)
                //    {
                //        workedPoints.Add((x, y));
                //        //Console.WriteLine($"Worked @ {x},{y}");
                //        return true;
                //    }

                //    return false;
                //}

                ////var workingD = DoSearchDown(long.MaxValue, (d) =>
                ////{
                ////    var x = d / 2;
                ////    var y = d - x;
                ////    return works(x, y);
                ////});
                //var workingD = DoSearchDown(long.MaxValue, works);

                //var point = workedPoints.OrderBy(i => i.Item1 + i.Item2).First();

                var cache = new Dictionary<(long, long), bool>();
                bool works(long x, long y)
                {
                    if (cache.TryGetValue((x, y), out var v))
                    {
                        return v;
                    }

                    var state1 = initialState.Resume(x);
                    var state2 = state1.Resume(y);
                    var r = state2.Output.Single() == 1;
                    //Console.WriteLine($"Checking {x},{y} -> {r}");
                    cache[(x, y)] = r;
                    return r;
                    //var startWorked = state2.Output.Single() == 1;
                    //if (!startWorked)
                    //{
                    //    return false;
                    //}

                    //var p1Worked = initialState.Resume(x).Resume(y + 99).Output.Single() == 1;
                    //var p2Worked = initialState.Resume(x + 99).Resume(y).Output.Single() == 1;
                    //var p3Worked = initialState.Resume(x + 99).Resume(y + 99).Output.Single() == 1;
                    //if (p1Worked && p2Worked && p3Worked)
                    //{
                    //    //workedPoints.Add((x, y));
                    //    //Console.WriteLine($"Worked @ {x},{y}");
                    //    return true;
                    //}

                    //return false;
                }

                bool testD(long d)
                {

                    var range = DoRangeSearch(0, d, x => works(x, d - x));
                    // 100x100 is different by 99 from left/right and top/bottom
                    if (range.Item2 - range.Item1 >= 99)
                    {
                        var point = (range.Item1, d - range.Item1 - 99);
                        workedPoints.Add(point);
                        Console.WriteLine($"{point.Item1},{point.Item2} = {point.Item1 * 10000 + point.Item2}");
                        return true;
                    }

                    //Console.WriteLine($"{d} -> {range.Item1},{range.Item2}");

                    return false;
                }

                var workingD = BinarySearch.GetMin(testD, 1000000000);

                for (var i = 0; i < 100; i++)
                {
                    var j = workingD - i;
                    Console.WriteLine($"{j}: {testD(j)}");
                }

                var point = workedPoints.OrderBy(i => i.Item1 + i.Item2).First();


                //Console.WriteLine($"Starting search at {point.Item1},{point.Item2} = {point.Item1 * 10000 + point.Item2}");

                //(long, long) improve1((long, long) point)
                //{
                //    var newX = DoSearchDown(point.Item1, (x) =>
                //    {
                //        return works(x, point.Item2);
                //    });
                //    var newY = DoSearchDown(point.Item2, (y) =>
                //    {
                //        return works(newX, y);
                //    });
                //    var r = (newX, newY);
                //    if (distance(r) > distance(point) || r.newX > 100000000000 || r.newY > 100000000000)
                //    {
                //        return point;
                //    }

                //    return r;
                //}

                //(long, long) improve2((long, long) point)
                //{
                //    var newY = DoSearchDown(point.Item2, (y) =>
                //    {
                //        return works(point.Item1, y);
                //    });
                //    var newX = DoSearchDown(point.Item1, (x) =>
                //    {
                //        return works(x, newY);
                //    });
                //    var r = (newX, newY);
                //    if (distance(r) > distance(point) || r.newX > 100000000000 || r.newY> 100000000000)
                //    {
                //        return point;
                //    }

                //    return r;
                //}


                //(long, long) improve3((long, long) point)
                //{
                //    var newX = DoSearchDown(point.Item1, (x) =>
                //    {
                //        var y = point.Item2 - (point.Item1 - x);
                //        Console.WriteLine($"Checking {x},{y}");
                //        return works(x, y);
                //    });
                //    var r = (newX, point.Item2 - (point.Item1 - newX));
                //    if (distance(r) > distance(point) || r.newX > 100000000000 || r.Item2 > 100000000000)
                //    {
                //        Console.WriteLine($"{point.Item1},{point.Item2} -> {r.newX},{r.Item2} failed");
                //        return point;
                //    }

                //    Console.WriteLine($"{point.Item1},{point.Item2} -> {r.newX},{r.Item2} succeeded");
                //    return r;
                //}

                //(long, long) improve4((long, long) point)
                //{
                //    var newX = DoSearchDown(point.Item1, (x) =>
                //    {
                //        var y = point.Item2 + (point.Item1 - x);
                //        Console.WriteLine($"Checking2 {x},{y}");
                //        return works(x, y);
                //    });
                //    var r = (newX, point.Item2 - (point.Item1 - newX));
                //    if (distance(r) > distance(point) || r.newX > 100000000000 || r.Item2 > 100000000000)
                //    {
                //        Console.WriteLine($"{point.Item1},{point.Item2} -> {r.newX},{r.Item2} failed");
                //        return point;
                //    }

                //    Console.WriteLine($"{point.Item1},{point.Item2} -> {r.newX},{r.Item2} succeeded");
                //    return r;
                //}


                //long distance((long, long) point)
                //{
                //    return point.Item1 + point.Item2;
                //}

                //while (true)
                //{
                //    var points = new[]
                //    {
                //        improve1(point),
                //        improve2(point),
                //        improve3(point),
                //        improve4(point),
                //        //improve1((point.Item1 - 1, point.Item2 - 1)),
                //        //improve2((point.Item1 - 1, point.Item2 - 1)),
                //    };
                //    var newPoint = points.OrderBy(distance).First();
                //    ////if (altNewPoint.Item1 + altNewPoint, item2)
                //    //var newX = DoSearchDown(point.Item1, (x) =>
                //    //{
                //    //    return works(x, point.Item2);
                //    //});
                //    //var newY = DoSearchDown(point.Item2, (y) =>
                //    //{
                //    //    return works(newX, y);
                //    //});
                //    //var altX = DoSearchDown(newX, (x) =>
                //    //{
                //    //    return works(x, newY - 1);
                //    //});
                //    //if (altX > newX)
                //    //{
                //    //    altX = newX;
                //    //}
                //    //else
                //    //{
                //    //    newY--;
                //    //}
                //    //var altY = DoSearchDown(newY, (y) =>
                //    //{
                //    //    return works(altX-1, y);
                //    //});
                //    //if (altY > newY)
                //    //{
                //    //    altY = newY;
                //    //}
                //    //else
                //    //{
                //    //    altX--;
                //    //}
                //    Console.WriteLine($"{point.Item1},{point.Item2} -> {newPoint.Item1},{newPoint.Item2}= {newPoint.Item1 * 10000 + newPoint.Item2}");
                //    if (point == newPoint)
                //    {
                //        break;
                //    }
                //    point = newPoint;
                //}

                //Console.WriteLine($"{point.Item1},{point.Item2} = {point.Item1 * 10000 + point.Item2}");

                //// 6453376653012295112 is wrong
                //// 25266141840166 is wrong
                //// 25266142340493 is wrong
                //// 39486726538327
                //// 3948605915,667388327 = 39486726538327 is probably wrong too

                var sb = new StringBuilder();
                for (var y = -20; y < 120; y++)
                {
                    for (var x = -20; x < 120; x++)
                    {
                        var value = works(x + point.Item1, y + point.Item2);
                        var isShip = x >= 0 && x < 100 && y >= 0 && y < 100;
                        sb.Append(value ? (isShip ? 'O' : '#') : (isShip ? 'X' : '.'));
                    }

                    sb.AppendLine();
                }
                Console.WriteLine(sb.ToString());

                Console.WriteLine($"{point.Item1},{point.Item2} = {point.Item1 * 10000 + point.Item2}");



                ////for (var x = 0; x < workingD; x++)
                ////{
                ////    var y = workingD - x;
                ////    var state1 = initialState.Resume(x);
                ////    var state2 = state1.Resume(y);
                ////    var startWorked = state2.Output.Single() == 1;
                ////    if (!startWorked)
                ////    {
                ////        return false;
                ////    }

                ////    var endWorked = initialState.Resume(x + 10).Resume(y + 10).Output.Single() == 1;
                ////    if (endWorked)
                ////    {
                ////        workedPoints.Add((x, y));
                ////        return true;
                ////    }
                ////}

                ////Console.WriteLine(workingD);
                ////var best = workedPoints.OrderByDescending(i => i.Item1 + i.Item2).First();
                ////Console.WriteLine($"{best.Item1},{best.Item2} = {best.Item1 * 10000 + best.Item2}");
                // 1069,1717 = 10691717 - SUBTRACT 2 FROM y -> 10691715
                // 1070,1716 = 10701716 - add 1 to Y -> 10701717
                // 1070,1717 = 10701717 - WRONG
                // 1067,1712 = 10671712 - CORRECT
            });
        }
        private (long, long) DoRangeSearch(long min, long max, Func<long, bool> test)
        {
            //var minValue = test(min) ? min : DoSearchUp(min, i => i < max ? !test(i) : false) + 1;
            //var maxValue = test(max) ? max : DoSearchDown(max, i => i > min ? !test(i) : false) + 1;
            //Console.WriteLine($"min/max -> {minValue}, {maxValue}");
            //return (minValue, maxValue);
            //long bestWorking = long.MaxValue;
            //long worstNotWorking = 0;
            var rnd = new Random();
            while (true)
            {
                var guess = (rnd.Next(int.MaxValue) * (long)rnd.Next(int.MaxValue)) % (max - min) + min;
                if (test(guess))
                {
                    //Console.WriteLine($"starting from guess {guess} - {min} - {max}");
                    var minValue = BinarySearch.GetMin(i => i > guess ? false :test(i), guess, 0, guess);
                    var maxValue = BinarySearch.GetMax(i => i < guess ? false : test(i), guess, guess);
                    //Console.WriteLine($"{guess} min/max -> {minValue}, {maxValue}");
                    return (Math.Min(minValue, maxValue), Math.Max(minValue, maxValue));
                }
            }
        }

        public override void Run()
        {
            //return;
            RunScenario("part1", @"109,424,203,1,21102,1,11,0,1106,0,282,21101,0,18,0,1106,0,259,1202,1,1,221,203,1,21101,0,31,0,1105,1,282,21102,38,1,0,1105,1,259,20102,1,23,2,21201,1,0,3,21102,1,1,1,21101,0,57,0,1105,1,303,2101,0,1,222,20102,1,221,3,21002,221,1,2,21101,0,259,1,21101,0,80,0,1106,0,225,21102,1,152,2,21101,91,0,0,1106,0,303,1201,1,0,223,21001,222,0,4,21101,0,259,3,21102,225,1,2,21101,0,225,1,21102,1,118,0,1105,1,225,20101,0,222,3,21102,61,1,2,21101,133,0,0,1106,0,303,21202,1,-1,1,22001,223,1,1,21102,148,1,0,1105,1,259,2101,0,1,223,21001,221,0,4,21001,222,0,3,21101,0,14,2,1001,132,-2,224,1002,224,2,224,1001,224,3,224,1002,132,-1,132,1,224,132,224,21001,224,1,1,21101,0,195,0,105,1,109,20207,1,223,2,20101,0,23,1,21102,-1,1,3,21102,214,1,0,1105,1,303,22101,1,1,1,204,1,99,0,0,0,0,109,5,2101,0,-4,249,21202,-3,1,1,21202,-2,1,2,21201,-1,0,3,21102,1,250,0,1106,0,225,22101,0,1,-4,109,-5,2106,0,0,109,3,22107,0,-2,-1,21202,-1,2,-1,21201,-1,-1,-1,22202,-1,-2,-2,109,-3,2105,1,0,109,3,21207,-2,0,-1,1206,-1,294,104,0,99,22102,1,-2,-2,109,-3,2105,1,0,109,5,22207,-3,-4,-1,1206,-1,346,22201,-4,-3,-4,21202,-3,-1,-1,22201,-4,-1,2,21202,2,-1,-1,22201,-4,-1,1,21202,-2,1,3,21101,343,0,0,1106,0,303,1105,1,415,22207,-2,-3,-1,1206,-1,387,22201,-3,-2,-3,21202,-2,-1,-1,22201,-3,-1,3,21202,3,-1,-1,22201,-3,-1,2,22101,0,-4,1,21101,0,384,0,1106,0,303,1105,1,415,21202,-4,-1,-4,22201,-4,-3,-4,22202,-3,-2,-2,22202,-2,-4,-4,22202,-3,-2,-3,21202,-4,-1,-2,22201,-3,-2,1,21201,1,0,-4,109,-5,2106,0,0");

        }
    }
}
