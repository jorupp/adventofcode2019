using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AoC.Year2019.Day7
{
    public class Part1 : BasePart
    {
        protected void RunScenario(string title, string input)
        {
            RunScenario(title, () =>
            {
                var lines = input.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                var initialData = lines[0].Split(',').Select(int.Parse).ToArray();

                var count = 5;
                var outputs = new List<int>();

                for (var i1 = 0; i1 < count; i1++)
                {
                    for (var i2 = 0; i2 < count; i2++)
                    {
                        for (var i3 = 0; i3 < count; i3++)
                        {
                            for (var i4 = 0; i4 < count; i4++)
                            {
                                for (var i5 = 0; i5 < count; i5++)
                                {
                                    var settings = new[] {i1, i2, i3, i4, i5};
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

                Console.WriteLine(outputs.Max());
            });
        }

        private int RunThrusterChain(int[] program, int[] phaseSettings)
        {
            var inputSignal = 0;

            //Console.WriteLine($"Running thruster chain {string.Join(",", phaseSettings)} on {string.Join(",", program)}");

            while (phaseSettings.Length > 0)
            {
                //Console.WriteLine($"Running chain part {string.Join(",", phaseSettings)} with {inputSignal}");
                Simulate(program.ToList().ToArray(), new [] { phaseSettings[0], inputSignal }, out var output);
                if (output.Count != 1)
                {
                    throw new InvalidOperationException();
                }
                inputSignal = output[0];
                phaseSettings = phaseSettings.Skip(1).ToArray();
            }

            return inputSignal;
        }

        private async Task RunProgram(int[] program)
        {

        }

        private void Simulate(int[] data, int[] input, out List<int> output)
        {
            var ip = 0;
            output = new List<int>();

            while (true)
            {
                var i = data[ip];

                int GetParam(int pNum)
                {
                    var pMode = i / (10 * (int)Math.Pow(10, pNum)) % 10;
                    return data[pMode == 0 ? data[ip + pNum] : ip + pNum];
                }
                void SetByParam(int pNum, int value)
                {
                    // don't really need to support parameter mode here, but we will
                    var pMode = i / (10 * (int)Math.Pow(10, pNum)) % 10;
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
                        SetByParam(1, input[0]);
                        input = input.Skip(1).ToArray();
                        ip += 2;
                        break;
                    case 4:
                        var oval = GetParam(1);
                        output.Add(oval);
                        //Console.WriteLine($"Writing output {oval} from {ip} in {string.Join(",", data)}");
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
                        ip += 1;
                        return;
                    default:
                        throw new NotImplementedException();
                }
            }
        }
        public override void Run()
        {
            RunScenario("initial-1", @"3,15,3,16,1002,16,10,16,1,16,15,15,4,15,99,0,0");
            RunScenario("initial-2", @"3,23,3,24,1002,24,10,24,1002,23,-1,23,101,5,23,23,1,24,23,23,4,23,99,0,0");
            RunScenario("initial-3", @"3,31,3,32,1002,32,10,32,1001,31,-2,31,1007,31,0,33,1002,33,7,33,1,33,31,31,1,32,31,31,4,31,99,0,0,0");
            //return;
            RunScenario("part1", @"3,8,1001,8,10,8,105,1,0,0,21,38,55,80,97,118,199,280,361,442,99999,3,9,101,2,9,9,1002,9,5,9,1001,9,4,9,4,9,99,3,9,101,5,9,9,102,2,9,9,1001,9,5,9,4,9,99,3,9,1001,9,4,9,102,5,9,9,101,4,9,9,102,4,9,9,1001,9,4,9,4,9,99,3,9,1001,9,3,9,1002,9,2,9,101,3,9,9,4,9,99,3,9,101,5,9,9,1002,9,2,9,101,3,9,9,1002,9,5,9,4,9,99,3,9,1002,9,2,9,4,9,3,9,101,1,9,9,4,9,3,9,1002,9,2,9,4,9,3,9,101,1,9,9,4,9,3,9,1001,9,2,9,4,9,3,9,102,2,9,9,4,9,3,9,1002,9,2,9,4,9,3,9,1002,9,2,9,4,9,3,9,101,2,9,9,4,9,3,9,1002,9,2,9,4,9,99,3,9,102,2,9,9,4,9,3,9,1002,9,2,9,4,9,3,9,101,2,9,9,4,9,3,9,102,2,9,9,4,9,3,9,1002,9,2,9,4,9,3,9,101,1,9,9,4,9,3,9,101,2,9,9,4,9,3,9,102,2,9,9,4,9,3,9,1002,9,2,9,4,9,3,9,102,2,9,9,4,9,99,3,9,102,2,9,9,4,9,3,9,1002,9,2,9,4,9,3,9,102,2,9,9,4,9,3,9,102,2,9,9,4,9,3,9,1001,9,2,9,4,9,3,9,101,2,9,9,4,9,3,9,101,1,9,9,4,9,3,9,101,2,9,9,4,9,3,9,1001,9,2,9,4,9,3,9,102,2,9,9,4,9,99,3,9,1002,9,2,9,4,9,3,9,101,2,9,9,4,9,3,9,1001,9,2,9,4,9,3,9,102,2,9,9,4,9,3,9,102,2,9,9,4,9,3,9,1001,9,1,9,4,9,3,9,101,2,9,9,4,9,3,9,102,2,9,9,4,9,3,9,101,2,9,9,4,9,3,9,1001,9,1,9,4,9,99,3,9,102,2,9,9,4,9,3,9,101,1,9,9,4,9,3,9,1002,9,2,9,4,9,3,9,101,1,9,9,4,9,3,9,1001,9,2,9,4,9,3,9,1002,9,2,9,4,9,3,9,1002,9,2,9,4,9,3,9,1001,9,2,9,4,9,3,9,1001,9,1,9,4,9,3,9,102,2,9,9,4,9,99");

        }
    }
}
