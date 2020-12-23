using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC.Year2020.Day23
{
    public class Part2 : BasePart
    {
        private interface IResult { }

        private abstract class Call : IResult
        {
            public Func<int, int> After;

        }

        private class GetValueCall : Call
        {
            public int Index;
            public int Step;

            public GetValueCall(int index, int step, Func<int, int> after = null)
            {
                Index = index;
                Step = step;
                After = after;
            }
        }

        private class GetIndexCall : Call
        {
            public int Value;
            public int Step;

            public GetIndexCall(int value, int step, Func<int, int> after = null)
            {
                Value = value;
                Step = step;
                After = after;
            }
        }

        private class ResultValue : IResult
        {
            public int Value;

            public ResultValue(int value)
            {
                Value = value;
            }
        }


        protected void RunScenario(string title, string input, int padding, int count)
        {
            RunScenario(title, () =>
            {
                var data = input.Select(i => int.Parse(i.ToString())).ToList();
                data = data.Concat(Enumerable.Range(data.Count + 1, padding - data.Count)).ToList();

                var valueCache = new Dictionary<int, Func<int, IResult>>();
                var indexCache = new Dictionary<int, Func<int, IResult>>();

                var cacheInterval = 1000;
                var realValueCache = new Dictionary<int, int>();
                var realIndexCache = new Dictionary<int, int>();

                int evaluate(IResult result)
                {
                    var stack = new Stack<IResult>();
                    while(true)
                    {
                        stack.Push(result);
                        if (result is ResultValue val)
                        {
                            stack.Pop();
                            var v = val.Value;
                            while (stack.Count > 0)
                            {
                                var r = stack.Pop();
                                if (r is Call c)
                                {
                                    if(c.After != null)
                                    {
                                        v = c.After(v);
                                    }
                                }
                            }
                            return v;
                        }
                        else if (result is GetIndexCall iCall)
                        {
                            // somehow cache every 1000 or whatever
                            result = getIndex(iCall.Value, iCall.Step);
                        }
                        else if (result is GetValueCall vCall)
                        {
                            result = getValue(vCall.Index, vCall.Step);
                        }
                    }
                }

                IResult getValue(int index, int step)
                {
                    if (step == 0)
                    {
                        return new ResultValue(data[index]);
                    }

                    if (valueCache.TryGetValue(step, out var v))
                    {
                        return v(index);
                    }
                    else
                    {
                        //Console.WriteLine($"Building value for {step}");
                        //if (step >=4)
                        //{

                        //}
                        var v2 = _getValueFunction(step);
                        valueCache[step] = v2;
                        return v2(index);
                    }
                }

                IResult getIndex(int value, int step)
                {
                    if (indexCache.TryGetValue(step, out var v))
                    {
                        return v(value);
                    }
                    else
                    {
                        //Console.WriteLine($"Building index for {step}");
                        var v2 = _getIndexFunction(step);
                        //if (v2 < 0 || v2 >= data.Count)
                        //{
                        //    throw new ArgumentException();
                        //}
                        indexCache[step] = v2;
                        return v2(value);
                    }
                }

                Func<int, IResult> _getValueFunction(int step)
                {
                    if (step == 0)
                    {
                        return (index) => new ResultValue(data[index]);
                    }

                    var pCurrent = evaluate(getValue(0, step - 1));

                    var pDestination = (pCurrent - 2 + data.Count) % data.Count + 1;
                    var pDestinationIx = evaluate(getIndex(pDestination, step - 1));
                    while (pDestinationIx == 1 || pDestinationIx == 2 || pDestinationIx == 3)
                    {
                        pDestination = (pDestination - 2 + data.Count) % data.Count + 1;
                        pDestinationIx = evaluate(getIndex(pDestination, step - 1));
                    }

                    return (index) =>
                    {
                        if (index == data.Count - 1)
                        {
                            return new ResultValue(pCurrent);
                        }
                        if (index < pDestinationIx - 4)
                        {
                            return new GetValueCall(index + 4, step - 1);
                        }
                        if (index == pDestinationIx - 4)
                        {
                            return new ResultValue(pDestination);
                        }
                        if (index == pDestinationIx - 3)
                        {
                            return new GetValueCall(1, step - 1);
                        }
                        if (index == pDestinationIx - 2)
                        {
                            return new GetValueCall(2, step - 1);
                        }
                        if (index == pDestinationIx - 1)
                        {
                            return new GetValueCall(3, step - 1);
                        }
                        if (index >= pDestinationIx)
                        {
                            return new GetValueCall(index + 1, step - 1);
                        }
                        throw new InvalidOperationException($"Couldn't determine index to get value for {index} w/ {pDestinationIx}");
                    };
                }

                Func<int, IResult> _getIndexFunction(int step)
                {
                    if (step == 0)
                    {
                        return (value) => new ResultValue(data.IndexOf(value));
                    }

                    var pCurrent = evaluate(getValue(0, step - 1));

                    var pDestination = (pCurrent - 2 + data.Count) % data.Count + 1;
                    var pDestinationIx = evaluate(getIndex(pDestination, step - 1));
                    while (pDestinationIx == 1 || pDestinationIx == 2 || pDestinationIx == 3)
                    {
                        pDestination = (pDestination - 2 + data.Count) % data.Count + 1;
                        pDestinationIx = evaluate(getIndex(pDestination, step - 1));
                    }

                    Func<int, int> after = (pIx) =>
                    {
                        if (pIx == 0)
                        {
                            return data.Count - 1;
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

                        throw new InvalidOperationException($"Couldn't determine index for {pDestinationIx}");
                    };

                    return (value) =>
                    {
                        return new GetIndexCall(value, step - 1, after);
                    };
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

                Console.WriteLine($"Running for {count} rounds with {padding} numbers");

                for(var t = 0; t < count; t++)
                {
                    if (t % 1000 == 0)
                    {
                        Console.WriteLine($"Preloading {t}");
                    }
                    getValue(0, t);
                    getIndex(0, t);
                }

                Console.WriteLine($"After {count} rounds with {padding} numbers");

                var ix = evaluate(getIndex(1, count));
                Console.WriteLine($"1 is at index {ix}");

                var v1 = evaluate(getValue((ix + 1) % padding, count));
                Console.WriteLine($"Followed by {v1}");
                var v2 = evaluate(getValue((ix + 2) % padding, count));
                Console.WriteLine($"Followed by {v2}");

                var answer = v1 * v2;

                Console.WriteLine($"Answer: {answer}");

                //var keys = valueCache.Keys.OrderBy(i => i.Item2).ThenBy(i => i.Item1).ToList();
                //var groups = valueCache.Keys.GroupBy(i => i.Item2, i => i.Item2).OrderBy(i => i.Key).Select(i => new { i.Key, values = i.OrderBy(ii => ii).ToList() }).ToList();
            });
        }


        public override void Run()
        {
            RunScenario("initial", @"389125467", 9, 100);
            //return;
            RunScenario("part1", @"362981754", 9, 100);


            RunScenario("initial", @"389125467", 1000000, 10000);

            //RunScenario("initial", @"389125467", 9, 100);
            ////return;
            //RunScenario("part1", @"362981754", 9, 100);

            //RunScenario("initial", @"389125467", 1000000, 1000000);
            ////return;
            //RunScenario("part1", @"362981754", 1000000, 10000000);

        }
    }
}
