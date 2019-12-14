using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using AoC.GraphSolver;

namespace AoC.Year2019.Day14
{
    public class Part1 : BasePart
    {
        protected void RunScenario(string title, string input)
        {
            RunScenario(title, () =>
            {
                var lines = input.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

                var reactions = lines.Select(i =>
                {
                    var parts = i
                        .Split(new[] {'=', '>'}, StringSplitOptions.RemoveEmptyEntries)
                        .Select(ii => ii.Trim())
                        .Select(ii => ii.Split(',', StringSplitOptions.RemoveEmptyEntries)
                            .Select(iii => iii.Split(' ', StringSplitOptions.RemoveEmptyEntries))
                            //.Select(iii => { Console.WriteLine(iii[0]); Console.WriteLine(iii[1]); return iii; })
                            .ToDictionary(iii => iii[1], iii => long.Parse(iii[0]))
                        )
                        .ToList();
                    return (parts[0], parts[1]);
                }).ToList();

                if (reactions.Any(ii => ii.Item2.Count != 1))
                {
                    throw new NotImplementedException();
                }

                var start = new ChemNode(new Dictionary<string, long>(), reactions, new Estimator(reactions), new Dictionary<string, long>());
                var solution = new RealSolver().Evaluate<ChemNode, string, long>(start);


                Console.WriteLine($"{solution.Description} - {solution.Ore}");
            });
        }

        private class Estimator
        {
            private List<(Dictionary<string, long>, Dictionary<string, long>)> _reactions;
            private Dictionary<(string, long), long> _estimates = new Dictionary<(string, long), long>();

            public Estimator(List<(Dictionary<string, long>, Dictionary<string, long>)> reactions)
            {
                _reactions = reactions;
            }

            public long GetEstimate(string key, long quantity)
            {
                if (key == "ORE")
                {
                    return quantity;
                }
                var eKey = (key, quantity);
                if (_estimates.TryGetValue(eKey, out var v))
                {
                    return v;
                }
                var reaction = _reactions.Single(i => i.Item2.Single().Key == key);
                var q = (long) Math.Ceiling((decimal) quantity / reaction.Item2.Single().Value);
                var e = reaction.Item1.Sum(i => GetEstimate(i.Key, i.Value * q));
                _estimates[eKey] = e;
                return e;
            }
        }

        private class ChemNode : Node<ChemNode, string, long>
        {
            private Dictionary<string, long> _stuff;
            private List<(Dictionary<string, long>, Dictionary<string, long>)> _reactions;
            private Estimator _estimator;
            private Dictionary<string, long> _done;

            public long Ore
            {
                get { return -_stuff["ORE"]; }
            }

            public ChemNode(Dictionary<string, long> input, List<(Dictionary<string, long>, Dictionary<string, long>)> reactions, Estimator estimator , Dictionary<string, long> done)
            {
                _stuff = input.ToDictionary(i => i.Key, i => i.Value);
                _reactions = reactions;
                _estimator = estimator;
                _done = done.ToDictionary(i => i.Key, i => i.Value);
            }

            public override IEnumerable<ChemNode> GetAdjacent()
            {
                Console.WriteLine($"Generating reactions for {this.Description} - {this.CurrentCost} - {this.EstimatedCost}");
                foreach (var reaction in _reactions)
                {
                    var node = new ChemNode(this._stuff, this._reactions, this._estimator, this._done);

                    var output = reaction.Item2.Single();
                    var oldOutputValue = (node._stuff.TryGetValue(output.Key, out var vv) ? vv : 0);
                    long quantity = 1;
                    if (output.Key == "FUEL")
                    {
                        if (oldOutputValue != 0)
                        {
                            continue;
                        }
                    }
                    else
                    {
                        if (oldOutputValue >= 0)
                        {
                            continue;
                        }

                        quantity = (long)Math.Ceiling((decimal) -oldOutputValue / output.Value);
                    }

                    node._done[output.Key] = (node._stuff.TryGetValue(output.Key, out var vvv) ? vvv : 0) + quantity;
                    node._stuff[output.Key] = oldOutputValue + output.Value * quantity;


                    //foreach (var output in reaction.Item2)
                    //{
                    //    node._stuff[output.Key] = (node._stuff.TryGetValue(output.Key, out var v) ? v : 0) + output.Value;
                    //}

                    foreach (var input in reaction.Item1)
                    {
                        node._stuff[input.Key] = (node._stuff.TryGetValue(input.Key, out var v) ? v : 0) - input.Value * quantity;
                    }

                    Console.WriteLine($"Returning {node.Description} - {node.CurrentCost} - {node.EstimatedCost} - {node.IsValid} - {node.IsComplete}");
                    yield return node;
                }
            }

            public override bool IsValid
            {
                get
                {
                    var fuel = this._stuff.TryGetValue("FUEL", out var v) ? v : 0;
                    if (fuel != 1)
                    {
                        return false;
                    }

                    return true;
                }
            }

            public override bool IsComplete
            {
                get
                {
                    var fuel = this._stuff.TryGetValue("FUEL", out var v) ? v : 0;
                    if (fuel != 1)
                    {
                        return false;
                    }
                    return this._stuff.All(i => i.Key == "ORE" || i.Value >= 0);
                }
            }

            public override long CurrentCost
            {
                get
                {
                    return -(this._stuff.TryGetValue("ORE", out var v) ? v : 0);
                }
            }

            public override long EstimatedCost
            {
                get
                {
                    return CurrentCost + (long)(this._stuff.Where(i => i.Value < 0)
                               .Sum(i => this._estimator.GetEstimate(i.Key, -i.Value)) * 1.1);
                    //return -this._stuff.Values.Where(i => i < 0).Sum();
                }
            }

            protected override string GetKey()
            {
                return string.Join(",", this._stuff.OrderBy(i => i.Key).Select(i => $"{i.Key}={i.Value}"));
            }

            public override string Description
            {
                get
                {
                    return $"{GetKey()} - {string.Join(", ", _done)}";
                }
            }
        }

        public override void Run()
        {
//            RunScenario("initial", @"10 ORE => 10 A
//1 ORE => 1 B
//7 A, 1 B => 1 C
//7 A, 1 C => 1 D
//7 A, 1 D => 1 E
//7 A, 1 E => 1 FUEL");
//            RunScenario("initial", @"9 ORE => 2 A
//8 ORE => 3 B
//7 ORE => 5 C
//3 A, 4 B => 1 AB
//5 B, 7 C => 1 BC
//4 C, 1 A => 1 CA
//2 AB, 3 BC, 4 CA => 1 FUEL");
//            //return;
//            RunScenario("initial", @"157 ORE => 5 NZVS
//165 ORE => 6 DCFZ
//44 XJWVT, 5 KHKGT, 1 QDVJ, 29 NZVS, 9 GPVTF, 48 HKGWZ => 1 FUEL
//12 HKGWZ, 1 GPVTF, 8 PSHF => 9 QDVJ
//179 ORE => 7 PSHF
//177 ORE => 5 HKGWZ
//7 DCFZ, 7 PSHF => 2 XJWVT
//165 ORE => 2 GPVTF
//3 DCFZ, 7 NZVS, 5 HKGWZ, 10 PSHF => 8 KHKGT");
            RunScenario("initial", @"2 VPVL, 7 FWMGM, 2 CXFTF, 11 MNCFX => 1 STKFG
17 NVRVD, 3 JNWZP => 8 VPVL
53 STKFG, 6 MNCFX, 46 VJHF, 81 HVMC, 68 CXFTF, 25 GNMV => 1 FUEL
22 VJHF, 37 MNCFX => 5 FWMGM
139 ORE => 4 NVRVD
144 ORE => 7 JNWZP
5 MNCFX, 7 RFSQX, 2 FWMGM, 2 VPVL, 19 CXFTF => 3 HVMC
5 VJHF, 7 MNCFX, 9 VPVL, 37 CXFTF => 6 GNMV
145 ORE => 6 MNCFX
1 NVRVD => 8 CXFTF
1 VJHF, 6 MNCFX => 4 RFSQX
176 ORE => 6 VJHF");

            return;
            RunScenario("initial", @"171 ORE => 8 CNZTR
7 ZLQW, 3 BMBT, 9 XCVML, 26 XMNCP, 1 WPTQ, 2 MZWV, 1 RJRHP => 4 PLWSL
114 ORE => 4 BHXH
14 VRPVC => 6 BMBT
6 BHXH, 18 KTJDG, 12 WPTQ, 7 PLWSL, 31 FHTLT, 37 ZDVW => 1 FUEL
6 WPTQ, 2 BMBT, 8 ZLQW, 18 KTJDG, 1 XMNCP, 6 MZWV, 1 RJRHP => 6 FHTLT
15 XDBXC, 2 LTCX, 1 VRPVC => 6 ZLQW
13 WPTQ, 10 LTCX, 3 RJRHP, 14 XMNCP, 2 MZWV, 1 ZLQW => 1 ZDVW
5 BMBT => 4 WPTQ
189 ORE => 9 KTJDG
1 MZWV, 17 XDBXC, 3 XCVML => 2 XMNCP
12 VRPVC, 27 CNZTR => 2 XDBXC
15 KTJDG, 12 BHXH => 5 XCVML
3 BHXH, 2 VRPVC => 7 MZWV
121 ORE => 7 VRPVC
7 XCVML => 6 RJRHP
5 BHXH, 4 VRPVC => 5 LTCX");
            //return;
            //RunScenario("part1", @"ff2f323f");

        }
    }
}
