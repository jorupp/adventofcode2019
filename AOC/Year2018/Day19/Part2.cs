using System;
using System.Linq;

namespace AoC.Year2018.Day19
{
    public class Part2 : BasePart
    {
        protected void RunScenario(string title, string input)
        {
            RunScenario(title, () =>
            {
                var lines = input.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                var ipRegister = lines[0].Numbers()[0];
                var data = lines.Skip(1).Select(i =>
                {
                    var instruction = i.AlphaNumeric()[0];
                    var args = i.Numbers();
                    return (instruction, args);
                }).ToArray();

                var registers = Enumerable.Repeat(0, 6).ToArray();
                registers[0] = 1;

                while (registers[ipRegister] >= 0 && registers[ipRegister] < data.Length)
                {
                    if (registers[ipRegister] == 4)
                    {

                    }
                    var instruction = data[registers[ipRegister]];
                    var newRegisters = Execute(registers, instruction.Item1, instruction.Item2);
                    Console.WriteLine($"ip={registers[ipRegister]} [{string.Join(", ", registers)}] {instruction.Item1} {string.Join(" ", instruction.Item2)} [{string.Join(", ", newRegisters)}]");
                    registers = newRegisters;

                    registers[ipRegister]++;
                }

                Console.WriteLine(registers[0]);
            });
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
            //            RunScenario("initial", @"#ip 0
            //seti 5 0 1
            //seti 6 0 2
            //addi 0 1 0
            //addr 1 2 3
            //setr 1 0 0
            //seti 8 0 4
            //seti 9 0 5
            //");
            //return;
            RunScenario("part1", @"#ip 3
addi 3 16 3
seti 1 9 5
seti 1 1 4
mulr 5 4 2
eqrr 2 1 2
addr 2 3 3
addi 3 1 3
addr 5 0 0
addi 4 1 4
gtrr 4 1 2
addr 3 2 3
seti 2 3 3
addi 5 1 5
gtrr 5 1 2
addr 2 3 3
seti 1 4 3
mulr 3 3 3
addi 1 2 1
mulr 1 1 1
mulr 3 1 1
muli 1 11 1
addi 2 2 2
mulr 2 3 2
addi 2 20 2
addr 1 2 1
addr 3 0 3
seti 0 4 3
setr 3 9 2
mulr 2 3 2
addr 3 2 2
mulr 3 2 2
muli 2 14 2
mulr 2 3 2
addr 1 2 1
seti 0 6 0
seti 0 0 3");

        }
    }
}
