using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AOC.Year2019.Day15
{
    public class IntCodeSimulationState
    {
        private readonly long _ip;
        private readonly long _relativeBase;
        private readonly List<long> _output;
        private readonly bool _halted;
        private readonly IDictionary<int, long> _data;

        public List<long> Output => _output;
        public bool Halted => _halted;

        // starts a new program and runs it until it needs it's first input
        public static IntCodeSimulationState Start(IDictionary<int, long> data)
        {
            return new IntCodeSimulationState(data, 0, 0, new List<long>(), false).ResumeInternal(null);
        }

        private IntCodeSimulationState(IDictionary<int, long> data, long ip, long relativeBase, List<long> output, bool halted)
        {
            _data = data;
            _ip = ip;
            _relativeBase = relativeBase;
            _output = output;
            _halted = halted;
        }

        public IntCodeSimulationState Resume(long input)
        {
            return ResumeInternal(input);
        }

        private IntCodeSimulationState ResumeInternal(long? input)
        {
            if (this._halted)
            {
                throw new NotImplementedException("Cannot resume a halted program");
            }

            var data = _data.ToDictionary(i => i.Key, i => i.Value);
            long ip = _ip;
            long relativeBase = _relativeBase;
            bool halted = false;
            var output = new List<long>();

            long GetValue(long dataIx)
            {
                return data.TryGetValue((int)dataIx, out var v) ? v : 0;
            }

            while (true)
            {
                var i = GetValue(ip);

                long GetParamIx(long pNum)
                {
                    var pMode = i / (10 * (long)Math.Pow(10, pNum)) % 10;
                    var pix = pMode == 0 ? GetValue(ip + pNum) : pMode == 1 ? ip + pNum : relativeBase + GetValue(ip + pNum);
                    //Console.WriteLine($"PMode: {pMode} for {pNum} -> {pix}");
                    return pix;
                }

                long GetParam(long pNum)
                {
                    return GetValue(GetParamIx(pNum));
                }
                void SetByParam(long pNum, long value)
                {
                    data[(int)GetParamIx(pNum)] = value;
                }

                //Console.WriteLine($"Running opcode {i} @ {ip}");
                //Console.WriteLine(string.Join(",", data));
                //Console.WriteLine($"Relative base: {relativeBase}");
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
                        if (!input.HasValue)
                        {
                            goto done;
                        }
                        SetByParam(1, input.Value);
                        input = null; // next time we need input, we have to stop and wait
                        ip += 2;
                        break;
                    case 4:
                        var oval = GetParam(1);
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
                    case 9:
                        relativeBase = relativeBase + GetParam(1);
                        ip += 2;
                        break;
                    case 99:
                        //Console.WriteLine($"Terminating");
                        ip += 1;
                        halted = true; 
                        goto done;
                    default:
                        throw new NotImplementedException();
                }

            }
            done:

            return new IntCodeSimulationState(data, ip, relativeBase, output, halted);
        }
    }
}
