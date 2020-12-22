using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC.Year2020.Day22
{
    public class Part2 : BasePart
    {
        int _game = 0;
        //Dictionary<State, (Queue<int>[], bool)> _cache = new Dictionary<State, (Queue<int>[], bool)>();

        protected void RunScenario(string title, string input)
        {
            //_cache = new Dictionary<State, (Queue<int>[], bool)>();
            _game = 0;
            RunScenario(title, () =>
            {
                var playerInputs = input.Replace("\r\n", "\n").Split("\n\n", StringSplitOptions.RemoveEmptyEntries);

                var players = playerInputs.Select(i =>
                {
                    var x = i.Split("\n", StringSplitOptions.RemoveEmptyEntries).Skip(1).ToList();
                    return new Queue<int>(x.Where(i => !string.IsNullOrWhiteSpace(i)).Select(i => { return int.Parse(i); }));
                }).ToArray();

                var result = Play(players);

                var score = result.Item1.SelectMany(i => i).Reverse().Select((i, ix) => i * (ix + 1)).Sum();

                Console.WriteLine(score);
            });
        }

        private class State
        {
            private readonly List<List<int>> players;

            public State(Queue<int>[] players)
            {
                this.players = players.Select(i => i.ToList()).ToList();
            }

            public override bool Equals(object obj)
            {
                var other = (State)obj;

                return other.players.Zip(this.players, (a, b) =>
                {
                    return a.Count == b.Count && a.Zip(b, (x, y) => x == y).All(i => i);
                }).All(i => i);
            }

            public override int GetHashCode()
            {
                return players[0].Count * 62231 + players[1].Count * 2341 + players[0].Aggregate((a, i) => a ^ i) + players[1].Aggregate((a, i) => a ^ i);
            }
        }

        private void Debug(string x = "")
        {
            return;
            Console.WriteLine(x);
        }

        // true if player 1 wins
        private (Queue<int>[], bool) Play(Queue<int>[] players)
        {
            var game = ++_game;
            Console.WriteLine($"=== Game {game} ===");
            Debug();

            var round = 0;
            var seen = new HashSet<State>();
            while (players.All(i => i.Count > 0))
            {
                round++;

                Debug($"-- Round {round} (Game {game}) --");
                var state = new State(players);
                if (seen.Contains(state))
                {
                    return (players, true);
                }
                seen.Add(state);
                //if (_cache.TryGetValue(state, out var p))
                //{
                //    //Console.WriteLine("Cache hit");
                //    foreach (var s in seen)
                //    {
                //        _cache[s] = p;
                //    }
                //    return p;
                //}

                Debug($"Player 1's deck: " + string.Join(", ", players[0]));
                Debug($"Player 2's deck: " + string.Join(", ", players[1]));

                var cards = players.Select(i => i.Dequeue()).ToList();
                Debug($"Player 1 plays: {cards[0]}");
                Debug($"Player 2 plays: {cards[1]}");

                if (players.Select((i, ix) => i.Count >= cards[ix]).All(i => i))
                {
                    // can recurse

                    var newPlayers = players.Select((i, ix) => new Queue<int>(i.Take(cards[ix]))).ToArray();
                    var subResult = Play(newPlayers);

                    if (subResult.Item2)
                    {
                        players[0].Enqueue(cards[0]);
                        players[0].Enqueue(cards[1]);
                        Debug($"Player 1 wins subgame for round {round} of game {game}!");
                    }
                    else
                    {
                        players[1].Enqueue(cards[1]);
                        players[1].Enqueue(cards[0]);
                        Debug($"Player 2 wins subgame for round {round} of game {game}!");
                    }
                }
                else
                {
                    if (cards[0] > cards[1])
                    {
                        players[0].Enqueue(cards[0]);
                        players[0].Enqueue(cards[1]);
                        Debug($"Player 1 wins round {round} of game {game}!");
                    }
                    else
                    {
                        players[1].Enqueue(cards[1]);
                        players[1].Enqueue(cards[0]);
                        Debug($"Player 2 wins round {round} of game {game}!");
                    }
                }
            }

            var result = (players, players[0].Count > 0);
            //foreach (var s in seen)
            //{
            //    _cache[s] = result;
            //}
            return result;
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
