using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;

namespace AoC.Year2018.Day16
{
    public class Part2 : BasePart
    {
        protected void RunScenario(string title, string input)
        {
            RunScenario(title, () =>
            {
                var lines = input.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries).Select(i => i.Numbers()).ToArray();
                var commands = new[] { "gtrr", "borr", "gtir", "eqri", "addr", "seti", "eqrr", "gtri", "banr", "addi", "setr", "mulr", "bori", "muli", "eqir", "bani" };
                var registers = Enumerable.Repeat(0, 4).ToArray();

                foreach (var line in lines)
                {
                    var command = commands[line[0]];
                    var arguments = line.Skip(1).ToArray();

                    var newRegisters = Execute(registers, command, arguments);

                    Console.WriteLine($"{string.Join(" ", registers)} + {command} {string.Join(" ", arguments)} => {string.Join(" ", newRegisters)}");

                    registers = newRegisters;
                }

                Console.WriteLine(registers[0]);
            });
        }

        private IEnumerable<string[]> GetInputs(string[] lines)
        {
            var parts = new List<string>();
            foreach (var x in lines)
            {
                if (parts.Count == 3)
                {
                    yield return parts.ToArray();
                    parts = new List<string>();
                }

                parts.Add(x);
            }

            yield return parts.ToArray();
        }

        private bool AreEqual(int[] i1, int[] i2)
        {
            return i1.Length == i2.Length && i1.Zip(i2, (i, ii) => i == ii).All(i => i);
        }

        private int[] Execute(int[] input, string command, int[] arguments)
        {
            var registers = input.ToArray();
            var a = arguments[0];
            var b = arguments[1];
            var c = arguments[2];
            switch (command)
            {
                //Addition:
                //addr (add register) stores into register C the result of adding register A and register B.
                case "addr":
                    registers[c] = registers[a] + registers[b];
                    break;
                //addi (add immediate) stores into register C the result of adding register A and value B.
                case "addi":
                    registers[c] = registers[a] + b;
                    break;

                //Multiplication:
                //mulr (multiply register) stores into register C the result of multiplying register A and register B.
                case "mulr":
                    registers[c] = registers[a] * registers[b];
                    break;
                //muli (multiply immediate) stores into register C the result of multiplying register A and value B.
                case "muli":
                    registers[c] = registers[a] * b;
                    break;

                //Bitwise AND:
                //banr (bitwise AND register) stores into register C the result of the bitwise AND of register A and register B.
                case "banr":
                    registers[c] = registers[a] & registers[b];
                    break;
                //bani (bitwise AND immediate) stores into register C the result of the bitwise AND of register A and value B.
                case "bani":
                    registers[c] = registers[a] & b;
                    break;

                //Bitwise OR:
                //borr (bitwise OR register) stores into register C the result of the bitwise OR of register A and register B.
                case "borr":
                    registers[c] = registers[a] | registers[b];
                    break;
                //bori (bitwise OR immediate) stores into register C the result of the bitwise OR of register A and value B.
                case "bori":
                    registers[c] = registers[a] | b;
                    break;

                //Assignment:
                //setr (set register) copies the contents of register A into register C. (Input B is ignored.)
                case "setr":
                    registers[c] = registers[a];
                    break;
                //seti (set immediate) stores value A into register C. (Input B is ignored.)
                case "seti":
                    registers[c] = a;
                    break;

                //Greater-than testing:
                //gtir (greater-than immediate/register) sets register C to 1 if value A is greater than register B. Otherwise, register C is set to 0.
                case "gtir":
                    registers[c] = a > registers[b] ? 1 : 0;
                    break;
                //gtri (greater-than register/immediate) sets register C to 1 if register A is greater than value B. Otherwise, register C is set to 0.
                case "gtri":
                    registers[c] = registers[a] > b ? 1 : 0;
                    break;
                //gtrr (greater-than register/register) sets register C to 1 if register A is greater than register B. Otherwise, register C is set to 0.
                case "gtrr":
                    registers[c] = registers[a] > registers[b] ? 1 : 0;
                    break;

                //Equality testing:
                //eqir (equal immediate/register) sets register C to 1 if value A is equal to register B. Otherwise, register C is set to 0.
                case "eqir":
                    registers[c] = a == registers[b] ? 1 : 0;
                    break;
                //eqri (equal register/immediate) sets register C to 1 if register A is equal to value B. Otherwise, register C is set to 0.
                case "eqri":
                    registers[c] = registers[a] == b ? 1 : 0;
                    break;
                //eqrr (equal register/register) sets register C to 1 if register A is equal to register B. Otherwise, register C is set to 0.
                case "eqrr":
                    registers[c] = registers[a] == registers[b] ? 1 : 0;
                    break;

                default:
                    throw new Exception($"Invalid command: {command}");
            }

            return registers;
        }

        public override void Run()
        {
            RunScenario("part1", @"5 1 3 3
5 3 2 2
5 2 1 0
3 0 2 3
13 3 2 3
13 3 3 3
4 1 3 1
10 1 0 2
5 1 2 1
13 0 0 3
9 3 1 3
11 1 0 3
13 3 1 3
13 3 3 3
4 2 3 2
10 2 1 0
5 2 1 1
13 2 0 2
9 2 1 2
5 3 2 3
15 3 2 2
13 2 1 2
4 2 0 0
10 0 3 2
13 1 0 3
9 3 2 3
5 2 2 0
6 0 3 3
13 3 2 3
4 3 2 2
5 2 0 3
5 3 2 1
6 0 3 3
13 3 3 3
13 3 2 3
4 3 2 2
13 2 0 3
9 3 2 3
5 2 0 1
6 0 3 0
13 0 2 0
4 2 0 2
5 2 2 0
5 1 1 1
6 0 3 1
13 1 3 1
13 1 3 1
4 1 2 2
10 2 3 0
5 0 0 3
13 1 0 2
9 2 2 2
5 1 1 1
2 3 2 1
13 1 2 1
4 1 0 0
5 3 0 1
7 2 1 1
13 1 1 1
4 0 1 0
10 0 1 2
5 2 0 3
5 0 0 1
5 2 3 0
6 0 3 3
13 3 2 3
4 3 2 2
10 2 2 3
13 2 0 2
9 2 3 2
5 1 0 1
11 1 0 0
13 0 3 0
4 0 3 3
10 3 2 0
13 0 0 3
9 3 2 3
5 0 1 1
5 0 2 2
14 2 3 1
13 1 1 1
4 1 0 0
5 0 0 3
5 1 1 2
13 1 0 1
9 1 2 1
5 2 3 1
13 1 3 1
4 1 0 0
5 3 0 1
13 1 0 2
9 2 3 2
14 3 2 2
13 2 1 2
13 2 1 2
4 0 2 0
10 0 1 3
13 0 0 0
9 0 2 0
5 0 2 1
5 3 3 2
1 0 2 1
13 1 2 1
4 1 3 3
10 3 1 1
5 0 1 3
5 1 1 0
14 3 2 2
13 2 2 2
13 2 2 2
4 1 2 1
5 3 0 0
5 3 2 2
5 2 0 0
13 0 3 0
4 0 1 1
10 1 3 2
5 0 3 0
5 1 1 1
5 2 3 3
11 1 3 3
13 3 3 3
4 3 2 2
13 1 0 0
9 0 2 0
5 0 1 3
11 1 0 3
13 3 1 3
4 3 2 2
10 2 3 3
5 0 1 1
5 3 3 0
5 2 3 2
7 2 0 0
13 0 3 0
4 3 0 3
10 3 3 0
5 1 0 3
5 0 1 2
5 1 2 1
5 3 1 3
13 3 3 3
13 3 2 3
4 0 3 0
10 0 0 3
5 2 1 2
13 2 0 0
9 0 1 0
5 2 2 1
10 0 2 0
13 0 1 0
4 3 0 3
5 3 2 1
13 2 0 0
9 0 1 0
7 2 1 2
13 2 3 2
4 3 2 3
10 3 0 1
5 2 3 0
5 2 0 3
5 1 0 2
6 0 3 0
13 0 2 0
4 0 1 1
10 1 2 3
5 3 3 2
5 2 3 0
5 0 3 1
3 0 2 0
13 0 1 0
13 0 2 0
4 0 3 3
10 3 3 2
5 0 2 3
5 2 3 1
5 3 3 0
1 1 0 0
13 0 3 0
13 0 2 0
4 0 2 2
13 0 0 3
9 3 1 3
5 2 1 0
13 3 0 1
9 1 3 1
9 3 1 1
13 1 3 1
4 1 2 2
5 1 1 0
5 2 2 3
5 2 2 1
11 0 3 0
13 0 2 0
13 0 1 0
4 0 2 2
10 2 2 0
13 1 0 2
9 2 0 2
14 2 3 3
13 3 3 3
4 3 0 0
10 0 2 3
13 1 0 0
9 0 1 0
5 2 0 2
13 2 3 2
13 2 2 2
4 3 2 3
10 3 2 0
5 3 1 1
5 0 2 3
5 2 2 2
2 3 2 3
13 3 1 3
4 0 3 0
10 0 0 1
5 2 0 0
13 0 0 2
9 2 3 2
5 1 2 3
11 3 0 0
13 0 1 0
4 0 1 1
10 1 1 3
5 1 1 0
13 0 0 1
9 1 3 1
9 0 1 0
13 0 1 0
4 3 0 3
10 3 0 2
5 2 1 0
5 1 1 3
4 3 3 1
13 1 2 1
4 2 1 2
10 2 1 1
5 2 3 2
5 0 1 3
2 3 2 3
13 3 2 3
4 3 1 1
5 2 0 3
5 3 2 2
6 0 3 0
13 0 2 0
4 0 1 1
10 1 2 0
5 0 3 2
5 0 0 1
5 2 1 1
13 1 2 1
13 1 1 1
4 1 0 0
10 0 0 1
13 2 0 0
9 0 2 0
5 2 3 2
6 0 3 0
13 0 3 0
4 0 1 1
10 1 0 2
13 2 0 1
9 1 3 1
5 3 1 0
8 1 3 3
13 3 1 3
13 3 2 3
4 2 3 2
10 2 2 1
5 1 2 3
5 3 0 2
15 0 2 3
13 3 2 3
4 3 1 1
10 1 3 2
5 0 1 0
13 1 0 1
9 1 3 1
5 2 2 3
8 1 3 3
13 3 3 3
13 3 1 3
4 3 2 2
10 2 0 3
13 3 0 0
9 0 1 0
5 1 1 2
4 0 0 1
13 1 1 1
4 3 1 3
10 3 1 0
5 0 3 2
13 1 0 3
9 3 1 3
5 3 2 1
15 1 2 3
13 3 3 3
13 3 1 3
4 3 0 0
10 0 3 1
5 2 1 0
5 2 3 3
6 0 3 2
13 2 1 2
4 1 2 1
10 1 1 0
5 0 3 1
13 2 0 3
9 3 0 3
5 2 3 2
2 3 2 3
13 3 3 3
4 0 3 0
5 2 2 1
5 0 2 3
2 3 2 1
13 1 2 1
4 1 0 0
10 0 1 1
5 3 0 2
5 2 1 0
3 0 2 0
13 0 2 0
13 0 2 0
4 0 1 1
10 1 2 0
5 2 2 2
5 2 1 1
5 3 1 1
13 1 2 1
4 1 0 0
10 0 3 1
5 3 2 2
5 2 1 0
5 3 3 3
1 0 2 2
13 2 3 2
13 2 3 2
4 2 1 1
10 1 1 0
5 0 1 3
5 3 2 2
5 2 1 1
14 3 2 2
13 2 2 2
13 2 2 2
4 0 2 0
10 0 2 1
5 2 2 2
5 3 3 0
2 3 2 2
13 2 3 2
4 1 2 1
10 1 1 0
5 0 0 1
5 0 2 2
5 1 3 3
4 3 3 1
13 1 1 1
13 1 1 1
4 0 1 0
10 0 2 2
13 3 0 1
9 1 2 1
5 1 1 0
5 3 1 3
4 0 0 3
13 3 2 3
13 3 3 3
4 2 3 2
10 2 2 3
5 0 3 2
5 2 2 0
5 3 0 1
8 1 0 0
13 0 3 0
4 3 0 3
10 3 0 0
5 1 0 3
5 2 0 2
7 2 1 1
13 1 1 1
4 1 0 0
10 0 3 1
13 0 0 0
9 0 2 0
0 0 3 3
13 3 2 3
4 1 3 1
5 0 0 2
5 2 3 3
6 0 3 0
13 0 2 0
4 1 0 1
5 2 2 2
5 3 0 0
13 3 0 3
9 3 3 3
7 2 0 3
13 3 1 3
4 3 1 1
10 1 0 2
5 1 2 1
5 2 2 0
5 2 1 3
6 0 3 0
13 0 2 0
4 2 0 2
10 2 1 1
5 1 2 2
5 3 2 3
13 3 0 0
9 0 0 0
15 3 2 3
13 3 2 3
4 1 3 1
10 1 2 3
5 3 1 0
5 3 2 1
13 2 0 2
9 2 2 2
1 2 0 0
13 0 1 0
13 0 1 0
4 0 3 3
10 3 2 2
13 1 0 1
9 1 2 1
13 2 0 3
9 3 3 3
5 3 1 0
8 0 1 1
13 1 1 1
4 1 2 2
10 2 3 1
5 0 2 3
13 1 0 0
9 0 2 0
5 3 2 2
3 0 2 3
13 3 2 3
13 3 2 3
4 1 3 1
10 1 0 0
5 0 1 1
5 2 2 2
5 0 1 3
2 3 2 2
13 2 1 2
4 0 2 0
10 0 1 3
5 3 3 0
5 1 2 1
5 0 0 2
3 2 0 1
13 1 1 1
13 1 2 1
4 1 3 3
10 3 0 1
13 0 0 3
9 3 3 3
5 1 1 0
5 2 0 2
10 0 2 0
13 0 2 0
13 0 3 0
4 1 0 1
10 1 3 2
5 2 2 3
5 2 0 0
5 0 2 1
6 0 3 1
13 1 3 1
4 2 1 2
5 0 3 0
5 0 3 3
5 2 0 1
12 1 3 1
13 1 1 1
4 1 2 2
10 2 0 1
5 2 1 3
13 0 0 0
9 0 1 0
5 0 0 2
14 2 3 0
13 0 3 0
4 0 1 1
5 1 3 3
13 2 0 0
9 0 1 0
13 3 2 0
13 0 3 0
4 0 1 1
5 2 2 0
11 3 0 3
13 3 3 3
4 1 3 1
10 1 0 3
5 3 0 1
5 3 2 0
3 2 0 1
13 1 2 1
13 1 3 1
4 1 3 3
10 3 1 0
5 0 1 1
5 0 2 3
5 3 3 2
14 3 2 3
13 3 2 3
4 0 3 0
10 0 2 3
5 1 0 1
13 0 0 0
9 0 0 0
13 1 2 1
13 1 2 1
13 1 1 1
4 3 1 3
5 2 1 1
5 2 3 0
3 0 2 0
13 0 2 0
4 3 0 3
10 3 0 0
5 0 1 3
1 1 2 3
13 3 2 3
4 3 0 0
10 0 3 1
5 2 2 2
5 1 0 0
5 1 1 3
10 0 2 0
13 0 1 0
4 0 1 1
5 0 2 3
5 3 2 0
12 2 3 2
13 2 2 2
4 1 2 1
5 3 1 2
5 1 2 3
5 1 1 0
4 0 0 2
13 2 2 2
13 2 2 2
4 2 1 1
10 1 3 2
5 3 1 0
5 2 1 1
8 0 1 0
13 0 2 0
4 2 0 2
5 2 0 0
5 3 0 3
5 1 3 0
13 0 2 0
4 2 0 2
10 2 0 3
5 3 1 0
5 1 2 1
13 2 0 2
9 2 2 2
7 2 0 1
13 1 3 1
4 3 1 3
10 3 2 1
5 0 3 3
5 1 3 0
10 0 2 2
13 2 2 2
4 1 2 1
10 1 3 3
5 2 3 0
5 0 0 2
5 3 2 1
8 1 0 1
13 1 3 1
13 1 1 1
4 3 1 3
10 3 2 1
5 1 3 0
5 2 0 3
13 3 0 2
9 2 2 2
12 2 3 2
13 2 1 2
4 1 2 1
10 1 1 0
5 0 0 2
5 2 2 1
14 2 3 1
13 1 3 1
13 1 3 1
4 0 1 0
10 0 2 3
5 2 0 1
5 3 3 2
13 2 0 0
9 0 1 0
5 2 0 1
13 1 3 1
4 3 1 3
10 3 2 1
5 0 1 0
5 3 0 3
15 3 2 3
13 3 2 3
4 1 3 1
5 2 1 0
5 1 1 2
5 1 0 3
11 3 0 0
13 0 1 0
13 0 2 0
4 0 1 1
10 1 3 3
5 0 3 2
5 3 1 0
13 0 0 1
9 1 1 1
3 2 0 0
13 0 2 0
4 0 3 3
10 3 2 2
13 3 0 1
9 1 0 1
5 1 0 3
5 1 3 0
9 3 1 1
13 1 2 1
4 1 2 2
10 2 2 3
5 0 2 1
5 3 0 0
5 0 0 2
3 2 0 1
13 1 1 1
4 1 3 3
10 3 1 0
5 2 0 2
5 2 0 3
5 3 3 1
8 1 3 3
13 3 3 3
4 3 0 0
10 0 2 2
5 2 2 0
13 1 0 3
9 3 1 3
0 0 3 0
13 0 2 0
4 2 0 2
10 2 1 1
13 2 0 2
9 2 2 2
5 0 1 0
13 2 0 3
9 3 2 3
12 2 3 0
13 0 3 0
4 1 0 1
10 1 2 0
5 1 1 3
13 3 0 2
9 2 0 2
5 2 3 1
5 2 3 2
13 2 1 2
4 2 0 0
10 0 1 3
13 1 0 1
9 1 0 1
5 3 0 0
5 0 2 2
3 2 0 2
13 2 2 2
4 3 2 3
10 3 1 1
5 0 2 3
5 2 1 2
5 1 2 0
10 0 2 3
13 3 3 3
4 1 3 1
10 1 1 0
5 2 0 1
5 0 1 3
5 0 2 2
5 3 1 1
13 1 1 1
4 1 0 0
10 0 2 3
5 3 1 0
5 1 0 1
5 2 1 2
7 2 0 0
13 0 2 0
13 0 2 0
4 3 0 3
5 1 2 0
5 0 0 2
5 2 1 0
13 0 1 0
4 3 0 3
10 3 2 1
5 2 1 3
13 3 0 0
9 0 3 0
3 2 0 2
13 2 1 2
13 2 2 2
4 2 1 1
10 1 1 2
5 2 3 0
5 0 0 3
5 0 0 1
12 0 3 3
13 3 1 3
4 2 3 2
10 2 1 1
5 3 0 2
5 1 3 3
13 3 2 3
13 3 2 3
4 1 3 1
10 1 1 0
5 1 0 1
5 0 2 2
5 3 2 3
13 1 2 3
13 3 1 3
13 3 3 3
4 0 3 0
5 1 2 3
5 0 0 1
9 3 1 2
13 2 3 2
4 2 0 0
10 0 2 1
5 3 2 2
13 2 0 0
9 0 3 0
5 0 0 3
14 3 2 3
13 3 2 3
13 3 2 3
4 3 1 1
5 2 1 3
5 0 1 2
3 2 0 3
13 3 1 3
13 3 3 3
4 1 3 1
10 1 0 0
5 3 0 1
13 3 0 3
9 3 0 3
5 2 1 2
2 3 2 2
13 2 3 2
13 2 3 2
4 2 0 0
10 0 1 2
5 2 3 0
5 1 2 3
5 2 2 1
0 0 3 3
13 3 1 3
4 2 3 2
10 2 2 1
5 2 1 2
13 3 0 0
9 0 3 0
5 0 2 3
2 3 2 3
13 3 3 3
4 3 1 1
5 1 1 0
5 2 1 3
5 0 2 2
14 2 3 2
13 2 3 2
4 2 1 1
5 1 3 3
5 3 2 2
5 2 0 0
0 0 3 2
13 2 2 2
4 2 1 1
5 2 2 3
5 0 0 2
5 0 3 0
14 2 3 0
13 0 3 0
4 0 1 1
5 3 3 3
5 3 0 0
15 0 2 3
13 3 1 3
4 1 3 1
5 0 2 3
5 2 0 2
5 2 2 0
2 3 2 2
13 2 3 2
4 2 1 1
10 1 0 0
5 3 0 1
5 2 3 3
5 0 3 2
15 1 2 3
13 3 1 3
4 3 0 0
10 0 3 2
5 2 1 0
13 1 0 3
9 3 2 3
7 0 1 1
13 1 2 1
4 2 1 2
5 3 3 3
5 3 2 0
5 2 0 1
8 0 1 3
13 3 3 3
4 2 3 2
10 2 3 3
5 1 0 2
5 3 3 1
5 2 0 0
15 1 2 1
13 1 1 1
13 1 2 1
4 3 1 3
5 1 0 0
5 3 0 2
5 3 2 1
15 1 2 2
13 2 2 2
4 2 3 3
10 3 0 1
5 0 1 3
13 1 0 2
9 2 2 2
2 3 2 0
13 0 2 0
4 0 1 1
10 1 3 0");

        }
    }
}
