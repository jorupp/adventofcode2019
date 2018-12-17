using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;

namespace AoC.Year2018.Day16
{
    public class Part2a : BasePart
    {
        private static readonly string[] commands = new[] { "addr", "addi", "mulr", "muli", "banr", "bani", "borr", "bori", "setr", "seti", "gtir", "gtri", "gtrr", "eqir", "eqri", "eqrr" };
        protected void RunScenario(string title, string input)
        {
            RunScenario(title, () =>
            {
                var lines = input.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                var possible = Enumerable.Range(0, 16).Select(i => Enumerable.Repeat(true, 16).ToArray()).ToArray();

                var inputs = GetInputs(lines).ToArray();
                foreach (var inp in inputs)
                {
                    var start = inp[0].Numbers();
                    var command = inp[1].Numbers();
                    var commandNumber = command[0];
                    var args = command.Skip(1).ToArray();
                    var end = inp[2].Numbers();

                    var q = (from c in commands.Select((i, ix) => new { i, ix })
                        let output = Execute(start, c.i, args)
                        where !AreEqual(end, output)
                        select c.ix).ToArray();
                    foreach (var i in q)
                    {
                        possible[i][commandNumber] = false;
                    }
                }

                DumpState(possible);

                for (var z = 0; z < 32; z++)
                {
                    for (var x = 0; x < 16; x++)
                    {
                        var row = possible[x];
                        var rowSet = row.Select((i, ix) => new {i, ix}).Where(i => i.i).ToArray();
                        if (rowSet.Length == 1)
                        {
                            for (var y = 0; y < 16; y++)
                            {
                                if (y == x)
                                    continue;
                                possible[y][rowSet[0].ix] = false;
                            }
                        }

                        var col = possible.Select(i => i[x]).ToArray();
                        var colSet = col.Select((i, ix) => new {i, ix}).Where(i => i.i).ToArray();
                        if (colSet.Length == 1)
                        {
                            for (var y = 0; y < 16; y++)
                            {
                                if (y == x)
                                    continue;
                                possible[colSet[0].ix][y] = false;
                            }
                        }
                    }
                }

                DumpState(possible);
                DumpMap(possible);
            });
        }

        private void DumpMap(bool[][] possible)
        {
            var sb = new StringBuilder();
            for (var x = 0; x < 16; x++)
            {
                for (var y = 0; y < 16; y++)
                {
                    if (possible[y][x])
                    {
                        sb.AppendLine(commands[y]);
                    }
                }

                sb.AppendLine();
            }
            Console.WriteLine(sb.ToString());
        }

        private void DumpState(bool[][] possible)
        {
            var sb = new StringBuilder();

            for (var y = 0; y < 16; y++)
            {
                sb.Append(y.ToString().PadLeft(2, ' '));
            }

            sb.AppendLine();
            for (var x = 0; x < 16; x++)
            {
                for (var y = 0; y < 16; y++)
                {
                    sb.Append(possible[x][y] ? "X " : ". ");
                }

                sb.Append("  ");
                sb.Append(commands[x]);
                sb.AppendLine();
            }
            Console.WriteLine(sb.ToString());
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
            //return;
            RunScenario("part1", @"Before: [1, 3, 2, 2]
6 2 3 2
After:  [1, 3, 1, 2]

Before: [0, 2, 1, 1]
3 3 3 3
After:  [0, 2, 1, 0]

Before: [1, 3, 2, 1]
5 0 2 2
After:  [1, 3, 0, 1]

Before: [3, 1, 2, 2]
15 1 2 1
After:  [3, 0, 2, 2]

Before: [1, 3, 2, 1]
5 0 2 0
After:  [0, 3, 2, 1]

Before: [3, 3, 2, 1]
12 3 1 1
After:  [3, 1, 2, 1]

Before: [2, 1, 0, 1]
13 1 0 1
After:  [2, 0, 0, 1]

Before: [1, 1, 2, 1]
0 3 1 3
After:  [1, 1, 2, 0]

Before: [1, 1, 1, 2]
9 2 1 3
After:  [1, 1, 1, 2]

Before: [1, 1, 1, 2]
8 1 3 0
After:  [0, 1, 1, 2]

Before: [3, 1, 2, 1]
10 3 2 2
After:  [3, 1, 1, 1]

Before: [1, 1, 2, 2]
5 0 2 2
After:  [1, 1, 0, 2]

Before: [1, 0, 0, 3]
1 0 1 1
After:  [1, 1, 0, 3]

Before: [1, 2, 2, 3]
5 0 2 2
After:  [1, 2, 0, 3]

Before: [0, 0, 2, 1]
10 3 2 1
After:  [0, 1, 2, 1]

Before: [1, 2, 2, 3]
5 0 2 0
After:  [0, 2, 2, 3]

Before: [3, 0, 0, 2]
0 0 2 2
After:  [3, 0, 1, 2]

Before: [1, 3, 2, 1]
5 0 2 1
After:  [1, 0, 2, 1]

Before: [0, 1, 1, 0]
9 2 1 2
After:  [0, 1, 2, 0]

Before: [2, 3, 2, 1]
10 3 2 2
After:  [2, 3, 1, 1]

Before: [0, 0, 3, 0]
14 3 2 3
After:  [0, 0, 3, 1]

Before: [3, 3, 1, 3]
7 3 0 3
After:  [3, 3, 1, 1]

Before: [1, 0, 1, 0]
4 2 0 3
After:  [1, 0, 1, 2]

Before: [0, 1, 0, 3]
1 1 0 2
After:  [0, 1, 1, 3]

Before: [1, 2, 2, 1]
10 3 2 3
After:  [1, 2, 2, 1]

Before: [3, 1, 1, 1]
9 2 1 3
After:  [3, 1, 1, 2]

Before: [3, 0, 1, 1]
4 2 3 2
After:  [3, 0, 2, 1]

Before: [0, 3, 0, 2]
11 0 0 1
After:  [0, 0, 0, 2]

Before: [0, 1, 3, 0]
14 3 2 3
After:  [0, 1, 3, 1]

Before: [0, 1, 2, 1]
15 1 2 0
After:  [0, 1, 2, 1]

Before: [1, 2, 2, 1]
5 0 2 2
After:  [1, 2, 0, 1]

Before: [2, 1, 0, 1]
0 3 1 1
After:  [2, 0, 0, 1]

Before: [1, 2, 1, 2]
3 3 3 3
After:  [1, 2, 1, 0]

Before: [0, 0, 0, 0]
6 0 3 0
After:  [1, 0, 0, 0]

Before: [3, 1, 1, 3]
0 2 1 1
After:  [3, 0, 1, 3]

Before: [0, 1, 1, 2]
0 2 1 1
After:  [0, 0, 1, 2]

Before: [1, 3, 2, 2]
5 0 2 1
After:  [1, 0, 2, 2]

Before: [2, 1, 3, 1]
13 1 0 3
After:  [2, 1, 3, 0]

Before: [2, 1, 2, 3]
13 1 0 2
After:  [2, 1, 0, 3]

Before: [3, 1, 1, 2]
9 2 1 2
After:  [3, 1, 2, 2]

Before: [2, 1, 1, 1]
13 1 0 2
After:  [2, 1, 0, 1]

Before: [2, 1, 0, 2]
8 1 3 2
After:  [2, 1, 0, 2]

Before: [0, 1, 2, 2]
6 2 3 1
After:  [0, 1, 2, 2]

Before: [1, 0, 2, 1]
5 0 2 2
After:  [1, 0, 0, 1]

Before: [1, 1, 1, 1]
9 2 1 2
After:  [1, 1, 2, 1]

Before: [3, 1, 1, 1]
9 2 1 1
After:  [3, 2, 1, 1]

Before: [3, 2, 3, 0]
14 3 2 3
After:  [3, 2, 3, 1]

Before: [2, 1, 2, 3]
13 1 0 1
After:  [2, 0, 2, 3]

Before: [1, 3, 1, 1]
4 2 3 2
After:  [1, 3, 2, 1]

Before: [3, 3, 2, 1]
12 3 1 3
After:  [3, 3, 2, 1]

Before: [3, 3, 0, 1]
12 3 1 2
After:  [3, 3, 1, 1]

Before: [1, 3, 0, 2]
14 2 3 2
After:  [1, 3, 1, 2]

Before: [2, 1, 2, 3]
13 1 0 3
After:  [2, 1, 2, 0]

Before: [0, 1, 1, 0]
9 2 1 1
After:  [0, 2, 1, 0]

Before: [0, 0, 2, 1]
1 3 1 3
After:  [0, 0, 2, 1]

Before: [0, 3, 1, 1]
4 2 3 2
After:  [0, 3, 2, 1]

Before: [0, 3, 2, 2]
6 2 3 3
After:  [0, 3, 2, 1]

Before: [1, 1, 2, 0]
5 0 2 3
After:  [1, 1, 2, 0]

Before: [3, 0, 2, 2]
6 2 3 3
After:  [3, 0, 2, 1]

Before: [3, 0, 1, 2]
3 3 3 3
After:  [3, 0, 1, 0]

Before: [2, 1, 3, 1]
0 2 0 3
After:  [2, 1, 3, 1]

Before: [2, 1, 1, 2]
8 1 3 1
After:  [2, 0, 1, 2]

Before: [2, 1, 3, 3]
13 1 0 0
After:  [0, 1, 3, 3]

Before: [1, 1, 1, 3]
9 2 1 1
After:  [1, 2, 1, 3]

Before: [3, 3, 3, 1]
7 2 3 1
After:  [3, 0, 3, 1]

Before: [1, 1, 1, 0]
9 2 1 2
After:  [1, 1, 2, 0]

Before: [3, 1, 2, 2]
8 1 3 1
After:  [3, 0, 2, 2]

Before: [3, 1, 0, 1]
3 3 3 3
After:  [3, 1, 0, 0]

Before: [3, 0, 3, 3]
7 3 2 2
After:  [3, 0, 1, 3]

Before: [1, 1, 1, 0]
9 2 1 0
After:  [2, 1, 1, 0]

Before: [0, 0, 3, 1]
3 3 3 0
After:  [0, 0, 3, 1]

Before: [0, 2, 0, 1]
6 0 2 0
After:  [1, 2, 0, 1]

Before: [0, 2, 2, 1]
10 3 2 0
After:  [1, 2, 2, 1]

Before: [3, 3, 0, 1]
12 3 1 0
After:  [1, 3, 0, 1]

Before: [3, 2, 3, 1]
3 3 3 0
After:  [0, 2, 3, 1]

Before: [2, 2, 3, 2]
3 3 3 2
After:  [2, 2, 0, 2]

Before: [0, 3, 0, 2]
14 2 3 1
After:  [0, 1, 0, 2]

Before: [0, 0, 3, 0]
14 3 2 1
After:  [0, 1, 3, 0]

Before: [1, 3, 0, 1]
12 3 1 3
After:  [1, 3, 0, 1]

Before: [0, 2, 2, 2]
7 2 1 3
After:  [0, 2, 2, 1]

Before: [2, 3, 3, 1]
3 3 3 0
After:  [0, 3, 3, 1]

Before: [2, 1, 3, 2]
13 1 0 1
After:  [2, 0, 3, 2]

Before: [2, 1, 2, 1]
15 1 2 0
After:  [0, 1, 2, 1]

Before: [1, 1, 1, 0]
4 2 0 2
After:  [1, 1, 2, 0]

Before: [3, 1, 2, 1]
15 1 2 0
After:  [0, 1, 2, 1]

Before: [3, 1, 3, 2]
8 1 3 1
After:  [3, 0, 3, 2]

Before: [1, 1, 1, 3]
9 2 1 2
After:  [1, 1, 2, 3]

Before: [2, 1, 3, 1]
13 1 0 2
After:  [2, 1, 0, 1]

Before: [2, 3, 1, 1]
4 2 3 1
After:  [2, 2, 1, 1]

Before: [1, 2, 2, 2]
3 3 3 1
After:  [1, 0, 2, 2]

Before: [1, 1, 1, 0]
9 2 1 1
After:  [1, 2, 1, 0]

Before: [2, 1, 2, 1]
15 1 2 1
After:  [2, 0, 2, 1]

Before: [1, 0, 3, 2]
3 3 3 3
After:  [1, 0, 3, 0]

Before: [1, 1, 1, 2]
9 2 1 0
After:  [2, 1, 1, 2]

Before: [2, 3, 3, 3]
7 3 2 0
After:  [1, 3, 3, 3]

Before: [1, 2, 2, 2]
5 0 2 2
After:  [1, 2, 0, 2]

Before: [1, 1, 1, 3]
0 2 1 0
After:  [0, 1, 1, 3]

Before: [3, 1, 1, 3]
0 2 1 3
After:  [3, 1, 1, 0]

Before: [3, 1, 3, 0]
14 3 2 1
After:  [3, 1, 3, 0]

Before: [3, 1, 0, 1]
0 3 1 2
After:  [3, 1, 0, 1]

Before: [1, 3, 1, 0]
4 2 0 0
After:  [2, 3, 1, 0]

Before: [2, 2, 1, 3]
2 2 3 0
After:  [0, 2, 1, 3]

Before: [3, 3, 3, 0]
14 3 2 3
After:  [3, 3, 3, 1]

Before: [0, 3, 2, 1]
11 0 0 0
After:  [0, 3, 2, 1]

Before: [0, 0, 2, 0]
6 0 3 2
After:  [0, 0, 1, 0]

Before: [0, 1, 2, 1]
15 1 2 2
After:  [0, 1, 0, 1]

Before: [1, 1, 1, 1]
4 2 0 2
After:  [1, 1, 2, 1]

Before: [3, 0, 1, 1]
3 3 3 0
After:  [0, 0, 1, 1]

Before: [0, 0, 1, 3]
1 2 1 0
After:  [1, 0, 1, 3]

Before: [1, 1, 1, 3]
2 1 3 2
After:  [1, 1, 0, 3]

Before: [1, 2, 1, 1]
4 2 0 1
After:  [1, 2, 1, 1]

Before: [1, 2, 1, 3]
2 1 3 1
After:  [1, 0, 1, 3]

Before: [0, 1, 1, 1]
11 0 0 2
After:  [0, 1, 0, 1]

Before: [1, 3, 1, 1]
12 3 1 0
After:  [1, 3, 1, 1]

Before: [1, 1, 1, 3]
4 2 0 1
After:  [1, 2, 1, 3]

Before: [3, 2, 0, 3]
0 0 2 2
After:  [3, 2, 1, 3]

Before: [2, 2, 2, 1]
10 3 2 2
After:  [2, 2, 1, 1]

Before: [1, 0, 1, 3]
1 0 1 2
After:  [1, 0, 1, 3]

Before: [1, 1, 2, 1]
5 0 2 3
After:  [1, 1, 2, 0]

Before: [1, 0, 1, 0]
4 2 0 0
After:  [2, 0, 1, 0]

Before: [2, 1, 0, 3]
13 1 0 1
After:  [2, 0, 0, 3]

Before: [3, 1, 2, 2]
8 1 3 0
After:  [0, 1, 2, 2]

Before: [0, 1, 2, 2]
11 0 0 3
After:  [0, 1, 2, 0]

Before: [0, 1, 3, 2]
3 3 3 1
After:  [0, 0, 3, 2]

Before: [1, 0, 2, 2]
3 3 3 1
After:  [1, 0, 2, 2]

Before: [2, 0, 2, 2]
3 3 3 3
After:  [2, 0, 2, 0]

Before: [2, 1, 1, 0]
13 1 0 0
After:  [0, 1, 1, 0]

Before: [2, 1, 3, 1]
0 3 1 2
After:  [2, 1, 0, 1]

Before: [0, 3, 0, 2]
6 0 2 1
After:  [0, 1, 0, 2]

Before: [3, 3, 3, 1]
12 3 1 0
After:  [1, 3, 3, 1]

Before: [1, 0, 1, 3]
4 2 0 1
After:  [1, 2, 1, 3]

Before: [0, 1, 1, 2]
3 3 3 2
After:  [0, 1, 0, 2]

Before: [0, 0, 1, 3]
2 2 3 1
After:  [0, 0, 1, 3]

Before: [0, 1, 0, 2]
14 2 3 1
After:  [0, 1, 0, 2]

Before: [3, 1, 2, 2]
15 1 2 3
After:  [3, 1, 2, 0]

Before: [0, 2, 3, 0]
14 3 2 1
After:  [0, 1, 3, 0]

Before: [2, 1, 2, 1]
15 1 2 3
After:  [2, 1, 2, 0]

Before: [2, 1, 2, 2]
8 1 3 0
After:  [0, 1, 2, 2]

Before: [3, 1, 0, 1]
0 0 2 0
After:  [1, 1, 0, 1]

Before: [3, 2, 0, 3]
7 3 0 3
After:  [3, 2, 0, 1]

Before: [0, 0, 1, 1]
4 2 3 3
After:  [0, 0, 1, 2]

Before: [1, 2, 2, 1]
5 0 2 3
After:  [1, 2, 2, 0]

Before: [2, 0, 1, 1]
4 2 3 3
After:  [2, 0, 1, 2]

Before: [2, 1, 3, 2]
8 1 3 3
After:  [2, 1, 3, 0]

Before: [1, 1, 2, 1]
15 1 2 2
After:  [1, 1, 0, 1]

Before: [2, 1, 1, 2]
9 2 1 2
After:  [2, 1, 2, 2]

Before: [1, 2, 2, 3]
5 0 2 3
After:  [1, 2, 2, 0]

Before: [1, 2, 2, 2]
5 0 2 0
After:  [0, 2, 2, 2]

Before: [3, 0, 1, 3]
2 2 3 1
After:  [3, 0, 1, 3]

Before: [1, 0, 2, 2]
5 0 2 3
After:  [1, 0, 2, 0]

Before: [2, 0, 1, 1]
4 2 3 1
After:  [2, 2, 1, 1]

Before: [2, 1, 2, 3]
13 1 0 0
After:  [0, 1, 2, 3]

Before: [0, 1, 2, 3]
11 0 0 2
After:  [0, 1, 0, 3]

Before: [0, 0, 3, 1]
1 3 1 0
After:  [1, 0, 3, 1]

Before: [0, 2, 0, 0]
11 0 0 3
After:  [0, 2, 0, 0]

Before: [0, 2, 2, 2]
7 2 1 1
After:  [0, 1, 2, 2]

Before: [2, 1, 2, 3]
15 1 2 3
After:  [2, 1, 2, 0]

Before: [2, 1, 2, 1]
13 1 0 1
After:  [2, 0, 2, 1]

Before: [1, 0, 2, 1]
5 0 2 1
After:  [1, 0, 2, 1]

Before: [1, 0, 3, 1]
1 0 1 0
After:  [1, 0, 3, 1]

Before: [3, 0, 3, 0]
14 3 2 0
After:  [1, 0, 3, 0]

Before: [0, 1, 1, 2]
8 1 3 3
After:  [0, 1, 1, 0]

Before: [2, 1, 0, 3]
13 1 0 2
After:  [2, 1, 0, 3]

Before: [2, 1, 1, 3]
2 2 3 3
After:  [2, 1, 1, 0]

Before: [3, 2, 2, 0]
7 2 1 2
After:  [3, 2, 1, 0]

Before: [2, 3, 0, 2]
14 2 3 1
After:  [2, 1, 0, 2]

Before: [2, 1, 2, 2]
13 1 0 1
After:  [2, 0, 2, 2]

Before: [0, 0, 3, 0]
6 0 3 0
After:  [1, 0, 3, 0]

Before: [2, 3, 2, 1]
12 3 1 1
After:  [2, 1, 2, 1]

Before: [2, 2, 2, 1]
10 3 2 0
After:  [1, 2, 2, 1]

Before: [0, 3, 0, 1]
6 0 2 0
After:  [1, 3, 0, 1]

Before: [0, 1, 0, 3]
2 1 3 3
After:  [0, 1, 0, 0]

Before: [0, 3, 1, 1]
11 0 0 1
After:  [0, 0, 1, 1]

Before: [2, 0, 0, 1]
3 3 3 3
After:  [2, 0, 0, 0]

Before: [0, 1, 1, 3]
9 2 1 0
After:  [2, 1, 1, 3]

Before: [0, 3, 0, 0]
6 0 3 2
After:  [0, 3, 1, 0]

Before: [2, 1, 2, 0]
7 2 0 3
After:  [2, 1, 2, 1]

Before: [3, 1, 1, 1]
4 2 3 1
After:  [3, 2, 1, 1]

Before: [2, 1, 2, 2]
13 1 0 3
After:  [2, 1, 2, 0]

Before: [1, 1, 2, 0]
15 1 2 0
After:  [0, 1, 2, 0]

Before: [1, 3, 1, 1]
4 2 0 3
After:  [1, 3, 1, 2]

Before: [2, 1, 2, 3]
2 1 3 2
After:  [2, 1, 0, 3]

Before: [1, 2, 1, 2]
4 2 0 2
After:  [1, 2, 2, 2]

Before: [3, 1, 2, 1]
15 1 2 1
After:  [3, 0, 2, 1]

Before: [1, 1, 2, 3]
2 2 3 2
After:  [1, 1, 0, 3]

Before: [2, 1, 1, 0]
13 1 0 2
After:  [2, 1, 0, 0]

Before: [2, 1, 1, 2]
13 1 0 3
After:  [2, 1, 1, 0]

Before: [3, 2, 2, 3]
2 2 3 3
After:  [3, 2, 2, 0]

Before: [2, 3, 2, 1]
12 3 1 0
After:  [1, 3, 2, 1]

Before: [1, 1, 1, 1]
4 2 3 1
After:  [1, 2, 1, 1]

Before: [0, 3, 3, 1]
12 3 1 0
After:  [1, 3, 3, 1]

Before: [1, 3, 2, 1]
12 3 1 0
After:  [1, 3, 2, 1]

Before: [3, 2, 2, 2]
7 2 1 1
After:  [3, 1, 2, 2]

Before: [1, 1, 1, 2]
8 1 3 1
After:  [1, 0, 1, 2]

Before: [0, 3, 2, 1]
12 3 1 1
After:  [0, 1, 2, 1]

Before: [3, 0, 2, 1]
10 3 2 1
After:  [3, 1, 2, 1]

Before: [0, 2, 2, 2]
0 3 2 1
After:  [0, 0, 2, 2]

Before: [1, 1, 2, 1]
10 3 2 2
After:  [1, 1, 1, 1]

Before: [2, 1, 3, 0]
13 1 0 2
After:  [2, 1, 0, 0]

Before: [0, 1, 2, 2]
8 1 3 1
After:  [0, 0, 2, 2]

Before: [3, 1, 1, 1]
4 2 3 0
After:  [2, 1, 1, 1]

Before: [2, 2, 2, 2]
6 2 3 2
After:  [2, 2, 1, 2]

Before: [0, 0, 3, 1]
7 2 3 2
After:  [0, 0, 0, 1]

Before: [0, 3, 2, 1]
10 3 2 3
After:  [0, 3, 2, 1]

Before: [1, 0, 1, 0]
1 2 1 2
After:  [1, 0, 1, 0]

Before: [1, 3, 2, 1]
10 3 2 1
After:  [1, 1, 2, 1]

Before: [2, 2, 1, 1]
3 2 3 1
After:  [2, 0, 1, 1]

Before: [0, 1, 2, 3]
15 1 2 0
After:  [0, 1, 2, 3]

Before: [2, 3, 3, 1]
12 3 1 3
After:  [2, 3, 3, 1]

Before: [0, 1, 2, 2]
15 1 2 2
After:  [0, 1, 0, 2]

Before: [3, 1, 1, 3]
9 2 1 2
After:  [3, 1, 2, 3]

Before: [0, 0, 0, 3]
6 0 2 1
After:  [0, 1, 0, 3]

Before: [3, 3, 2, 1]
10 3 2 0
After:  [1, 3, 2, 1]

Before: [1, 1, 2, 1]
15 1 2 1
After:  [1, 0, 2, 1]

Before: [2, 3, 2, 1]
7 2 0 0
After:  [1, 3, 2, 1]

Before: [2, 1, 3, 0]
13 1 0 0
After:  [0, 1, 3, 0]

Before: [2, 1, 2, 0]
13 1 0 3
After:  [2, 1, 2, 0]

Before: [3, 0, 1, 1]
4 2 3 3
After:  [3, 0, 1, 2]

Before: [0, 3, 1, 1]
3 2 3 1
After:  [0, 0, 1, 1]

Before: [0, 2, 2, 2]
0 3 2 0
After:  [0, 2, 2, 2]

Before: [2, 3, 2, 1]
12 3 1 2
After:  [2, 3, 1, 1]

Before: [0, 0, 1, 2]
1 2 1 0
After:  [1, 0, 1, 2]

Before: [3, 3, 0, 1]
12 3 1 3
After:  [3, 3, 0, 1]

Before: [0, 2, 0, 1]
11 0 0 1
After:  [0, 0, 0, 1]

Before: [3, 1, 1, 1]
9 2 1 0
After:  [2, 1, 1, 1]

Before: [3, 1, 0, 1]
0 0 2 1
After:  [3, 1, 0, 1]

Before: [3, 3, 0, 1]
12 3 1 1
After:  [3, 1, 0, 1]

Before: [2, 3, 3, 3]
7 3 2 3
After:  [2, 3, 3, 1]

Before: [1, 1, 2, 3]
2 1 3 3
After:  [1, 1, 2, 0]

Before: [2, 2, 2, 3]
2 1 3 2
After:  [2, 2, 0, 3]

Before: [1, 1, 2, 2]
5 0 2 0
After:  [0, 1, 2, 2]

Before: [1, 1, 2, 1]
15 1 2 3
After:  [1, 1, 2, 0]

Before: [2, 1, 2, 3]
15 1 2 2
After:  [2, 1, 0, 3]

Before: [2, 1, 1, 3]
13 1 0 3
After:  [2, 1, 1, 0]

Before: [2, 1, 2, 2]
13 1 0 0
After:  [0, 1, 2, 2]

Before: [1, 0, 2, 1]
5 0 2 0
After:  [0, 0, 2, 1]

Before: [0, 1, 2, 2]
8 1 3 3
After:  [0, 1, 2, 0]

Before: [3, 2, 2, 2]
6 2 3 0
After:  [1, 2, 2, 2]

Before: [2, 1, 3, 1]
13 1 0 0
After:  [0, 1, 3, 1]

Before: [2, 1, 3, 0]
14 3 2 1
After:  [2, 1, 3, 0]

Before: [0, 1, 3, 0]
14 3 2 1
After:  [0, 1, 3, 0]

Before: [1, 1, 1, 3]
2 2 3 3
After:  [1, 1, 1, 0]

Before: [2, 2, 2, 0]
7 2 1 2
After:  [2, 2, 1, 0]

Before: [2, 1, 1, 1]
9 2 1 2
After:  [2, 1, 2, 1]

Before: [3, 1, 2, 1]
10 3 2 0
After:  [1, 1, 2, 1]

Before: [1, 3, 2, 1]
10 3 2 2
After:  [1, 3, 1, 1]

Before: [0, 1, 0, 0]
11 0 0 1
After:  [0, 0, 0, 0]

Before: [2, 1, 2, 2]
0 3 2 3
After:  [2, 1, 2, 0]

Before: [2, 1, 0, 0]
13 1 0 2
After:  [2, 1, 0, 0]

Before: [1, 0, 2, 1]
10 3 2 2
After:  [1, 0, 1, 1]

Before: [3, 2, 2, 3]
2 1 3 0
After:  [0, 2, 2, 3]

Before: [1, 0, 1, 3]
2 2 3 2
After:  [1, 0, 0, 3]

Before: [3, 0, 2, 2]
0 3 2 3
After:  [3, 0, 2, 0]

Before: [0, 3, 3, 1]
12 3 1 3
After:  [0, 3, 3, 1]

Before: [1, 3, 2, 3]
5 0 2 0
After:  [0, 3, 2, 3]

Before: [2, 1, 0, 2]
13 1 0 0
After:  [0, 1, 0, 2]

Before: [1, 3, 2, 2]
5 0 2 2
After:  [1, 3, 0, 2]

Before: [1, 1, 1, 2]
9 2 1 1
After:  [1, 2, 1, 2]

Before: [1, 2, 2, 1]
5 0 2 0
After:  [0, 2, 2, 1]

Before: [0, 1, 2, 1]
10 3 2 0
After:  [1, 1, 2, 1]

Before: [0, 3, 3, 0]
6 0 3 1
After:  [0, 1, 3, 0]

Before: [0, 1, 2, 2]
8 1 3 2
After:  [0, 1, 0, 2]

Before: [2, 1, 3, 2]
13 1 0 2
After:  [2, 1, 0, 2]

Before: [1, 1, 0, 1]
3 3 3 1
After:  [1, 0, 0, 1]

Before: [3, 1, 2, 1]
10 3 2 3
After:  [3, 1, 2, 1]

Before: [2, 1, 1, 3]
2 1 3 3
After:  [2, 1, 1, 0]

Before: [0, 1, 2, 2]
8 1 3 0
After:  [0, 1, 2, 2]

Before: [3, 1, 1, 0]
9 2 1 2
After:  [3, 1, 2, 0]

Before: [2, 2, 3, 0]
14 3 2 2
After:  [2, 2, 1, 0]

Before: [3, 2, 2, 1]
10 3 2 0
After:  [1, 2, 2, 1]

Before: [0, 1, 0, 1]
3 3 3 2
After:  [0, 1, 0, 1]

Before: [3, 1, 0, 2]
3 3 3 3
After:  [3, 1, 0, 0]

Before: [0, 3, 0, 0]
11 0 0 0
After:  [0, 3, 0, 0]

Before: [3, 2, 1, 1]
3 2 3 0
After:  [0, 2, 1, 1]

Before: [1, 1, 1, 1]
9 2 1 3
After:  [1, 1, 1, 2]

Before: [2, 3, 3, 1]
12 3 1 2
After:  [2, 3, 1, 1]

Before: [0, 1, 3, 3]
2 1 3 2
After:  [0, 1, 0, 3]

Before: [3, 1, 3, 2]
8 1 3 0
After:  [0, 1, 3, 2]

Before: [1, 1, 1, 0]
9 2 1 3
After:  [1, 1, 1, 2]

Before: [3, 1, 1, 2]
8 1 3 1
After:  [3, 0, 1, 2]

Before: [2, 3, 0, 1]
12 3 1 1
After:  [2, 1, 0, 1]

Before: [2, 1, 1, 3]
13 1 0 1
After:  [2, 0, 1, 3]

Before: [1, 3, 2, 2]
5 0 2 0
After:  [0, 3, 2, 2]

Before: [3, 0, 0, 3]
0 0 2 3
After:  [3, 0, 0, 1]

Before: [0, 0, 0, 3]
11 0 0 1
After:  [0, 0, 0, 3]

Before: [2, 1, 0, 1]
13 1 0 3
After:  [2, 1, 0, 0]

Before: [0, 3, 1, 1]
12 3 1 3
After:  [0, 3, 1, 1]

Before: [1, 1, 1, 3]
9 2 1 0
After:  [2, 1, 1, 3]

Before: [1, 0, 2, 3]
1 0 1 1
After:  [1, 1, 2, 3]

Before: [1, 1, 2, 3]
2 1 3 0
After:  [0, 1, 2, 3]

Before: [0, 2, 2, 1]
11 0 0 1
After:  [0, 0, 2, 1]

Before: [1, 1, 1, 1]
9 2 1 1
After:  [1, 2, 1, 1]

Before: [1, 1, 3, 0]
14 3 2 0
After:  [1, 1, 3, 0]

Before: [1, 1, 1, 1]
4 2 0 3
After:  [1, 1, 1, 2]

Before: [1, 0, 2, 1]
5 0 2 3
After:  [1, 0, 2, 0]

Before: [2, 1, 2, 3]
15 1 2 0
After:  [0, 1, 2, 3]

Before: [0, 1, 1, 1]
1 1 0 0
After:  [1, 1, 1, 1]

Before: [0, 3, 0, 1]
12 3 1 1
After:  [0, 1, 0, 1]

Before: [1, 2, 2, 1]
7 2 1 3
After:  [1, 2, 2, 1]

Before: [3, 1, 3, 2]
8 1 3 3
After:  [3, 1, 3, 0]

Before: [0, 0, 3, 0]
14 3 2 0
After:  [1, 0, 3, 0]

Before: [3, 1, 0, 0]
0 0 2 2
After:  [3, 1, 1, 0]

Before: [1, 0, 2, 2]
6 2 3 1
After:  [1, 1, 2, 2]

Before: [0, 1, 1, 3]
0 2 1 2
After:  [0, 1, 0, 3]

Before: [3, 3, 0, 2]
14 2 3 3
After:  [3, 3, 0, 1]

Before: [2, 1, 2, 2]
6 2 3 1
After:  [2, 1, 2, 2]

Before: [2, 2, 3, 3]
2 1 3 1
After:  [2, 0, 3, 3]

Before: [2, 1, 3, 3]
13 1 0 2
After:  [2, 1, 0, 3]

Before: [2, 3, 3, 3]
0 2 0 3
After:  [2, 3, 3, 1]

Before: [1, 1, 1, 1]
4 2 3 0
After:  [2, 1, 1, 1]

Before: [3, 0, 1, 1]
1 3 1 0
After:  [1, 0, 1, 1]

Before: [1, 1, 2, 2]
8 1 3 1
After:  [1, 0, 2, 2]

Before: [3, 2, 0, 3]
0 0 2 3
After:  [3, 2, 0, 1]

Before: [1, 1, 1, 2]
4 2 0 1
After:  [1, 2, 1, 2]

Before: [2, 1, 1, 3]
13 1 0 0
After:  [0, 1, 1, 3]

Before: [3, 0, 2, 1]
1 3 1 2
After:  [3, 0, 1, 1]

Before: [2, 1, 1, 0]
13 1 0 3
After:  [2, 1, 1, 0]

Before: [1, 0, 2, 2]
1 0 1 0
After:  [1, 0, 2, 2]

Before: [1, 1, 2, 3]
2 2 3 1
After:  [1, 0, 2, 3]

Before: [2, 1, 1, 1]
9 2 1 3
After:  [2, 1, 1, 2]

Before: [0, 1, 2, 0]
15 1 2 3
After:  [0, 1, 2, 0]

Before: [3, 1, 0, 2]
14 2 3 3
After:  [3, 1, 0, 1]

Before: [0, 2, 0, 0]
11 0 0 2
After:  [0, 2, 0, 0]

Before: [2, 0, 2, 1]
10 3 2 2
After:  [2, 0, 1, 1]

Before: [1, 3, 1, 2]
4 2 0 2
After:  [1, 3, 2, 2]

Before: [2, 0, 0, 2]
14 2 3 2
After:  [2, 0, 1, 2]

Before: [0, 1, 2, 3]
2 1 3 2
After:  [0, 1, 0, 3]

Before: [1, 3, 2, 1]
12 3 1 1
After:  [1, 1, 2, 1]

Before: [2, 1, 0, 0]
13 1 0 3
After:  [2, 1, 0, 0]

Before: [0, 1, 2, 0]
11 0 0 1
After:  [0, 0, 2, 0]

Before: [2, 3, 0, 2]
14 2 3 2
After:  [2, 3, 1, 2]

Before: [0, 1, 2, 1]
15 1 2 3
After:  [0, 1, 2, 0]

Before: [1, 3, 1, 0]
4 2 0 2
After:  [1, 3, 2, 0]

Before: [1, 2, 0, 3]
2 1 3 0
After:  [0, 2, 0, 3]

Before: [0, 3, 3, 2]
11 0 0 2
After:  [0, 3, 0, 2]

Before: [2, 1, 1, 2]
13 1 0 0
After:  [0, 1, 1, 2]

Before: [1, 0, 2, 2]
5 0 2 2
After:  [1, 0, 0, 2]

Before: [3, 1, 2, 3]
15 1 2 1
After:  [3, 0, 2, 3]

Before: [2, 0, 3, 2]
0 2 0 3
After:  [2, 0, 3, 1]

Before: [1, 3, 2, 0]
5 0 2 1
After:  [1, 0, 2, 0]

Before: [3, 2, 0, 3]
0 0 2 0
After:  [1, 2, 0, 3]

Before: [2, 3, 0, 2]
14 2 3 3
After:  [2, 3, 0, 1]

Before: [1, 0, 2, 3]
5 0 2 1
After:  [1, 0, 2, 3]

Before: [0, 1, 0, 3]
1 1 0 1
After:  [0, 1, 0, 3]

Before: [2, 1, 3, 2]
8 1 3 2
After:  [2, 1, 0, 2]

Before: [1, 0, 1, 3]
1 2 1 0
After:  [1, 0, 1, 3]

Before: [0, 2, 0, 1]
6 0 2 1
After:  [0, 1, 0, 1]

Before: [2, 1, 1, 0]
9 2 1 0
After:  [2, 1, 1, 0]

Before: [0, 1, 1, 1]
9 2 1 3
After:  [0, 1, 1, 2]

Before: [3, 0, 1, 1]
1 2 1 2
After:  [3, 0, 1, 1]

Before: [1, 1, 2, 3]
5 0 2 3
After:  [1, 1, 2, 0]

Before: [3, 1, 2, 2]
15 1 2 0
After:  [0, 1, 2, 2]

Before: [3, 1, 1, 0]
9 2 1 1
After:  [3, 2, 1, 0]

Before: [2, 1, 0, 1]
13 1 0 2
After:  [2, 1, 0, 1]

Before: [3, 3, 1, 1]
3 2 3 0
After:  [0, 3, 1, 1]

Before: [3, 1, 1, 2]
8 1 3 3
After:  [3, 1, 1, 0]

Before: [1, 2, 1, 0]
4 2 0 1
After:  [1, 2, 1, 0]

Before: [1, 0, 1, 1]
1 3 1 1
After:  [1, 1, 1, 1]

Before: [0, 3, 0, 1]
12 3 1 2
After:  [0, 3, 1, 1]

Before: [1, 0, 2, 0]
5 0 2 1
After:  [1, 0, 2, 0]

Before: [2, 1, 2, 2]
13 1 0 2
After:  [2, 1, 0, 2]

Before: [2, 1, 2, 1]
3 3 3 1
After:  [2, 0, 2, 1]

Before: [2, 0, 2, 1]
7 2 0 2
After:  [2, 0, 1, 1]

Before: [2, 1, 2, 1]
15 1 2 2
After:  [2, 1, 0, 1]

Before: [3, 3, 3, 1]
12 3 1 3
After:  [3, 3, 3, 1]

Before: [2, 1, 1, 1]
9 2 1 0
After:  [2, 1, 1, 1]

Before: [0, 1, 2, 3]
2 2 3 2
After:  [0, 1, 0, 3]

Before: [0, 2, 0, 2]
6 0 2 1
After:  [0, 1, 0, 2]

Before: [1, 1, 2, 2]
5 0 2 1
After:  [1, 0, 2, 2]

Before: [0, 0, 3, 3]
7 3 2 2
After:  [0, 0, 1, 3]

Before: [2, 3, 2, 0]
7 2 0 3
After:  [2, 3, 2, 1]

Before: [2, 2, 2, 2]
7 2 0 3
After:  [2, 2, 2, 1]

Before: [3, 1, 2, 3]
2 2 3 2
After:  [3, 1, 0, 3]

Before: [3, 1, 1, 0]
9 2 1 0
After:  [2, 1, 1, 0]

Before: [0, 3, 1, 0]
11 0 0 1
After:  [0, 0, 1, 0]

Before: [2, 0, 0, 1]
1 3 1 3
After:  [2, 0, 0, 1]

Before: [3, 0, 3, 0]
14 3 2 1
After:  [3, 1, 3, 0]

Before: [0, 0, 1, 0]
11 0 0 1
After:  [0, 0, 1, 0]

Before: [2, 1, 2, 2]
7 2 0 0
After:  [1, 1, 2, 2]

Before: [3, 3, 1, 1]
12 3 1 0
After:  [1, 3, 1, 1]

Before: [2, 1, 3, 3]
13 1 0 1
After:  [2, 0, 3, 3]

Before: [0, 2, 3, 0]
14 3 2 3
After:  [0, 2, 3, 1]

Before: [2, 2, 3, 3]
2 1 3 3
After:  [2, 2, 3, 0]

Before: [2, 3, 2, 2]
7 2 0 3
After:  [2, 3, 2, 1]

Before: [3, 3, 0, 2]
14 2 3 2
After:  [3, 3, 1, 2]

Before: [1, 2, 3, 0]
14 3 2 3
After:  [1, 2, 3, 1]

Before: [2, 2, 1, 1]
4 2 3 1
After:  [2, 2, 1, 1]

Before: [3, 2, 2, 1]
10 3 2 1
After:  [3, 1, 2, 1]

Before: [1, 3, 1, 0]
4 2 0 3
After:  [1, 3, 1, 2]

Before: [1, 0, 3, 0]
14 3 2 2
After:  [1, 0, 1, 0]

Before: [1, 3, 0, 2]
14 2 3 1
After:  [1, 1, 0, 2]

Before: [3, 0, 0, 3]
7 3 0 0
After:  [1, 0, 0, 3]

Before: [0, 2, 1, 1]
4 2 3 2
After:  [0, 2, 2, 1]

Before: [2, 1, 1, 2]
9 2 1 0
After:  [2, 1, 1, 2]

Before: [1, 1, 3, 0]
14 3 2 1
After:  [1, 1, 3, 0]

Before: [1, 0, 2, 2]
6 2 3 3
After:  [1, 0, 2, 1]

Before: [3, 0, 0, 2]
3 3 3 2
After:  [3, 0, 0, 2]

Before: [2, 2, 1, 1]
4 2 3 3
After:  [2, 2, 1, 2]

Before: [0, 1, 1, 3]
9 2 1 3
After:  [0, 1, 1, 2]

Before: [0, 0, 2, 1]
11 0 0 2
After:  [0, 0, 0, 1]

Before: [2, 1, 2, 0]
7 2 0 2
After:  [2, 1, 1, 0]

Before: [2, 1, 2, 0]
15 1 2 3
After:  [2, 1, 2, 0]

Before: [1, 1, 3, 2]
3 3 3 3
After:  [1, 1, 3, 0]

Before: [1, 3, 3, 1]
12 3 1 3
After:  [1, 3, 3, 1]

Before: [1, 0, 2, 0]
1 0 1 0
After:  [1, 0, 2, 0]

Before: [2, 1, 0, 3]
13 1 0 3
After:  [2, 1, 0, 0]

Before: [0, 3, 3, 3]
7 3 2 1
After:  [0, 1, 3, 3]

Before: [2, 1, 0, 3]
2 1 3 3
After:  [2, 1, 0, 0]

Before: [1, 3, 2, 1]
12 3 1 2
After:  [1, 3, 1, 1]

Before: [1, 0, 2, 2]
5 0 2 1
After:  [1, 0, 2, 2]

Before: [0, 1, 1, 1]
1 1 0 1
After:  [0, 1, 1, 1]

Before: [0, 2, 3, 2]
3 3 3 2
After:  [0, 2, 0, 2]

Before: [0, 0, 2, 1]
10 3 2 0
After:  [1, 0, 2, 1]

Before: [2, 0, 1, 0]
1 2 1 0
After:  [1, 0, 1, 0]

Before: [2, 3, 3, 2]
0 2 0 1
After:  [2, 1, 3, 2]

Before: [2, 1, 3, 2]
0 2 0 2
After:  [2, 1, 1, 2]

Before: [0, 1, 0, 2]
11 0 0 0
After:  [0, 1, 0, 2]

Before: [2, 3, 3, 1]
7 2 3 0
After:  [0, 3, 3, 1]

Before: [0, 3, 2, 1]
10 3 2 2
After:  [0, 3, 1, 1]

Before: [0, 0, 2, 2]
6 2 3 2
After:  [0, 0, 1, 2]

Before: [3, 1, 1, 2]
9 2 1 3
After:  [3, 1, 1, 2]

Before: [0, 0, 3, 0]
6 0 3 2
After:  [0, 0, 1, 0]

Before: [3, 3, 3, 1]
12 3 1 1
After:  [3, 1, 3, 1]

Before: [2, 1, 1, 2]
13 1 0 2
After:  [2, 1, 0, 2]

Before: [0, 1, 2, 3]
2 1 3 1
After:  [0, 0, 2, 3]

Before: [0, 2, 2, 1]
3 3 3 3
After:  [0, 2, 2, 0]

Before: [1, 0, 2, 0]
5 0 2 3
After:  [1, 0, 2, 0]

Before: [3, 3, 2, 1]
10 3 2 2
After:  [3, 3, 1, 1]

Before: [1, 0, 1, 2]
4 2 0 0
After:  [2, 0, 1, 2]

Before: [0, 1, 1, 0]
9 2 1 0
After:  [2, 1, 1, 0]

Before: [0, 2, 2, 1]
10 3 2 2
After:  [0, 2, 1, 1]

Before: [1, 3, 1, 1]
3 3 3 0
After:  [0, 3, 1, 1]

Before: [1, 1, 2, 2]
0 3 2 0
After:  [0, 1, 2, 2]

Before: [0, 1, 2, 3]
2 2 3 0
After:  [0, 1, 2, 3]

Before: [0, 1, 1, 3]
2 1 3 0
After:  [0, 1, 1, 3]

Before: [0, 1, 3, 0]
6 0 3 2
After:  [0, 1, 1, 0]

Before: [0, 1, 1, 1]
4 2 3 0
After:  [2, 1, 1, 1]

Before: [1, 0, 2, 0]
5 0 2 2
After:  [1, 0, 0, 0]

Before: [1, 2, 1, 2]
4 2 0 3
After:  [1, 2, 1, 2]

Before: [3, 3, 3, 3]
7 3 2 1
After:  [3, 1, 3, 3]

Before: [0, 3, 1, 1]
4 2 3 3
After:  [0, 3, 1, 2]

Before: [1, 1, 2, 1]
15 1 2 0
After:  [0, 1, 2, 1]

Before: [1, 1, 2, 3]
15 1 2 1
After:  [1, 0, 2, 3]

Before: [1, 2, 3, 1]
3 3 3 1
After:  [1, 0, 3, 1]

Before: [3, 1, 2, 0]
15 1 2 0
After:  [0, 1, 2, 0]

Before: [1, 1, 3, 2]
8 1 3 1
After:  [1, 0, 3, 2]

Before: [3, 1, 2, 0]
15 1 2 3
After:  [3, 1, 2, 0]

Before: [0, 2, 2, 3]
11 0 0 3
After:  [0, 2, 2, 0]

Before: [0, 2, 2, 1]
10 3 2 1
After:  [0, 1, 2, 1]

Before: [3, 1, 1, 3]
9 2 1 0
After:  [2, 1, 1, 3]

Before: [3, 0, 2, 3]
7 3 0 0
After:  [1, 0, 2, 3]

Before: [3, 1, 3, 3]
7 3 2 2
After:  [3, 1, 1, 3]

Before: [3, 3, 2, 1]
12 3 1 0
After:  [1, 3, 2, 1]

Before: [2, 1, 2, 1]
10 3 2 1
After:  [2, 1, 2, 1]

Before: [2, 0, 3, 2]
0 2 0 2
After:  [2, 0, 1, 2]

Before: [0, 1, 1, 2]
9 2 1 1
After:  [0, 2, 1, 2]

Before: [0, 0, 1, 1]
1 3 1 0
After:  [1, 0, 1, 1]

Before: [3, 3, 0, 2]
0 0 2 2
After:  [3, 3, 1, 2]

Before: [3, 1, 2, 1]
15 1 2 2
After:  [3, 1, 0, 1]

Before: [2, 2, 2, 1]
10 3 2 1
After:  [2, 1, 2, 1]

Before: [1, 1, 0, 2]
8 1 3 1
After:  [1, 0, 0, 2]

Before: [1, 1, 1, 1]
3 3 3 0
After:  [0, 1, 1, 1]

Before: [3, 3, 2, 1]
10 3 2 3
After:  [3, 3, 2, 1]

Before: [2, 1, 1, 2]
8 1 3 3
After:  [2, 1, 1, 0]

Before: [2, 3, 0, 1]
12 3 1 3
After:  [2, 3, 0, 1]

Before: [1, 1, 1, 2]
8 1 3 3
After:  [1, 1, 1, 0]

Before: [1, 3, 3, 0]
14 3 2 3
After:  [1, 3, 3, 1]

Before: [1, 1, 2, 1]
5 0 2 2
After:  [1, 1, 0, 1]

Before: [3, 2, 2, 2]
3 3 3 3
After:  [3, 2, 2, 0]

Before: [2, 1, 1, 0]
13 1 0 1
After:  [2, 0, 1, 0]

Before: [1, 0, 2, 1]
1 0 1 0
After:  [1, 0, 2, 1]

Before: [0, 2, 2, 3]
11 0 0 2
After:  [0, 2, 0, 3]

Before: [2, 3, 1, 1]
12 3 1 0
After:  [1, 3, 1, 1]

Before: [3, 0, 0, 0]
0 0 2 3
After:  [3, 0, 0, 1]

Before: [0, 2, 0, 3]
6 0 2 2
After:  [0, 2, 1, 3]

Before: [2, 1, 1, 2]
9 2 1 3
After:  [2, 1, 1, 2]

Before: [2, 0, 0, 2]
0 0 1 3
After:  [2, 0, 0, 1]

Before: [1, 1, 2, 0]
15 1 2 1
After:  [1, 0, 2, 0]

Before: [2, 0, 0, 1]
0 0 1 0
After:  [1, 0, 0, 1]

Before: [0, 0, 3, 0]
6 0 3 3
After:  [0, 0, 3, 1]

Before: [3, 0, 3, 1]
3 3 3 1
After:  [3, 0, 3, 1]

Before: [3, 1, 1, 1]
4 2 3 3
After:  [3, 1, 1, 2]

Before: [2, 3, 2, 1]
10 3 2 1
After:  [2, 1, 2, 1]

Before: [1, 3, 1, 2]
3 3 3 0
After:  [0, 3, 1, 2]

Before: [3, 1, 1, 1]
0 2 1 0
After:  [0, 1, 1, 1]

Before: [0, 0, 1, 0]
11 0 0 3
After:  [0, 0, 1, 0]

Before: [0, 1, 2, 3]
11 0 0 1
After:  [0, 0, 2, 3]

Before: [1, 2, 2, 0]
5 0 2 1
After:  [1, 0, 2, 0]

Before: [2, 0, 3, 3]
7 3 2 1
After:  [2, 1, 3, 3]

Before: [3, 1, 0, 2]
0 0 2 2
After:  [3, 1, 1, 2]

Before: [2, 1, 2, 2]
8 1 3 3
After:  [2, 1, 2, 0]

Before: [0, 0, 1, 1]
4 2 3 2
After:  [0, 0, 2, 1]

Before: [2, 0, 1, 3]
1 2 1 2
After:  [2, 0, 1, 3]

Before: [0, 1, 3, 1]
7 2 3 0
After:  [0, 1, 3, 1]

Before: [0, 3, 1, 1]
11 0 0 0
After:  [0, 3, 1, 1]

Before: [0, 3, 2, 0]
6 0 3 3
After:  [0, 3, 2, 1]

Before: [0, 0, 3, 0]
14 3 2 2
After:  [0, 0, 1, 0]

Before: [0, 3, 2, 2]
6 2 3 1
After:  [0, 1, 2, 2]

Before: [1, 1, 3, 1]
7 2 3 0
After:  [0, 1, 3, 1]

Before: [3, 1, 0, 2]
8 1 3 1
After:  [3, 0, 0, 2]

Before: [1, 0, 1, 2]
4 2 0 3
After:  [1, 0, 1, 2]

Before: [3, 0, 0, 2]
14 2 3 3
After:  [3, 0, 0, 1]

Before: [2, 2, 2, 2]
7 2 1 3
After:  [2, 2, 2, 1]

Before: [0, 1, 2, 1]
11 0 0 0
After:  [0, 1, 2, 1]

Before: [2, 0, 1, 2]
3 3 3 3
After:  [2, 0, 1, 0]

Before: [0, 0, 1, 1]
4 2 3 0
After:  [2, 0, 1, 1]

Before: [2, 1, 0, 2]
14 2 3 3
After:  [2, 1, 0, 1]

Before: [0, 2, 1, 2]
3 3 3 0
After:  [0, 2, 1, 2]

Before: [0, 0, 0, 2]
14 2 3 1
After:  [0, 1, 0, 2]

Before: [3, 1, 1, 1]
0 3 1 1
After:  [3, 0, 1, 1]

Before: [2, 2, 3, 3]
2 1 3 0
After:  [0, 2, 3, 3]

Before: [0, 1, 1, 2]
9 2 1 3
After:  [0, 1, 1, 2]

Before: [1, 0, 0, 2]
1 0 1 1
After:  [1, 1, 0, 2]

Before: [3, 0, 0, 2]
14 2 3 2
After:  [3, 0, 1, 2]

Before: [1, 3, 2, 0]
5 0 2 0
After:  [0, 3, 2, 0]

Before: [2, 2, 3, 1]
0 2 0 2
After:  [2, 2, 1, 1]

Before: [1, 2, 0, 2]
14 2 3 0
After:  [1, 2, 0, 2]

Before: [1, 1, 3, 2]
3 3 3 1
After:  [1, 0, 3, 2]

Before: [0, 3, 3, 2]
11 0 0 0
After:  [0, 3, 3, 2]

Before: [0, 0, 1, 0]
11 0 0 0
After:  [0, 0, 1, 0]

Before: [1, 0, 1, 1]
4 2 0 2
After:  [1, 0, 2, 1]

Before: [2, 0, 2, 1]
0 0 1 3
After:  [2, 0, 2, 1]

Before: [0, 0, 2, 2]
6 2 3 3
After:  [0, 0, 2, 1]

Before: [0, 3, 1, 1]
11 0 0 3
After:  [0, 3, 1, 0]

Before: [2, 1, 2, 1]
10 3 2 0
After:  [1, 1, 2, 1]

Before: [3, 1, 3, 0]
14 3 2 0
After:  [1, 1, 3, 0]

Before: [0, 3, 2, 2]
0 3 2 3
After:  [0, 3, 2, 0]

Before: [0, 1, 2, 0]
15 1 2 2
After:  [0, 1, 0, 0]

Before: [2, 1, 2, 0]
13 1 0 1
After:  [2, 0, 2, 0]

Before: [1, 1, 2, 0]
15 1 2 2
After:  [1, 1, 0, 0]

Before: [1, 0, 1, 1]
4 2 3 0
After:  [2, 0, 1, 1]

Before: [0, 1, 3, 2]
8 1 3 1
After:  [0, 0, 3, 2]

Before: [2, 0, 1, 1]
3 2 3 1
After:  [2, 0, 1, 1]

Before: [1, 3, 1, 0]
4 2 0 1
After:  [1, 2, 1, 0]

Before: [0, 2, 3, 1]
11 0 0 1
After:  [0, 0, 3, 1]

Before: [2, 3, 1, 1]
12 3 1 2
After:  [2, 3, 1, 1]

Before: [0, 1, 0, 2]
8 1 3 2
After:  [0, 1, 0, 2]

Before: [3, 2, 1, 1]
3 2 3 3
After:  [3, 2, 1, 0]

Before: [0, 2, 3, 0]
14 3 2 2
After:  [0, 2, 1, 0]

Before: [1, 2, 3, 3]
7 3 2 1
After:  [1, 1, 3, 3]

Before: [0, 2, 1, 3]
2 1 3 3
After:  [0, 2, 1, 0]

Before: [1, 1, 1, 1]
0 2 1 3
After:  [1, 1, 1, 0]

Before: [1, 0, 0, 1]
1 3 1 0
After:  [1, 0, 0, 1]

Before: [1, 2, 1, 0]
4 2 0 2
After:  [1, 2, 2, 0]

Before: [2, 1, 1, 3]
9 2 1 0
After:  [2, 1, 1, 3]

Before: [0, 2, 2, 2]
6 2 3 1
After:  [0, 1, 2, 2]

Before: [3, 1, 1, 2]
0 2 1 2
After:  [3, 1, 0, 2]

Before: [2, 0, 1, 1]
1 2 1 1
After:  [2, 1, 1, 1]

Before: [0, 1, 2, 3]
1 1 0 2
After:  [0, 1, 1, 3]

Before: [3, 1, 3, 2]
8 1 3 2
After:  [3, 1, 0, 2]

Before: [1, 1, 1, 3]
2 1 3 0
After:  [0, 1, 1, 3]

Before: [1, 1, 2, 2]
6 2 3 0
After:  [1, 1, 2, 2]

Before: [0, 1, 2, 0]
15 1 2 1
After:  [0, 0, 2, 0]

Before: [0, 0, 2, 1]
11 0 0 3
After:  [0, 0, 2, 0]

Before: [1, 1, 1, 3]
0 2 1 2
After:  [1, 1, 0, 3]

Before: [0, 1, 1, 1]
4 2 3 3
After:  [0, 1, 1, 2]

Before: [2, 0, 1, 2]
0 0 1 3
After:  [2, 0, 1, 1]

Before: [2, 3, 3, 2]
3 3 3 3
After:  [2, 3, 3, 0]

Before: [2, 1, 3, 1]
0 3 1 0
After:  [0, 1, 3, 1]

Before: [1, 3, 2, 1]
3 3 3 3
After:  [1, 3, 2, 0]

Before: [0, 3, 1, 1]
3 3 3 1
After:  [0, 0, 1, 1]

Before: [3, 1, 1, 3]
2 1 3 3
After:  [3, 1, 1, 0]

Before: [1, 2, 1, 1]
4 2 0 2
After:  [1, 2, 2, 1]

Before: [3, 0, 3, 1]
1 3 1 1
After:  [3, 1, 3, 1]

Before: [0, 0, 1, 1]
11 0 0 1
After:  [0, 0, 1, 1]

Before: [1, 3, 2, 1]
10 3 2 3
After:  [1, 3, 2, 1]

Before: [3, 0, 2, 2]
6 2 3 1
After:  [3, 1, 2, 2]

Before: [2, 1, 0, 2]
3 3 3 2
After:  [2, 1, 0, 2]

Before: [1, 3, 3, 1]
12 3 1 0
After:  [1, 3, 3, 1]

Before: [3, 1, 1, 0]
0 2 1 0
After:  [0, 1, 1, 0]

Before: [1, 0, 2, 3]
2 2 3 0
After:  [0, 0, 2, 3]

Before: [2, 1, 0, 2]
3 3 3 3
After:  [2, 1, 0, 0]

Before: [0, 1, 3, 2]
8 1 3 2
After:  [0, 1, 0, 2]

Before: [1, 0, 2, 3]
5 0 2 0
After:  [0, 0, 2, 3]

Before: [2, 1, 3, 0]
13 1 0 3
After:  [2, 1, 3, 0]

Before: [0, 3, 2, 1]
12 3 1 0
After:  [1, 3, 2, 1]

Before: [0, 2, 0, 3]
11 0 0 1
After:  [0, 0, 0, 3]

Before: [0, 3, 0, 3]
11 0 0 1
After:  [0, 0, 0, 3]

Before: [2, 1, 0, 2]
13 1 0 1
After:  [2, 0, 0, 2]

Before: [0, 1, 1, 0]
9 2 1 3
After:  [0, 1, 1, 2]

Before: [2, 1, 2, 2]
8 1 3 2
After:  [2, 1, 0, 2]

Before: [0, 1, 2, 1]
15 1 2 1
After:  [0, 0, 2, 1]

Before: [1, 3, 2, 1]
5 0 2 3
After:  [1, 3, 2, 0]

Before: [3, 3, 3, 0]
14 3 2 1
After:  [3, 1, 3, 0]

Before: [1, 0, 1, 0]
4 2 0 1
After:  [1, 2, 1, 0]

Before: [0, 1, 2, 1]
10 3 2 3
After:  [0, 1, 2, 1]

Before: [3, 1, 0, 2]
0 0 2 0
After:  [1, 1, 0, 2]

Before: [0, 0, 0, 2]
14 2 3 0
After:  [1, 0, 0, 2]

Before: [2, 3, 3, 0]
14 3 2 2
After:  [2, 3, 1, 0]

Before: [3, 2, 1, 3]
2 2 3 2
After:  [3, 2, 0, 3]

Before: [3, 2, 2, 2]
0 3 2 1
After:  [3, 0, 2, 2]

Before: [3, 1, 1, 2]
9 2 1 1
After:  [3, 2, 1, 2]

Before: [3, 0, 1, 3]
1 2 1 0
After:  [1, 0, 1, 3]

Before: [0, 0, 2, 1]
1 3 1 2
After:  [0, 0, 1, 1]

Before: [0, 2, 0, 1]
6 0 2 2
After:  [0, 2, 1, 1]

Before: [3, 2, 2, 3]
7 2 1 3
After:  [3, 2, 2, 1]

Before: [1, 1, 0, 2]
8 1 3 3
After:  [1, 1, 0, 0]

Before: [2, 1, 1, 0]
9 2 1 1
After:  [2, 2, 1, 0]

Before: [3, 2, 0, 2]
14 2 3 3
After:  [3, 2, 0, 1]

Before: [2, 1, 3, 2]
13 1 0 0
After:  [0, 1, 3, 2]

Before: [1, 2, 2, 1]
10 3 2 0
After:  [1, 2, 2, 1]

Before: [3, 0, 1, 3]
7 3 0 1
After:  [3, 1, 1, 3]

Before: [0, 1, 2, 1]
0 3 1 3
After:  [0, 1, 2, 0]

Before: [0, 1, 2, 2]
15 1 2 0
After:  [0, 1, 2, 2]

Before: [0, 3, 3, 1]
11 0 0 2
After:  [0, 3, 0, 1]

Before: [1, 3, 3, 1]
12 3 1 1
After:  [1, 1, 3, 1]

Before: [0, 1, 0, 2]
8 1 3 0
After:  [0, 1, 0, 2]

Before: [3, 1, 1, 1]
3 2 3 2
After:  [3, 1, 0, 1]

Before: [0, 2, 0, 2]
6 0 2 2
After:  [0, 2, 1, 2]

Before: [0, 1, 2, 2]
15 1 2 3
After:  [0, 1, 2, 0]

Before: [1, 0, 2, 2]
6 2 3 2
After:  [1, 0, 1, 2]

Before: [0, 3, 0, 1]
12 3 1 0
After:  [1, 3, 0, 1]

Before: [0, 1, 1, 3]
9 2 1 2
After:  [0, 1, 2, 3]

Before: [2, 1, 2, 3]
15 1 2 1
After:  [2, 0, 2, 3]

Before: [1, 1, 3, 2]
8 1 3 2
After:  [1, 1, 0, 2]

Before: [1, 2, 2, 0]
5 0 2 3
After:  [1, 2, 2, 0]

Before: [3, 0, 3, 0]
14 3 2 2
After:  [3, 0, 1, 0]

Before: [1, 1, 1, 0]
4 2 0 3
After:  [1, 1, 1, 2]

Before: [1, 0, 2, 1]
10 3 2 3
After:  [1, 0, 2, 1]

Before: [0, 0, 1, 2]
11 0 0 3
After:  [0, 0, 1, 0]

Before: [0, 0, 1, 0]
1 2 1 3
After:  [0, 0, 1, 1]

Before: [0, 1, 3, 0]
14 3 2 2
After:  [0, 1, 1, 0]

Before: [0, 2, 0, 3]
6 0 2 0
After:  [1, 2, 0, 3]

Before: [1, 1, 2, 2]
5 0 2 3
After:  [1, 1, 2, 0]

Before: [3, 1, 3, 1]
3 3 3 3
After:  [3, 1, 3, 0]

Before: [0, 0, 0, 1]
1 3 1 3
After:  [0, 0, 0, 1]

Before: [2, 1, 1, 0]
9 2 1 2
After:  [2, 1, 2, 0]

Before: [0, 3, 0, 3]
11 0 0 0
After:  [0, 3, 0, 3]

Before: [0, 2, 1, 1]
4 2 3 1
After:  [0, 2, 1, 1]

Before: [3, 1, 3, 3]
2 1 3 2
After:  [3, 1, 0, 3]

Before: [0, 3, 0, 1]
12 3 1 3
After:  [0, 3, 0, 1]

Before: [3, 0, 1, 1]
4 2 3 1
After:  [3, 2, 1, 1]

Before: [1, 1, 2, 2]
8 1 3 0
After:  [0, 1, 2, 2]

Before: [1, 1, 2, 2]
8 1 3 2
After:  [1, 1, 0, 2]

Before: [0, 1, 3, 3]
2 1 3 1
After:  [0, 0, 3, 3]

Before: [1, 0, 3, 0]
14 3 2 1
After:  [1, 1, 3, 0]

Before: [2, 1, 0, 1]
3 3 3 3
After:  [2, 1, 0, 0]

Before: [0, 3, 0, 2]
11 0 0 2
After:  [0, 3, 0, 2]

Before: [1, 3, 0, 1]
12 3 1 1
After:  [1, 1, 0, 1]

Before: [1, 3, 2, 0]
5 0 2 2
After:  [1, 3, 0, 0]

Before: [1, 1, 0, 2]
14 2 3 0
After:  [1, 1, 0, 2]

Before: [3, 1, 2, 3]
15 1 2 2
After:  [3, 1, 0, 3]

Before: [3, 3, 1, 3]
2 2 3 0
After:  [0, 3, 1, 3]

Before: [1, 2, 3, 0]
14 3 2 1
After:  [1, 1, 3, 0]

Before: [0, 0, 1, 2]
1 2 1 2
After:  [0, 0, 1, 2]

Before: [0, 1, 1, 2]
8 1 3 2
After:  [0, 1, 0, 2]

Before: [0, 3, 3, 1]
3 3 3 3
After:  [0, 3, 3, 0]

Before: [3, 1, 1, 1]
9 2 1 2
After:  [3, 1, 2, 1]

Before: [1, 2, 1, 1]
4 2 3 2
After:  [1, 2, 2, 1]

Before: [1, 3, 1, 1]
12 3 1 3
After:  [1, 3, 1, 1]

Before: [1, 0, 0, 2]
14 2 3 2
After:  [1, 0, 1, 2]

Before: [2, 2, 0, 1]
3 3 3 1
After:  [2, 0, 0, 1]

Before: [2, 1, 1, 2]
13 1 0 1
After:  [2, 0, 1, 2]

Before: [1, 1, 1, 2]
4 2 0 0
After:  [2, 1, 1, 2]

Before: [1, 3, 2, 1]
10 3 2 0
After:  [1, 3, 2, 1]

Before: [0, 1, 1, 2]
8 1 3 1
After:  [0, 0, 1, 2]

Before: [2, 1, 3, 1]
13 1 0 1
After:  [2, 0, 3, 1]

Before: [0, 2, 1, 3]
2 2 3 2
After:  [0, 2, 0, 3]

Before: [1, 1, 0, 2]
14 2 3 1
After:  [1, 1, 0, 2]

Before: [2, 1, 0, 2]
8 1 3 3
After:  [2, 1, 0, 0]

Before: [1, 0, 2, 1]
10 3 2 1
After:  [1, 1, 2, 1]

Before: [3, 2, 2, 3]
7 2 1 0
After:  [1, 2, 2, 3]

Before: [1, 0, 1, 3]
1 0 1 1
After:  [1, 1, 1, 3]

Before: [2, 3, 2, 1]
3 3 3 3
After:  [2, 3, 2, 0]

Before: [1, 3, 1, 3]
4 2 0 2
After:  [1, 3, 2, 3]

Before: [2, 1, 2, 1]
13 1 0 2
After:  [2, 1, 0, 1]

Before: [0, 0, 1, 0]
6 0 3 2
After:  [0, 0, 1, 0]

Before: [0, 3, 0, 1]
11 0 0 1
After:  [0, 0, 0, 1]

Before: [1, 1, 2, 2]
6 2 3 1
After:  [1, 1, 2, 2]

Before: [0, 2, 0, 0]
6 0 2 0
After:  [1, 2, 0, 0]

Before: [3, 1, 2, 3]
7 3 0 3
After:  [3, 1, 2, 1]

Before: [1, 2, 2, 1]
5 0 2 1
After:  [1, 0, 2, 1]

Before: [1, 2, 2, 0]
5 0 2 2
After:  [1, 2, 0, 0]

Before: [2, 0, 3, 1]
0 0 1 1
After:  [2, 1, 3, 1]

Before: [1, 3, 2, 3]
5 0 2 1
After:  [1, 0, 2, 3]

Before: [0, 2, 2, 2]
11 0 0 2
After:  [0, 2, 0, 2]

Before: [2, 1, 0, 3]
13 1 0 0
After:  [0, 1, 0, 3]

Before: [1, 1, 3, 2]
8 1 3 0
After:  [0, 1, 3, 2]

Before: [0, 1, 2, 1]
10 3 2 2
After:  [0, 1, 1, 1]

Before: [2, 0, 2, 0]
0 0 1 1
After:  [2, 1, 2, 0]

Before: [0, 2, 1, 0]
11 0 0 0
After:  [0, 2, 1, 0]

Before: [0, 2, 0, 3]
6 0 2 3
After:  [0, 2, 0, 1]

Before: [2, 2, 2, 3]
7 2 0 2
After:  [2, 2, 1, 3]

Before: [2, 1, 2, 1]
10 3 2 3
After:  [2, 1, 2, 1]

Before: [2, 3, 3, 0]
14 3 2 3
After:  [2, 3, 3, 1]

Before: [3, 0, 2, 3]
2 2 3 1
After:  [3, 0, 2, 3]

Before: [3, 3, 3, 1]
12 3 1 2
After:  [3, 3, 1, 1]

Before: [2, 3, 2, 2]
6 2 3 1
After:  [2, 1, 2, 2]

Before: [3, 3, 1, 1]
4 2 3 3
After:  [3, 3, 1, 2]

Before: [2, 2, 3, 3]
0 2 0 1
After:  [2, 1, 3, 3]

Before: [1, 0, 2, 0]
5 0 2 0
After:  [0, 0, 2, 0]

Before: [1, 3, 1, 2]
3 3 3 3
After:  [1, 3, 1, 0]

Before: [0, 1, 1, 0]
1 1 0 2
After:  [0, 1, 1, 0]

Before: [1, 3, 3, 0]
14 3 2 0
After:  [1, 3, 3, 0]

Before: [0, 1, 3, 2]
8 1 3 3
After:  [0, 1, 3, 0]

Before: [0, 1, 0, 1]
1 1 0 3
After:  [0, 1, 0, 1]

Before: [1, 0, 1, 2]
1 2 1 0
After:  [1, 0, 1, 2]

Before: [0, 1, 2, 0]
6 0 3 1
After:  [0, 1, 2, 0]

Before: [3, 1, 1, 2]
3 3 3 1
After:  [3, 0, 1, 2]

Before: [0, 3, 2, 1]
10 3 2 0
After:  [1, 3, 2, 1]

Before: [2, 1, 1, 1]
0 2 1 2
After:  [2, 1, 0, 1]

Before: [1, 1, 2, 0]
5 0 2 2
After:  [1, 1, 0, 0]

Before: [1, 1, 2, 0]
5 0 2 1
After:  [1, 0, 2, 0]

Before: [2, 1, 2, 3]
2 1 3 0
After:  [0, 1, 2, 3]

Before: [2, 1, 2, 0]
15 1 2 1
After:  [2, 0, 2, 0]

Before: [1, 3, 0, 1]
12 3 1 0
After:  [1, 3, 0, 1]

Before: [2, 1, 1, 2]
8 1 3 2
After:  [2, 1, 0, 2]

Before: [0, 3, 0, 1]
6 0 2 1
After:  [0, 1, 0, 1]

Before: [0, 1, 0, 2]
8 1 3 3
After:  [0, 1, 0, 0]

Before: [2, 1, 2, 2]
15 1 2 1
After:  [2, 0, 2, 2]

Before: [1, 1, 1, 3]
4 2 0 2
After:  [1, 1, 2, 3]

Before: [0, 2, 1, 3]
11 0 0 0
After:  [0, 2, 1, 3]

Before: [3, 0, 2, 1]
10 3 2 3
After:  [3, 0, 2, 1]

Before: [1, 0, 3, 0]
14 3 2 3
After:  [1, 0, 3, 1]

Before: [3, 1, 2, 2]
15 1 2 2
After:  [3, 1, 0, 2]

Before: [1, 1, 1, 2]
0 2 1 0
After:  [0, 1, 1, 2]

Before: [0, 2, 0, 1]
3 3 3 1
After:  [0, 0, 0, 1]

Before: [1, 2, 2, 1]
10 3 2 1
After:  [1, 1, 2, 1]

Before: [3, 2, 2, 2]
0 3 2 3
After:  [3, 2, 2, 0]

Before: [3, 1, 2, 1]
15 1 2 3
After:  [3, 1, 2, 0]

Before: [1, 1, 0, 3]
2 1 3 2
After:  [1, 1, 0, 3]

Before: [3, 1, 0, 3]
7 3 0 3
After:  [3, 1, 0, 1]

Before: [0, 1, 3, 3]
11 0 0 3
After:  [0, 1, 3, 0]

Before: [3, 1, 3, 0]
14 3 2 2
After:  [3, 1, 1, 0]

Before: [3, 1, 0, 2]
8 1 3 3
After:  [3, 1, 0, 0]

Before: [2, 3, 3, 0]
0 2 0 0
After:  [1, 3, 3, 0]

Before: [3, 2, 3, 3]
7 3 0 0
After:  [1, 2, 3, 3]

Before: [1, 0, 2, 3]
5 0 2 2
After:  [1, 0, 0, 3]

Before: [0, 1, 3, 0]
11 0 0 2
After:  [0, 1, 0, 0]

Before: [0, 0, 2, 1]
10 3 2 3
After:  [0, 0, 2, 1]

Before: [1, 1, 1, 3]
4 2 0 3
After:  [1, 1, 1, 2]

Before: [3, 2, 2, 1]
10 3 2 3
After:  [3, 2, 2, 1]

Before: [2, 3, 3, 1]
12 3 1 0
After:  [1, 3, 3, 1]

Before: [3, 1, 2, 3]
15 1 2 0
After:  [0, 1, 2, 3]

Before: [1, 3, 3, 3]
7 3 2 3
After:  [1, 3, 3, 1]

Before: [1, 3, 2, 3]
5 0 2 3
After:  [1, 3, 2, 0]

Before: [1, 0, 3, 0]
14 3 2 0
After:  [1, 0, 3, 0]

Before: [2, 0, 2, 1]
0 0 1 2
After:  [2, 0, 1, 1]

Before: [3, 2, 2, 2]
6 2 3 1
After:  [3, 1, 2, 2]

Before: [2, 0, 3, 0]
14 3 2 3
After:  [2, 0, 3, 1]

Before: [1, 1, 1, 3]
9 2 1 3
After:  [1, 1, 1, 2]

Before: [2, 1, 3, 3]
13 1 0 3
After:  [2, 1, 3, 0]

Before: [2, 3, 2, 3]
2 2 3 1
After:  [2, 0, 2, 3]

Before: [1, 1, 2, 1]
10 3 2 0
After:  [1, 1, 2, 1]

Before: [1, 1, 2, 2]
15 1 2 2
After:  [1, 1, 0, 2]

Before: [2, 2, 3, 0]
14 3 2 3
After:  [2, 2, 3, 1]

Before: [2, 1, 1, 1]
13 1 0 3
After:  [2, 1, 1, 0]

Before: [0, 2, 1, 1]
4 2 3 0
After:  [2, 2, 1, 1]

Before: [2, 0, 0, 3]
0 0 1 0
After:  [1, 0, 0, 3]

Before: [0, 3, 0, 0]
11 0 0 2
After:  [0, 3, 0, 0]

Before: [2, 1, 1, 0]
9 2 1 3
After:  [2, 1, 1, 2]

Before: [1, 1, 2, 3]
15 1 2 3
After:  [1, 1, 2, 0]

Before: [3, 2, 3, 3]
2 1 3 0
After:  [0, 2, 3, 3]

Before: [2, 0, 1, 0]
0 0 1 3
After:  [2, 0, 1, 1]

Before: [2, 1, 1, 3]
0 2 1 1
After:  [2, 0, 1, 3]

Before: [1, 1, 1, 1]
4 2 3 3
After:  [1, 1, 1, 2]

Before: [2, 1, 1, 1]
9 2 1 1
After:  [2, 2, 1, 1]

Before: [1, 1, 2, 3]
5 0 2 1
After:  [1, 0, 2, 3]

Before: [0, 1, 1, 0]
6 0 3 2
After:  [0, 1, 1, 0]

Before: [3, 3, 0, 2]
14 2 3 0
After:  [1, 3, 0, 2]

Before: [3, 2, 2, 1]
10 3 2 2
After:  [3, 2, 1, 1]

Before: [1, 2, 2, 0]
5 0 2 0
After:  [0, 2, 2, 0]

Before: [2, 0, 2, 2]
0 3 2 0
After:  [0, 0, 2, 2]

Before: [0, 3, 2, 1]
12 3 1 3
After:  [0, 3, 2, 1]

Before: [0, 0, 1, 2]
1 2 1 3
After:  [0, 0, 1, 1]

Before: [1, 1, 3, 2]
8 1 3 3
After:  [1, 1, 3, 0]

Before: [1, 1, 2, 0]
15 1 2 3
After:  [1, 1, 2, 0]

Before: [1, 0, 2, 3]
5 0 2 3
After:  [1, 0, 2, 0]

Before: [0, 1, 1, 2]
9 2 1 2
After:  [0, 1, 2, 2]

Before: [2, 1, 2, 0]
15 1 2 2
After:  [2, 1, 0, 0]

Before: [1, 3, 3, 1]
12 3 1 2
After:  [1, 3, 1, 1]

Before: [2, 0, 2, 1]
3 3 3 2
After:  [2, 0, 0, 1]

Before: [0, 2, 2, 3]
2 2 3 1
After:  [0, 0, 2, 3]");

        }
    }
}
