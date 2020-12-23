using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC.Year2020.Day23
{
    public class Part2 : BasePart
    {
        protected void RunScenario(string title, string input, int padding, int count)
        {
            RunScenario(title, () =>
            {
                var data = input.Select(i => int.Parse(i.ToString())).ToList();
                data = data.Concat(Enumerable.Range(data.Count + 1, padding - data.Count)).ToList();


                var valueCache = new Dictionary<(int, int), int>();
                var indexCache = new Dictionary<(int, int), int>();

                int getValue(int index, int step)
                {
                    if (step == 0)
                    {
                        return data[index];
                    }

                    var key = (index, step);
                    if (valueCache.TryGetValue(key, out var v))
                    {
                        return v;
                    }
                    else
                    {
                        var v2 = _getValue(index, step);
                        valueCache[key] = v2;
                        indexCache[(v2, step)] = index;
                        return v2;
                    }
                }

                int getIndex(int value, int step)
                {
                    var key = (value, step);
                    if (indexCache.TryGetValue(key, out var v))
                    {
                        return v;
                    }
                    else
                    {
                        var v2 = _getIndex(value, step);
                        if (v2 < 0 || v2 >= data.Count)
                        {
                            throw new ArgumentException();
                        }
                        indexCache[key] = v2;
                        valueCache[(v2, step)] = value;
                        return v2;
                    }
                }

                int _getValue(int index, int step)
                {
                    var pCurrent = getValue(0, step - 1);
                    if (index == data.Count - 1)
                    {
                        return pCurrent;
                    }

                    var pDestination = (pCurrent - 2 + data.Count) % data.Count + 1;
                    var pDestinationIx = getIndex(pDestination, step - 1);
                    while (pDestinationIx == 1 || pDestinationIx == 2 || pDestinationIx == 3)
                    {
                        pDestination = (pDestination - 2 + data.Count) % data.Count + 1;
                        pDestinationIx = getIndex(pDestination, step - 1);
                    }

                    if (index < pDestinationIx - 4)
                    {
                        return getValue(index + 4, step - 1);
                    }
                    if (index == pDestinationIx - 4)
                    {
                        return pDestination;
                    }
                    if (index == pDestinationIx - 3)
                    {
                        return getValue(1, step - 1);
                    }
                    if (index == pDestinationIx - 2)
                    {
                        return getValue(2, step - 1);
                    }
                    if (index == pDestinationIx - 1)
                    {
                        return getValue(3, step - 1);
                    }
                    if(index >= pDestinationIx)
                    {
                        return getValue(index + 1, step - 1);
                    }
                    throw new InvalidOperationException($"Couldn't determine index to get value for {index} w/ {pDestinationIx}");
                }

                int _getIndex(int value, int step)
                {
                    if (step == 0)
                    {
                        return data.IndexOf(value);
                    }

                    var pIx = getIndex(value, step - 1);
                    if (pIx == 0)
                    {
                        return data.Count - 1;
                    }

                    var pCurrent = getValue(0, step - 1);

                    var pDestination = (pCurrent - 2 + data.Count) % data.Count + 1;
                    var pDestinationIx = getIndex(pDestination, step - 1);
                    while (pDestinationIx == 1 || pDestinationIx == 2 || pDestinationIx == 3)
                    {
                        pDestination = (pDestination - 2 + data.Count) % data.Count + 1;
                        pDestinationIx = getIndex(pDestination, step - 1);
                    }

                    if (pIx == pDestinationIx)
                    {
                        return pDestinationIx - 4;
                    }
                    if (pIx == 1)
                    {
                        return pDestinationIx - 3;
                    }
                    if (pIx == 2)
                    {
                        return pDestinationIx - 2;
                    }
                    if (pIx == 3)
                    {
                        return pDestinationIx - 1;
                    }

                    if (pIx < pDestinationIx)
                    {
                        return pIx - 4;
                    }
                    if (pIx > pDestinationIx)
                    {
                        return pIx - 1;
                    }

                    throw new InvalidOperationException($"Couldn't determine index for {value} w/ {pDestinationIx}");
                }

                ////var check = new Dictionary<string, int>();

                //for (var t = 0; t < count; t++)
                //{
                //    var current = data[0];

                //    //var movingIx = Enumerable.Range(currentIx, 3).Select(i => i % data.Count).ToList();
                //    //var moving = movingIx.Select(i => data[i]).ToList();
                //    //var destination = ((current - 2) + data.Count) % data.Count + 1;
                //    //while (moving.Contains(destination))
                //    //{
                //    //    destination = ((destination - 2) + data.Count) % data.Count + 1;
                //    //}
                //    //var destinationIx = data.IndexOf(destination);


                //    var moving = data.Skip(1).Take(3).ToList();
                //    var left = data.Take(1).Concat(data.Skip(4)).ToList();

                //    var destination = ((current - 2) + data.Count) % data.Count + 1;
                //    while (moving.Contains(destination))
                //    {
                //        destination = ((destination - 2) + data.Count) % data.Count + 1;
                //    }

                //    left.InsertRange(left.IndexOf(destination) + 1, moving);

                //    data = left.Concat(left).Skip(data.Count - 1).Take(data.Count).ToList();

                //    //var key = string.Join("", data);
                //    //if (check.TryGetValue(key, out var v))
                //    //{
                //    //    Console.WriteLine($"Found loop from {v} to {t}");
                //    //    break;
                //    //}
                //    //check.Add(key, t);

                //    if (t % 100 == 0)
                //    {
                //        Console.WriteLine($"Finished {t} loops");
                //    }
                //}

                //Console.WriteLine("Answer: " + string.Join("", data.Concat(data).SkipWhile(i => i != 1).Take(data.Count)));

                // not 32674859
                // is 24798635

                Console.WriteLine($"After {count} rounds with {padding} numbers");

                var ix = getIndex(1, count);
                Console.WriteLine($"1 is at index {ix}");

                var v1 = getValue((ix + 1) % padding, count);
                Console.WriteLine($"Followed by {v1}");
                var v2 = getValue((ix + 2) % padding, count);
                Console.WriteLine($"Followed by {v2}");

                var answer = v1 * v2;

                Console.WriteLine($"Answer: {answer}");
            });
        }


        public override void Run()
        {
            RunScenario("initial", @"389125467", 1000000, 1000);

            //RunScenario("initial", @"389125467", 9, 100);
            ////return;
            //RunScenario("part1", @"362981754", 9, 100);

            //RunScenario("initial", @"389125467", 1000000, 1000000);
            ////return;
            //RunScenario("part1", @"362981754", 1000000, 10000000);

        }
    }
}
