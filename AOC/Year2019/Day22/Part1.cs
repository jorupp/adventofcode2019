using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC.Year2019.Day22
{
    public class Part1 : BasePart
    {
        protected void RunScenario(string title, string input, int numCards, int? targetCard = null)
        {
            RunScenario(title, () =>
            {
                var lines = input.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

                var cards = Enumerable.Range(0, numCards).ToList();

                List<int> DealNewStack(List<int> cards)
                {
                    cards = cards.ToList();
                    cards.Reverse();
                    return cards;
                }

                List<int> Cut(List<int> cards, int count)
                {
                    if (count < 0)
                    {
                        count = cards.Count + count;
                    }
                    return cards.Skip(count).Concat(cards.Take(count)).ToList();
                }

                List<int> DealWithIncrement(List<int> cards, int increment)
                {
                    var outCards = Enumerable.Repeat(0, cards.Count).ToList();
                    for (var i = 0; i < cards.Count; i++)
                    {
                        var ix = (i * increment) % cards.Count;
                        //Console.WriteLine(ix);
                        outCards[ix] = cards[i];
                    }

                    return outCards;
                }

                //Console.WriteLine(string.Join(" ", cards));

                foreach (var line in lines)
                {
                    //Console.WriteLine(line);
                    if (line.StartsWith("deal with increment"))
                    {
                        var x = int.Parse(line.Split(' ').Last());
                        cards = DealWithIncrement(cards, x);
                    }
                    else if (line.StartsWith("cut"))
                    {
                        var x = int.Parse(line.Split(' ').Last());
                        cards = Cut(cards, x);
                    } else if (line.StartsWith("deal into new stack"))
                    {
                        cards = DealNewStack(cards);
                    }
                    else
                    {
                        throw new NotImplementedException(line);
                    }
                    //Console.WriteLine(string.Join(" ", cards));

                }

                if (targetCard != null)
                {
                    Console.WriteLine(cards.IndexOf(targetCard.Value));
                }
                else
                {
                    Console.WriteLine(string.Join(" ", cards));
                }

                // 9172 is not correct
            });
        }

        public override void Run()
        {
            RunScenario("initial", @"deal with increment 7
deal into new stack
deal into new stack", 10);
            RunScenario("initial", @"cut 6
deal with increment 7

deal into new stack", 10);
            RunScenario("initial", @"deal with increment 7
deal with increment 9
cut -2", 10);
            RunScenario("initial", @"deal into new stack
cut -2
deal with increment 7
cut 8
cut -4
deal with increment 7
cut 3
deal with increment 9
deal with increment 3
cut -1", 10);
            //return;
            //return;
            RunScenario("part1", @"cut -1353
deal with increment 63
cut -716
deal with increment 55
cut 1364
deal with increment 61
cut 1723
deal into new stack
deal with increment 51
cut 11
deal with increment 65
cut -6297
deal with increment 69
cut -3560
deal with increment 20
cut 1177
deal with increment 29
cut 6033
deal with increment 3
cut -3564
deal into new stack
cut 6447
deal into new stack
cut -4030
deal with increment 3
cut -6511
deal with increment 42
cut -8748
deal with increment 38
cut 5816
deal with increment 73
cut 9892
deal with increment 16
cut -9815
deal with increment 10
cut 673
deal with increment 12
cut 4518
deal with increment 52
cut 9464
deal with increment 68
cut 902
deal with increment 11
deal into new stack
deal with increment 45
cut -5167
deal with increment 68
deal into new stack
deal with increment 24
cut -8945
deal into new stack
deal with increment 36
cut 3195
deal with increment 52
cut -1494
deal with increment 11
cut -9658
deal into new stack
cut -4689
deal with increment 34
cut -9697
deal with increment 39
cut -6857
deal with increment 19
cut -6790
deal with increment 59
deal into new stack
deal with increment 52
cut -9354
deal with increment 71
cut 8815
deal with increment 2
cut 6618
deal with increment 47
cut -6746
deal into new stack
cut 1336
deal with increment 53
cut 6655
deal with increment 17
cut 8941
deal with increment 25
cut -3046
deal with increment 14
cut -7818
deal with increment 25
cut 4140
deal with increment 60
cut 6459
deal with increment 27
cut -6791
deal into new stack
cut 3821
deal with increment 13
cut 3157
deal with increment 13
cut 8524
deal into new stack
deal with increment 12
cut 5944", 10007, 2019);

        }
    }
}
