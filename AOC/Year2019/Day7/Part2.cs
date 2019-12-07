using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AoC.Year2019.Day7
{
    public class Part2 : BasePart
    {
        protected void RunScenario(string title, string input)
        {
            RunScenario(title, () =>
            {
                var lines = input.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                var initialData = lines[0].Split(',').Select(long.Parse).ToArray();

                var count = 10;
                var outputs = new List<long>();

                for (long i1 = 5; i1 < count; i1++)
                {
                    for (long i2 = 5; i2 < count; i2++)
                    {
                        for (long i3 = 5; i3 < count; i3++)
                        {
                            for (long i4 = 5; i4 < count; i4++)
                            {
                                for (long i5 = 5; i5 < count; i5++)
                                {
                                    var settings = new[] { i1, i2, i3, i4, i5 };
                                    if (settings.Distinct().Count() != 5)
                                    {
                                        continue;
                                    }

                                    outputs.Add(RunThrusterChain(initialData, settings));
                                }
                            }
                        }
                    }
                }

                //outputs.Add(RunThrusterChain(initialData.ToList().ToArray(), new long[] { 5, 6, 7, 8, 9 }));
                //outputs.Add(RunThrusterChain(initialData.ToList().ToArray(), new long[] { 9, 8, 7, 6, 5 }));
                //outputs.Add(RunThrusterChain(lines[0].Split(',').Select(long.Parse).ToArray(), new long[] { 9, 8, 7, 6, 5 }));
                //outputs.Add(RunThrusterChain(lines[0].Split(',').Select(long.Parse).ToArray(), new long[] { 9, 8, 7, 6, 5 }));
                //outputs.Add(RunThrusterChain(initialData.ToList().ToArray(), new long[] { 9, 8, 7, 6, 5 }));

                Console.WriteLine(outputs.Max());
            });
        }

        private long RunThrusterChain(long[] program, long[] phaseSettings)
        {
            var collections = phaseSettings.Select(i => new BlockingCollection<long>()).ToList();
            collections.AddRange(collections.ToArray());

            //Console.WriteLine($"Starting with {collections.Count} and {string.Join(",", program)}");

            for (var i = 0; i < phaseSettings.Length; i++)
            {
                collections[i].Add(phaseSettings[i]);
                if (i == 0)
                {
                    collections[i].Add(0);
                }
            }

            var threads = phaseSettings.Select((i, ix) =>
                new Thread((object _) =>
                {
                    int index = (int) _;
                    //Console.WriteLine($"Starting {index}");
                    Simulate(program.ToList().ToArray(), collections[index], collections[index + 1], index);
                })).ToList();

            //Console.WriteLine($"Starting threads {threads.Count}");
            var ixx = 0;
            foreach (var t in threads)
            {
                t.Start(ixx++);
            }

            //Console.WriteLine("Joining threads");
            foreach (var t in threads)
            {
                t.Join();
            }

            //Console.WriteLine("Joined threads");

            var output = collections[0].Take();

            Console.WriteLine($"Ran thruster chain {string.Join(",", phaseSettings)} and got {output}");

            return output;
        }


        private void Simulate(long[] data, BlockingCollection<long> input, BlockingCollection<long> output, int ix)
        {
            long ip = 0;

            while (true)
            {
                var i = data[ip];

                long GetParam(long pNum)
                {
                    var pMode = i / (10 * (long)Math.Pow(10, pNum)) % 10;
                    return data[pMode == 0 ? data[ip + pNum] : ip + pNum];
                }
                void SetByParam(long pNum, long value)
                {
                    // don't really need to support parameter mode here, but we will
                    var pMode = i / (10 * (long)Math.Pow(10, pNum)) % 10;
                    data[pMode == 0 ? data[ip + pNum] : ip + pNum] = value;
                }

                //Console.WriteLine($"Running opcode {i % 100}");
                switch (i % 100)
                {
                    case 1:
                        SetByParam(3, GetParam(1) + GetParam(2));
                        ip += 4;
                        break;
                    case 2:
                        SetByParam(3, GetParam(1) * GetParam(2));
                        ip += 4;
                        break;
                    case 3:
                        SetByParam(1, input.Take());
                        ip += 2;
                        break;
                    case 4:
                        var oval = GetParam(1);
                        //Console.WriteLine($"Writing output {oval} in {ix}");
                        output.Add(oval);
                        ip += 2;
                        break;
                    case 5:
                        if (GetParam(1) != 0)
                        {
                            ip = GetParam(2);
                        }
                        else
                        {
                            ip += 3;
                        }
                        break;
                    case 6:
                        if (GetParam(1) == 0)
                        {
                            ip = GetParam(2);
                        }
                        else
                        {
                            ip += 3;
                        }
                        break;
                    case 7:
                        SetByParam(3, GetParam(1) < GetParam(2) ? 1 : 0);
                        ip += 4;
                        break;
                    case 8:
                        SetByParam(3, GetParam(1) == GetParam(2) ? 1 : 0);
                        ip += 4;
                        break;
                    case 99:
                        //Console.WriteLine($"Terminating {ix}");
                        ip += 1;
                        return;
                    default:
                        throw new NotImplementedException();
                }
            }
        }
        public override void Run()
        {
            RunScenario("initial-1", @"3,26,1001,26,-4,26,3,27,1002,27,2,27,1,27,26,27,4,27,1001,28,-1,28,1005,28,6,99,0,0,5");
            RunScenario("initial-2", @"3,52,1001,52,-5,52,3,53,1,52,56,54,1007,54,5,55,1005,55,26,1001,54,-5,54,1105,1,12,1,53,54,53,1008,54,0,55,1001,55,1,55,2,53,55,53,4,53,1001,56,-1,56,1005,56,6,99,0,0,0,0,10");
            //return;
            RunScenario("part1", @"3,8,1001,8,10,8,105,1,0,0,21,38,55,80,97,118,199,280,361,442,99999,3,9,101,2,9,9,1002,9,5,9,1001,9,4,9,4,9,99,3,9,101,5,9,9,102,2,9,9,1001,9,5,9,4,9,99,3,9,1001,9,4,9,102,5,9,9,101,4,9,9,102,4,9,9,1001,9,4,9,4,9,99,3,9,1001,9,3,9,1002,9,2,9,101,3,9,9,4,9,99,3,9,101,5,9,9,1002,9,2,9,101,3,9,9,1002,9,5,9,4,9,99,3,9,1002,9,2,9,4,9,3,9,101,1,9,9,4,9,3,9,1002,9,2,9,4,9,3,9,101,1,9,9,4,9,3,9,1001,9,2,9,4,9,3,9,102,2,9,9,4,9,3,9,1002,9,2,9,4,9,3,9,1002,9,2,9,4,9,3,9,101,2,9,9,4,9,3,9,1002,9,2,9,4,9,99,3,9,102,2,9,9,4,9,3,9,1002,9,2,9,4,9,3,9,101,2,9,9,4,9,3,9,102,2,9,9,4,9,3,9,1002,9,2,9,4,9,3,9,101,1,9,9,4,9,3,9,101,2,9,9,4,9,3,9,102,2,9,9,4,9,3,9,1002,9,2,9,4,9,3,9,102,2,9,9,4,9,99,3,9,102,2,9,9,4,9,3,9,1002,9,2,9,4,9,3,9,102,2,9,9,4,9,3,9,102,2,9,9,4,9,3,9,1001,9,2,9,4,9,3,9,101,2,9,9,4,9,3,9,101,1,9,9,4,9,3,9,101,2,9,9,4,9,3,9,1001,9,2,9,4,9,3,9,102,2,9,9,4,9,99,3,9,1002,9,2,9,4,9,3,9,101,2,9,9,4,9,3,9,1001,9,2,9,4,9,3,9,102,2,9,9,4,9,3,9,102,2,9,9,4,9,3,9,1001,9,1,9,4,9,3,9,101,2,9,9,4,9,3,9,102,2,9,9,4,9,3,9,101,2,9,9,4,9,3,9,1001,9,1,9,4,9,99,3,9,102,2,9,9,4,9,3,9,101,1,9,9,4,9,3,9,1002,9,2,9,4,9,3,9,101,1,9,9,4,9,3,9,1001,9,2,9,4,9,3,9,1002,9,2,9,4,9,3,9,1002,9,2,9,4,9,3,9,1001,9,2,9,4,9,3,9,1001,9,1,9,4,9,3,9,102,2,9,9,4,9,99");

        }
    }
}
