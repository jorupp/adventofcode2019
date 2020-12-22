using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC.Year2020.Day22
{
    public class Part1 : BasePart
    {
        protected void RunScenario(string title, string input)
        {
            RunScenario(title, () =>
            {
                var playerInputs = input.Replace("\r\n", "\n").Split("\n\n", StringSplitOptions.RemoveEmptyEntries);

                //Console.WriteLine(playerInputs[0]);
                //Console.WriteLine("A");
                //Console.WriteLine(playerInputs[1]);

                var players = playerInputs.Select(i =>
                {
                    var x = i.Split("\n", StringSplitOptions.RemoveEmptyEntries).Skip(1).ToList();
                    return new Queue<int>(x.Where(i => !string.IsNullOrWhiteSpace(i)).Select(i => { return int.Parse(i); }));
                }).ToArray();

                while(players.All(i => i.Count > 0))
                {
                    var cards = players.Select(i => i.Dequeue()).ToList();
                    if (cards[0] > cards[1])
                    {
                        players[0].Enqueue(cards[0]);
                        players[0].Enqueue(cards[1]);
                    } else
                    {
                        players[1].Enqueue(cards[1]);
                        players[1].Enqueue(cards[0]);
                    }
                }

                var score = players.SelectMany(i => i).Reverse().Select((i, ix) => i * (ix + 1)).Sum();

                Console.WriteLine(score);
            });
        }

        public override void Run()
        {
            RunScenario("initial", @"Player 1:
9
2
6
3
1

Player 2:
5
8
4
7
10");
            //return;
            RunScenario("part1", @"Player 1:
28
3
35
27
19
40
14
15
17
22
45
47
26
13
32
38
43
24
29
5
31
48
49
41
25

Player 2:
34
12
2
50
16
1
44
11
36
6
10
42
20
8
46
9
37
4
7
18
23
39
30
33
21");

        }
    }
}
