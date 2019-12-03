using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AOC.Year2019
{
    public class Simulator
    {
        private readonly Dictionary<int, Instruction> _instructions;

        public Simulator(Dictionary<int, Instruction> instructions)
        {
            _instructions = instructions;
        }

        public void Execute(int[] data)
        {
            var ip = 0;

            while (true)
            {
                var i = data[ip];
                if (_instructions.TryGetValue(i, out var instruction))
                {
                    var done = instruction.Action(data, data.Skip(ip + 1).Take(instruction.ArgumentCount).ToArray());
                    ip += 1 + instruction.ArgumentCount;

                    if (done)
                    {
                        return;
                    }
                }
                else
                {
                    throw new NotImplementedException($"Opcode {i} is not implemented");
                }
            }
        }
    }

    public class Instruction
    {
        public int ArgumentCount { get; }
        public Func<int[], int[], bool> Action { get; }

        public Instruction(int argumentCount, Func<int[], int[], bool> action)
        {
            ArgumentCount = argumentCount;
            Action = action;
        }
    }
}
