using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using AOC;

namespace AoC.Year2019.Day22
{
    public class Part2 : BasePart
    {
        //long a, x, b, c;
        //return (a * x + b) % c;

        protected void RunScenario(string title, string input, long numCards, long targetPosition)
        {
            RunScenario(title, () =>
            {
                var lines = input.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);


                Func<long, long> DealNewStack(Func<long, long> input)
                {
                    return (targetPosition) => (-1 * input(targetPosition) + -1).ModAbs(numCards);
                }

                Func<long, long> Cut(Func<long, long> input, long count)
                {
                    //if (count < 0)
                    //{
                    //    count = numCards + count;
                    //}

                    return (prev) =>
                    {
                        var targetPosition = input(prev);
                        return (1 * targetPosition + count).ModAbs(numCards);
                        //if (targetPosition < numCards - count)
                        //{
                        //    return targetPosition + count;
                        //}
                        //else
                        //{
                        //    return targetPosition - (numCards - count);
                        //}
                    };
                }

                Func<long, long> DealWithIncrement(Func<long, long> input, long increment)
                {
                    long dealt = 0;
                    long startPosition = 0;
                    for (var i = 0; ; i++)
                    {
                        //Console.WriteLine($"T {targetPosition} {startPosition} {increment}");
                        if ((1 - startPosition) % increment == 0)
                        {
                            break;
                        }

                        dealt += (long)Math.Ceiling(((decimal)numCards - startPosition) / increment);
                        startPosition += increment - (numCards % increment);
                        startPosition %= increment;
                    }

                    //Console.WriteLine($"D {dealt} {targetPosition} {startPosition} {increment}");
                    dealt += (1 - startPosition) / increment;

                    return (prev) =>
                    {
                        var targetPosition = input(prev);
                        var a = dealt;
                        checked
                        {
                            return (a * targetPosition + 0).ModAbs(numCards);
                        }
                        //long dealt = 0;
                        //long startPosition = 0;
                        //for (var i = 0; ; i++)
                        //{
                        //    //Console.WriteLine($"T {targetPosition} {startPosition} {increment}");
                        //    if ((targetPosition - startPosition) % increment == 0)
                        //    {
                        //        break;
                        //    }

                        //    dealt += (long)Math.Ceiling(((decimal)numCards - startPosition) / increment);
                        //    startPosition += increment - (numCards % increment);
                        //    startPosition %= increment;
                        //}

                        ////Console.WriteLine($"D {dealt} {targetPosition} {startPosition} {increment}");
                        //dealt += (targetPosition - startPosition) / increment;

                        //return dealt;
                    };
                }

                Func<long, long> Compute()
                {
                    Func<long, long> result = v => v;
                    foreach (var line in lines.Reverse())
                    {
                        if (line.StartsWith("deal with increment"))
                        {
                            var x = long.Parse(line.Split(' ').Last());
                            result = DealWithIncrement(result, x);
                        }
                        else if (line.StartsWith("cut"))
                        {
                            var x = long.Parse(line.Split(' ').Last());
                            result = Cut(result, x);
                        }
                        else if (line.StartsWith("deal into new stack"))
                        {
                            result = DealNewStack(result);
                        }
                        else
                        {
                            throw new NotImplementedException(line);
                        }
                    }

                    return result;
                }

                var compute = Compute();
                Console.WriteLine(compute(targetPosition));
                //var valForZero = Compute()(0);

                //// magic number derived from excel magic
                //Func<long, long> compute = (i) =>
                //{
                //    {
                //        return (long)((((new BigInteger(valForZero) - i * 15432842991580)) % numCards + numCards) % numCards);
                //        //return ((((valForZero - i * 15432842991580)) % numCards + numCards) % numCards);
                //    }
                //};

                //var realCompute = Compute();

                //Console.WriteLine(compute(1970) == realCompute(1970));
                //Console.WriteLine(compute(12124) == realCompute(12124));
                //Console.WriteLine(compute(312341) == realCompute(312341));
                //Console.WriteLine(compute(1212434) == realCompute(1212434));
                //Console.WriteLine(compute(12124322) == realCompute(12124322));
                //Console.WriteLine(compute(121243344) == realCompute(121243344));
                //Console.WriteLine(compute(1212233324) == realCompute(1212233324));
                //Console.WriteLine(compute(34324534523) == realCompute(34324534523));
                //Console.WriteLine(compute(123412341234) == realCompute(123412341234));


                //var splitPoint = BinarySearch.GetMax(i => compute(i) == realCompute(i), 312341, 0, numCards);
                ////Console.WriteLine($"Split point: {splitPoint}");
                ////Console.WriteLine(compute(splitPoint - 1));
                ////Console.WriteLine(realCompute(splitPoint - 1));
                ////Console.WriteLine(compute(splitPoint));
                ////Console.WriteLine(realCompute(splitPoint));
                ////Console.WriteLine(compute(splitPoint + 1));
                ////Console.WriteLine(realCompute(splitPoint + 1));


                //for (long i = -60; i < 50; i++)
                //{
                //    Console.WriteLine($"{splitPoint + i}: {realCompute(splitPoint + i):00000000000000000}  {compute(splitPoint + i):00000000000000000}");
                //}


                //var compute = Compute();
                //var targetForMax = compute(numCards - 1);
                //Console.WriteLine(targetForMax);
                //compute = (i) =>
                //    ((targetForMax - (i - (numCards - 1)) * 15432842991580)) % 119315717514047;
                //var initialTarget = targetPosition;

                //for (long i = -60; i < 50; i++)
                //{
                //    Console.WriteLine($"{targetPosition + i}: {compute(targetPosition + i):00000000000000000}");

                //}

                ////Console.WriteLine(targetPosition);

                //if (initialTarget == targetPosition)
                //{
                //    Console.WriteLine($"Looped in {i}");
                //    return;
                //}

                //if (targetPosition < 10000000)
                //{

                //    Console.WriteLine($"Got to {targetPosition} in  {i}");
                //    return;
                //}

                //if (i % 10000 == 0)
                //{
                //    Console.WriteLine($"Loop number {i}");
                //}
                //for (long i = 0; i < 50; i++)
                //{
                //    Console.WriteLine($"{i:000}: {string.Join(" ", Enumerable.Range(11, 10).Select(i => $"{(targetPosition / i):000000000000000}"))}");
                //    targetPosition = Compute(targetPosition);
                //}


                ////var compute = Compute();
                //var initialTarget = targetPosition;

                //var loopTarget = 101741582076661;
                //////var loopTarget = 101741582076661 % 8828312 + 8828312 * 1;
                ////long loopTarget = 1234567 + 8828312;

                ////Console.WriteLine($"Target: {loopTarget}");
                ////targetPosition = initialTarget;
                ////for (long j = 0; j < loopTarget; j++)
                ////{
                ////    targetPosition = compute(targetPosition);
                ////}
                ////Console.WriteLine($"True result for {loopTarget}: {targetPosition}");
                ////targetPosition = initialTarget;

                //var seen = new Dictionary<long, long>();
                //for (long i = 1; ; i++)
                //{

                //    targetPosition = compute(targetPosition);
                //    //Console.WriteLine(targetPosition);

                //    if (initialTarget == targetPosition)
                //    {
                //        Console.WriteLine($"Looped in {i}");
                //        return;
                //    }
                //    //if (targetPosition < 10000000)
                //    //{
                //    //    Console.WriteLine($"Got to {targetPosition} in  {i}");
                //    //    return;
                //    //}

                //    if (i % 10000 == 0)
                //    {
                //        Console.WriteLine($"Loop number {i}");
                //    }


                //    if (seen.TryGetValue(targetPosition, out var v))
                //    {
                //        var loopLength = (i - v);
                //        Console.WriteLine($"Looped from {i} to {v} in {loopLength}");
                //        var toRun = (loopTarget - v) % loopLength + v;
                //        Console.WriteLine($"Simulating {toRun} times");


                //        //targetPosition = initialTarget;
                //        //for (long j = 0; j < toRun; j++)
                //        //{
                //        //    targetPosition = compute(targetPosition);
                //        //}

                //        //Console.WriteLine(targetPosition);
                //        Console.WriteLine(seen.Single(i => i.Value == toRun).Key);

                //        return;
                //    }


                //    seen[targetPosition] = i;
                //}
            });
        }


        //Loop number 0
        //Loop number 10000000
        //Looped from 11341386 to 2513075 in 8828311
        //Simulating 3291424 times
        //117669880996841
        // 117669880996841 is too high

        //Loop number 10000000
        //Looped from 11341387 to 2513075 in 8828312
        //Simulating 9423581 times
        //100492035067907
        //your answer is too high

        //85821781804505 is wrong
        //var toRun = (101741582076661) % loopLength;

        public override void Run()
        {

            //            foreach (var i in Enumerable.Range(0, 10))
            //            {
            //                RunScenario($"dealnewstack-{i}", @"deal into new stack", 10, i);
            //            }

            //            foreach (var i in Enumerable.Range(0, 10))
            //            {
            //                RunScenario($"dealnewstacktwice-{i}", @"deal into new stack
            //deal into new stack", 10, i);
            //            }

            //            return;

            //foreach (var i in Enumerable.Range(0, 10))
            //{
            //    RunScenario($"deal-increment-3-{i}", @"deal with increment 3", 10, i);
            //}
            //foreach (var i in Enumerable.Range(0, 10))
            //{
            //    RunScenario($"deal-increment-7-{i}", @"deal with increment 7", 10, i);
            //}
            //foreach (var i in Enumerable.Range(0, 10))
            //{
            //    RunScenario($"deal-increment-9-{i}", @"deal with increment 9", 10, i);
            //}

            //foreach (var j in Enumerable.Range(2, 9))
            //foreach (var i in Enumerable.Range(0, 11))
            //{
            //    RunScenario($"deal-increment-{j}-{i}", $"deal with increment {j}", 11, i);
            //}

            //return;

            //foreach (var i in Enumerable.Range(0, 10))
            //{
            //    RunScenario($"cut-minus4-{i}", @"cut -4", 10, i);
            //}

            //foreach (var i in Enumerable.Range(0, 10))
            //{
            //    RunScenario($"cut6-{i}", @"cut 6", 10, i);
            //}

            //return;

//            foreach (var i in Enumerable.Range(0, 10))
//            {
//                RunScenario($"initial-{i}", @"deal with increment 7
//deal into new stack
//deal into new stack", 10, i);

//            }
//            foreach (var i in Enumerable.Range(0, 10))
//            {
//                RunScenario($"initial-{i}", @"deal into new stack
//cut -2
//deal with increment 7
//cut 8
//cut -4
//deal with increment 7
//cut 3
//deal with increment 9
//deal with increment 3
//cut -1", 10, i);

//            }


//            RunScenario("part1", @"cut -1353
//deal with increment 63
//cut -716
//deal with increment 55
//cut 1364
//deal with increment 61
//cut 1723
//deal into new stack
//deal with increment 51
//cut 11
//deal with increment 65
//cut -6297
//deal with increment 69
//cut -3560
//deal with increment 20
//cut 1177
//deal with increment 29
//cut 6033
//deal with increment 3
//cut -3564
//deal into new stack
//cut 6447
//deal into new stack
//cut -4030
//deal with increment 3
//cut -6511
//deal with increment 42
//cut -8748
//deal with increment 38
//cut 5816
//deal with increment 73
//cut 9892
//deal with increment 16
//cut -9815
//deal with increment 10
//cut 673
//deal with increment 12
//cut 4518
//deal with increment 52
//cut 9464
//deal with increment 68
//cut 902
//deal with increment 11
//deal into new stack
//deal with increment 45
//cut -5167
//deal with increment 68
//deal into new stack
//deal with increment 24
//cut -8945
//deal into new stack
//deal with increment 36
//cut 3195
//deal with increment 52
//cut -1494
//deal with increment 11
//cut -9658
//deal into new stack
//cut -4689
//deal with increment 34
//cut -9697
//deal with increment 39
//cut -6857
//deal with increment 19
//cut -6790
//deal with increment 59
//deal into new stack
//deal with increment 52
//cut -9354
//deal with increment 71
//cut 8815
//deal with increment 2
//cut 6618
//deal with increment 47
//cut -6746
//deal into new stack
//cut 1336
//deal with increment 53
//cut 6655
//deal with increment 17
//cut 8941
//deal with increment 25
//cut -3046
//deal with increment 14
//cut -7818
//deal with increment 25
//cut 4140
//deal with increment 60
//cut 6459
//deal with increment 27
//cut -6791
//deal into new stack
//cut 3821
//deal with increment 13
//cut 3157
//deal with increment 13
//cut 8524
//deal into new stack
//deal with increment 12
//cut 5944", 10007, 3589);
            return;
            //return;
            //return;
            RunScenario("part1", @"cut -1353
deal with increment 63
cut -716
deal with increment 55
cut 1364
deal with increment 61
cut 1723
deal into new stack
deal with increment 51
cut 11
deal with increment 65
cut -6297
deal with increment 69
cut -3560
deal with increment 20
cut 1177
deal with increment 29
cut 6033
deal with increment 3
cut -3564
deal into new stack
cut 6447
deal into new stack
cut -4030
deal with increment 3
cut -6511
deal with increment 42
cut -8748
deal with increment 38
cut 5816
deal with increment 73
cut 9892
deal with increment 16
cut -9815
deal with increment 10
cut 673
deal with increment 12
cut 4518
deal with increment 52
cut 9464
deal with increment 68
cut 902
deal with increment 11
deal into new stack
deal with increment 45
cut -5167
deal with increment 68
deal into new stack
deal with increment 24
cut -8945
deal into new stack
deal with increment 36
cut 3195
deal with increment 52
cut -1494
deal with increment 11
cut -9658
deal into new stack
cut -4689
deal with increment 34
cut -9697
deal with increment 39
cut -6857
deal with increment 19
cut -6790
deal with increment 59
deal into new stack
deal with increment 52
cut -9354
deal with increment 71
cut 8815
deal with increment 2
cut 6618
deal with increment 47
cut -6746
deal into new stack
cut 1336
deal with increment 53
cut 6655
deal with increment 17
cut 8941
deal with increment 25
cut -3046
deal with increment 14
cut -7818
deal with increment 25
cut 4140
deal with increment 60
cut 6459
deal with increment 27
cut -6791
deal into new stack
cut 3821
deal with increment 13
cut 3157
deal with increment 13
cut 8524
deal into new stack
deal with increment 12
cut 5944", 119315717514047, 2020);

        }
    }
}
