﻿using System;
using System.Collections.Generic;
using System.Linq;
using AoC;

namespace AOC.Year2022.Day5
{
    public class Part2 : BasePart
    {
        // 11:01:05 - 11:12:46, time: 11m41s, 100 @ 7m58s
        protected void RunScenario(string title, string input)
        {
            RunScenario(title, () =>
            {
                var lines = input.Replace("\r\n", "\n").Split("\n", StringSplitOptions.None);

                var stacks = Enumerable.Range(0, 20).Select(i => new Stack<char>()).ToList();

                var blankLineIx = lines.Select((i, ix) => new { i, ix }).Where(i => i.i == "").Select(i => i.ix).Single();
                Console.WriteLine(lines[blankLineIx]);

                for (var i = blankLineIx - 2; i >= 0; i--)
                {
                    for (var ix = 1; ix < lines[i].Length; ix += 4)
                    {
                        var value = lines[i][ix];
                        if (value == ' ') continue;
                        stacks[(ix - 1) / 4 + 1].Push(value);
                    }
                }

                foreach (var line in lines.Skip(blankLineIx + 1))
                {
                    var parts = line.Split(" ");
                    var q = int.Parse(parts[1]);
                    var from = stacks[int.Parse(parts[3])];
                    var to = stacks[int.Parse(parts[5])];
                    var temp = new Stack<char>();
                    for (var i = 0; i < q; i++)
                    {
                        temp.Push(from.Pop());
                    }
                    for (var i = 0; i < q; i++)
                    {
                        to.Push(temp.Pop());
                    }
                }

                var result = new String(stacks.Select(i => i.TryPeek(out var x) ? (char?)x : null).Where(i => i.HasValue).OfType<char>().ToArray());

                Console.WriteLine(result);
            });
        }

        public override void Run()
        {
            RunScenario("initial", @"    [D]    
[N] [C]    
[Z] [M] [P]
 1   2   3 

move 1 from 2 to 1
move 3 from 1 to 3
move 2 from 2 to 1
move 1 from 1 to 2");
            //return;
            RunScenario("part1", @"[H]                 [Z]         [J]
[L]     [W] [B]     [G]         [R]
[R]     [G] [S]     [J] [H]     [Q]
[F]     [N] [T] [J] [P] [R]     [F]
[B]     [C] [M] [R] [Q] [F] [G] [P]
[C] [D] [F] [D] [D] [D] [T] [M] [G]
[J] [C] [J] [J] [C] [L] [Z] [V] [B]
[M] [Z] [H] [P] [N] [W] [P] [L] [C]
 1   2   3   4   5   6   7   8   9 

move 3 from 2 to 1
move 8 from 6 to 4
move 4 from 8 to 2
move 3 from 1 to 9
move 1 from 2 to 4
move 3 from 7 to 5
move 3 from 9 to 2
move 3 from 3 to 5
move 1 from 5 to 1
move 5 from 1 to 8
move 2 from 1 to 8
move 3 from 7 to 3
move 1 from 8 to 9
move 6 from 9 to 8
move 3 from 8 to 7
move 7 from 8 to 9
move 2 from 5 to 9
move 2 from 2 to 9
move 3 from 3 to 7
move 2 from 8 to 3
move 7 from 4 to 8
move 3 from 4 to 1
move 4 from 8 to 6
move 4 from 6 to 1
move 8 from 1 to 2
move 1 from 1 to 4
move 3 from 5 to 1
move 8 from 9 to 8
move 4 from 3 to 1
move 5 from 5 to 3
move 2 from 7 to 1
move 1 from 7 to 4
move 1 from 7 to 2
move 3 from 3 to 5
move 3 from 9 to 1
move 9 from 8 to 1
move 2 from 9 to 7
move 1 from 8 to 5
move 4 from 5 to 3
move 1 from 3 to 4
move 1 from 9 to 6
move 1 from 6 to 9
move 7 from 4 to 9
move 1 from 7 to 3
move 1 from 8 to 2
move 8 from 2 to 1
move 4 from 3 to 5
move 2 from 9 to 6
move 2 from 6 to 2
move 2 from 4 to 9
move 8 from 9 to 2
move 3 from 7 to 9
move 1 from 3 to 5
move 2 from 3 to 8
move 9 from 2 to 1
move 1 from 8 to 7
move 4 from 2 to 9
move 4 from 5 to 6
move 1 from 8 to 9
move 27 from 1 to 2
move 1 from 6 to 4
move 3 from 6 to 4
move 7 from 9 to 8
move 4 from 4 to 1
move 9 from 2 to 6
move 2 from 1 to 9
move 6 from 1 to 3
move 1 from 5 to 3
move 3 from 3 to 5
move 3 from 5 to 3
move 3 from 3 to 1
move 4 from 6 to 7
move 3 from 9 to 2
move 1 from 6 to 4
move 4 from 3 to 5
move 3 from 6 to 5
move 1 from 6 to 2
move 15 from 2 to 3
move 5 from 5 to 9
move 13 from 3 to 9
move 2 from 5 to 7
move 1 from 4 to 2
move 3 from 3 to 7
move 11 from 2 to 7
move 7 from 9 to 5
move 3 from 5 to 7
move 6 from 8 to 9
move 4 from 1 to 2
move 6 from 1 to 6
move 3 from 5 to 1
move 1 from 8 to 2
move 4 from 2 to 9
move 1 from 5 to 7
move 6 from 7 to 6
move 18 from 7 to 5
move 1 from 7 to 1
move 8 from 9 to 5
move 1 from 2 to 6
move 15 from 5 to 6
move 6 from 5 to 3
move 4 from 3 to 6
move 26 from 6 to 5
move 2 from 1 to 7
move 4 from 5 to 9
move 8 from 5 to 7
move 3 from 7 to 9
move 14 from 9 to 8
move 7 from 5 to 2
move 4 from 2 to 1
move 5 from 1 to 9
move 12 from 5 to 3
move 5 from 8 to 5
move 14 from 3 to 2
move 1 from 5 to 2
move 10 from 2 to 6
move 7 from 9 to 6
move 6 from 8 to 6
move 1 from 2 to 7
move 2 from 9 to 7
move 2 from 8 to 6
move 6 from 2 to 7
move 1 from 1 to 8
move 15 from 6 to 2
move 1 from 6 to 9
move 1 from 5 to 9
move 1 from 9 to 6
move 2 from 2 to 4
move 3 from 9 to 5
move 5 from 5 to 3
move 3 from 3 to 6
move 6 from 2 to 7
move 1 from 5 to 9
move 8 from 6 to 9
move 2 from 6 to 7
move 3 from 2 to 4
move 9 from 6 to 7
move 17 from 7 to 5
move 1 from 8 to 4
move 7 from 9 to 3
move 12 from 5 to 8
move 3 from 5 to 2
move 4 from 7 to 8
move 2 from 5 to 7
move 1 from 7 to 9
move 8 from 3 to 7
move 17 from 7 to 5
move 3 from 2 to 5
move 1 from 3 to 6
move 10 from 5 to 4
move 5 from 2 to 7
move 1 from 4 to 2
move 3 from 9 to 8
move 7 from 7 to 2
move 5 from 5 to 1
move 14 from 4 to 9
move 3 from 9 to 8
move 1 from 6 to 9
move 2 from 1 to 4
move 2 from 8 to 5
move 16 from 8 to 6
move 1 from 6 to 2
move 11 from 9 to 2
move 2 from 7 to 5
move 1 from 1 to 6
move 11 from 2 to 9
move 4 from 2 to 8
move 9 from 5 to 3
move 1 from 4 to 2
move 2 from 1 to 8
move 1 from 2 to 9
move 2 from 4 to 3
move 8 from 6 to 9
move 16 from 9 to 3
move 16 from 3 to 2
move 17 from 2 to 6
move 1 from 9 to 3
move 1 from 2 to 5
move 1 from 9 to 4
move 3 from 2 to 8
move 1 from 9 to 1
move 1 from 9 to 6
move 7 from 3 to 1
move 5 from 3 to 5
move 3 from 8 to 3
move 2 from 3 to 4
move 6 from 8 to 4
move 7 from 6 to 4
move 3 from 6 to 7
move 3 from 8 to 9
move 3 from 5 to 2
move 3 from 1 to 3
move 1 from 4 to 8
move 3 from 5 to 1
move 13 from 4 to 7
move 14 from 6 to 7
move 6 from 1 to 9
move 3 from 9 to 6
move 1 from 8 to 7
move 1 from 8 to 7
move 20 from 7 to 3
move 1 from 8 to 9
move 1 from 1 to 9
move 1 from 1 to 5
move 1 from 4 to 6
move 14 from 3 to 9
move 1 from 2 to 6
move 3 from 7 to 6
move 6 from 3 to 2
move 1 from 3 to 8
move 2 from 7 to 3
move 7 from 6 to 3
move 12 from 3 to 1
move 1 from 8 to 2
move 1 from 4 to 9
move 1 from 5 to 6
move 1 from 6 to 4
move 1 from 4 to 2
move 2 from 2 to 3
move 16 from 9 to 7
move 3 from 6 to 7
move 6 from 9 to 4
move 4 from 4 to 7
move 6 from 1 to 8
move 2 from 3 to 6
move 3 from 1 to 9
move 3 from 2 to 3
move 3 from 3 to 8
move 5 from 2 to 8
move 2 from 7 to 8
move 3 from 1 to 5
move 1 from 4 to 3
move 2 from 9 to 8
move 1 from 6 to 8
move 2 from 9 to 1
move 15 from 7 to 1
move 1 from 6 to 5
move 10 from 1 to 5
move 1 from 4 to 1
move 2 from 1 to 6
move 9 from 7 to 8
move 27 from 8 to 3
move 1 from 6 to 1
move 1 from 8 to 5
move 5 from 5 to 6
move 12 from 3 to 1
move 3 from 7 to 1
move 7 from 5 to 1
move 1 from 6 to 4
move 3 from 6 to 9
move 1 from 4 to 2
move 2 from 6 to 5
move 1 from 7 to 6
move 1 from 9 to 2
move 2 from 5 to 6
move 2 from 6 to 5
move 3 from 1 to 3
move 19 from 3 to 1
move 2 from 2 to 9
move 42 from 1 to 7
move 4 from 9 to 7
move 1 from 6 to 8
move 1 from 8 to 5
move 2 from 1 to 9
move 3 from 5 to 7
move 27 from 7 to 4
move 1 from 1 to 4
move 3 from 9 to 2
move 18 from 4 to 9
move 2 from 5 to 3
move 1 from 7 to 1
move 2 from 3 to 4
move 8 from 7 to 5
move 15 from 9 to 3
move 1 from 9 to 7
move 3 from 7 to 2
move 2 from 7 to 2
move 2 from 5 to 3
move 1 from 1 to 5
move 1 from 9 to 1
move 1 from 3 to 1
move 1 from 4 to 3
move 8 from 7 to 3
move 8 from 2 to 4
move 1 from 9 to 6
move 23 from 3 to 9
move 1 from 9 to 6
move 2 from 6 to 8
move 1 from 8 to 6
move 1 from 5 to 3
move 7 from 4 to 8
move 7 from 5 to 7
move 2 from 8 to 3
move 1 from 1 to 8
move 3 from 7 to 4
move 5 from 4 to 3
move 1 from 1 to 8
move 3 from 3 to 1
move 8 from 9 to 7
move 3 from 8 to 4
move 1 from 6 to 2
move 5 from 8 to 7
move 6 from 3 to 1
move 1 from 2 to 9
move 7 from 7 to 9
move 4 from 1 to 9
move 2 from 4 to 2
move 1 from 4 to 9
move 1 from 1 to 6
move 8 from 4 to 8
move 4 from 1 to 5
move 3 from 5 to 2
move 2 from 2 to 5
move 2 from 5 to 6
move 1 from 3 to 7
move 2 from 6 to 4
move 1 from 5 to 7
move 1 from 6 to 9
move 1 from 4 to 1
move 6 from 9 to 2
move 8 from 9 to 7
move 4 from 7 to 3
move 4 from 8 to 3
move 3 from 8 to 3
move 8 from 3 to 5
move 1 from 1 to 7
move 11 from 9 to 7
move 5 from 2 to 7
move 1 from 8 to 1
move 3 from 2 to 3
move 1 from 1 to 4
move 1 from 2 to 5
move 20 from 7 to 8
move 7 from 7 to 9
move 4 from 4 to 7
move 3 from 9 to 4
move 5 from 7 to 4
move 7 from 4 to 7
move 4 from 9 to 2
move 1 from 4 to 3
move 4 from 3 to 5
move 2 from 5 to 8
move 4 from 5 to 2
move 5 from 2 to 6
move 2 from 6 to 3
move 22 from 8 to 5
move 13 from 7 to 9
move 11 from 9 to 3
move 2 from 6 to 8
move 7 from 3 to 1
move 18 from 5 to 2
move 1 from 6 to 4
move 1 from 4 to 9
move 2 from 8 to 5
move 2 from 9 to 1
move 9 from 3 to 1
move 4 from 5 to 6
move 2 from 6 to 7
move 3 from 9 to 5
move 10 from 5 to 8
move 6 from 8 to 7
move 3 from 8 to 1
move 6 from 2 to 3
move 1 from 9 to 6
move 5 from 3 to 4
move 4 from 1 to 4
move 17 from 1 to 5
move 12 from 2 to 7
move 1 from 3 to 6
move 16 from 5 to 8
move 3 from 5 to 6
move 9 from 8 to 3
move 8 from 8 to 4
move 7 from 4 to 1
move 5 from 1 to 4
move 4 from 3 to 7
move 14 from 7 to 3
move 6 from 4 to 8
move 9 from 7 to 4
move 5 from 6 to 1
move 1 from 7 to 1
move 1 from 6 to 7
move 16 from 4 to 5
move 1 from 4 to 2
move 1 from 7 to 5
move 2 from 1 to 7
move 2 from 7 to 4
move 4 from 1 to 6
move 13 from 5 to 6
move 5 from 6 to 3
move 22 from 3 to 2
move 1 from 4 to 7
move 4 from 5 to 4
move 1 from 7 to 6
move 5 from 8 to 5
move 2 from 3 to 1
move 13 from 6 to 1
move 6 from 1 to 4
move 1 from 8 to 1
move 6 from 1 to 4
move 1 from 5 to 4
move 7 from 4 to 7
move 3 from 1 to 5
move 2 from 5 to 7
move 5 from 5 to 1
move 8 from 7 to 4
move 1 from 6 to 4
move 1 from 7 to 4
move 9 from 2 to 7
move 8 from 7 to 6
move 5 from 6 to 4
move 1 from 7 to 4
move 2 from 4 to 9
move 2 from 6 to 1
move 8 from 2 to 6
move 9 from 1 to 8
move 9 from 6 to 2
move 1 from 1 to 8
move 6 from 8 to 4
move 2 from 9 to 7
move 2 from 7 to 9
move 15 from 2 to 8
move 18 from 4 to 2
move 14 from 4 to 5
move 10 from 2 to 4
move 9 from 2 to 6
move 1 from 9 to 3
move 1 from 3 to 1
move 6 from 5 to 8
move 3 from 4 to 9
move 2 from 2 to 1
move 1 from 1 to 6
move 3 from 9 to 7
move 22 from 8 to 6
move 1 from 8 to 9
move 2 from 1 to 5
move 5 from 5 to 4
move 2 from 5 to 8
move 2 from 8 to 7
move 1 from 9 to 7
move 1 from 5 to 8
move 1 from 9 to 8
move 15 from 6 to 4
move 2 from 5 to 2
move 11 from 4 to 6
move 5 from 4 to 1
move 5 from 4 to 2
move 2 from 1 to 5
move 6 from 2 to 8
move 11 from 6 to 3
move 12 from 6 to 8
move 1 from 3 to 9
move 3 from 3 to 2
move 6 from 4 to 2
move 2 from 5 to 8
move 5 from 7 to 2
move 11 from 8 to 4
move 1 from 7 to 4
move 1 from 9 to 6
move 7 from 2 to 1
move 3 from 6 to 5
move 2 from 5 to 3
move 1 from 5 to 9
move 3 from 4 to 9
move 4 from 9 to 1
move 4 from 3 to 6
move 3 from 4 to 8
move 3 from 8 to 9
move 2 from 8 to 2
move 9 from 8 to 7
move 2 from 3 to 1
move 2 from 3 to 2
move 1 from 3 to 6
move 2 from 9 to 1
move 8 from 7 to 5
move 7 from 2 to 7
move 2 from 8 to 9
move 4 from 6 to 5
move 13 from 1 to 5
move 4 from 1 to 8
move 3 from 9 to 3
move 12 from 5 to 9
move 3 from 8 to 9
move 1 from 8 to 4
move 3 from 2 to 7
move 3 from 3 to 7
move 1 from 9 to 2
move 4 from 6 to 4
move 6 from 5 to 6
move 2 from 7 to 3
move 2 from 2 to 1
move 5 from 6 to 5
move 1 from 1 to 7
move 9 from 5 to 4
move 10 from 9 to 6
move 1 from 2 to 6
move 12 from 7 to 6
move 1 from 7 to 4
move 23 from 6 to 1
move 10 from 4 to 3
move 16 from 1 to 5
move 5 from 1 to 2
move 6 from 3 to 7
move 5 from 4 to 8");

        }
    }
}
