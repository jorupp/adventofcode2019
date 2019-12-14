using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using AoC.GraphSolver;

namespace AoC.Year2019.Day14
{
    public class Part2 : BasePart
    {
        protected void RunScenario(string title, string input)
        {
            RunScenario(title, () =>
            {
                var lines = input.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

                var reactions = lines.Select(i =>
                {
                    var parts = i
                        .Split(new[] { '=', '>' }, StringSplitOptions.RemoveEmptyEntries)
                        .Select(ii => ii.Trim())
                        .Select(ii => ii.Split(',', StringSplitOptions.RemoveEmptyEntries)
                            .Select(iii => iii.Split(' ', StringSplitOptions.RemoveEmptyEntries))
                            //.Select(iii => { Console.WriteLine(iii[0]); Console.WriteLine(iii[1]); return iii; })
                            .ToDictionary(iii => iii[1], iii => long.Parse(iii[0]))
                        )
                        .ToList();
                    return (parts[1].Single().Key, parts[1].Single().Value, parts[0]);
                }).ToList();

                //Console.WriteLine(Test(reactions, 460664));
                //Console.WriteLine(Test(reactions, 460665));
                //Console.WriteLine(Test(reactions, 5586022));
                //Console.WriteLine(Test(reactions, 5586023));
                //Console.WriteLine(Test(reactions, 82892753));
                //Console.WriteLine(Test(reactions, 82892754));


                //Console.WriteLine(Test(reactions, 82892753));
                //Console.WriteLine(Test(reactions, 82892754));

                long bestWorking = 0;
                long worstNotWorking = long.MaxValue;
                long target = 1000000;
                while (true)
                {
                    //Console.WriteLine($"Trying {target}");
                    if (Test(reactions, target))
                    {
                        bestWorking = target;
                        if (worstNotWorking == long.MaxValue)
                        {
                            target *= 2;
                        }
                        else
                        {
                            target += Math.Max(1, (worstNotWorking - bestWorking)/2);
                        }
                    }
                    else
                    {
                        worstNotWorking = target;
                        target -= Math.Max(1, (worstNotWorking - bestWorking) / 2);
                        if (target == bestWorking)
                            break;
                    }
                }

                Console.WriteLine(bestWorking);

            });
        }

        private bool Test(List<(string, long, Dictionary<string, long>)> reactions, long quantity)
        {
            var stores = new Dictionary<string, long>() {{"ORE", 1000000000000 } };
            RunReaction(reactions, stores, "FUEL", quantity);
            while (true)
            {
                var toRun = stores.Where(i => i.Value < 0).Select(i => i.Key).FirstOrDefault();
                if (toRun == null)
                {
                    //Console.WriteLine(string.Join(",", stores.OrderBy(i => i.Value).Select(i => $"{i.Key}={i.Value}")));
                    return true;
                }
                if (!RunReaction(reactions, stores, toRun, -stores[toRun]))
                {
                    return false;
                }
            }
        }

        private bool RunReaction(List<(string, long, Dictionary<string, long>)> reactions, Dictionary<string, long> stores, string key, long quantity)
        {
            if (key == "ORE")
            {
                return false;
            }

            var reaction = reactions.Single(i => i.Item1 == key);
            var q = (long)Math.Ceiling((decimal)quantity / reaction.Item2);

            stores[reaction.Item1] = (stores.TryGetValue(reaction.Item1, out var vv) ? vv : 0) + reaction.Item2 * q;

            foreach (var input in reaction.Item3)
            {
                var cInput = stores.TryGetValue(input.Key, out var v) ? v : 0;
                var newIn = cInput - input.Value * q;
                if (newIn > cInput)
                {
                    // overflow protection
                    return false;
                }
                stores[input.Key] = cInput - input.Value * q;
            }

            return true;
        }

        //private class ReactionsNode
        //{
        //    private List<(long, string, long, Dictionary<string, long>)> _reactions;
        //}

        //private class Estimator
        //{
        //    private List<(Dictionary<string, long>, Dictionary<string, long>)> _reactions;
        //    private Dictionary<(string, long), long> _estimates = new Dictionary<(string, long), long>();

        //    public Estimator(List<(Dictionary<string, long>, Dictionary<string, long>)> reactions)
        //    {
        //        _reactions = reactions;
        //    }

        //    public long GetEstimate(string key, long quantity)
        //    {
        //        if (key == "ORE")
        //        {
        //            return quantity;
        //        }
        //        var eKey = (key, quantity);
        //        if (_estimates.TryGetValue(eKey, out var v))
        //        {
        //            return v;
        //        }
        //        var reaction = _reactions.Single(i => i.Item2.Single().Key == key);
        //        var q = (long)Math.Ceiling((decimal)quantity / reaction.Item2.Single().Value);
        //        var e = reaction.Item1.Sum(i => GetEstimate(i.Key, i.Value * q));
        //        _estimates[eKey] = e;
        //        return e;
        //    }
        //}

        //private class ChemNode : Node<ChemNode, string, long>
        //{
        //    private Dictionary<string, long> _stuff;
        //    private List<(string, long, Dictionary<string, long>)> _reactions;
        //    //private Estimator _estimator;
        //    //private Dictionary<string, long> _done;

        //    public long Ore
        //    {
        //        get { return -_stuff["ORE"]; }
        //    }

        //    public ChemNode(Dictionary<string, long> input, List<(string, long, Dictionary<string, long>)> reactions)//, Estimator estimator)//, Dictionary<string, long> done)
        //    {
        //        _stuff = input.ToDictionary(i => i.Key, i => i.Value);
        //        _reactions = reactions;
        //        //_estimator = estimator;
        //        //_done = done.ToDictionary(i => i.Key, i => i.Value);
        //    }

        //    public override IEnumerable<ChemNode> GetAdjacent()
        //    {
        //        //Console.WriteLine($"Generating reactions for {this.Description} - {this.CurrentCost} - {this.EstimatedCost}");
        //        foreach (var reaction in _reactions)
        //        {
        //            var node = new ChemNode(this._stuff, this._reactions);//, this._estimator);//, this._done);

        //            var oldOutputValue = (node._stuff.TryGetValue(reaction.Item1, out var vv) ? vv : 0);
        //            if (reaction.Item1 == "FUEL")
        //            {
        //                continue;
        //            }

        //            var q = (long)Math.Floor((decimal)oldOutputValue / reaction.Item2);
        //            if (q == 0)
        //            {
        //                continue;
        //            }

        //            node._stuff[reaction.Item1] = oldOutputValue - reaction.Item2 * q;

        //            foreach (var input in reaction.Item3)
        //            {
        //                node._stuff[input.Key] = (node._stuff.TryGetValue(input.Key, out var v) ? v : 0) + input.Value * q;
        //            }

        //            if (!node.IsValid)
        //            {
        //                continue;
        //            }

        //            //Console.WriteLine($"Returning {node.Description} - {node.CurrentCost} - {node.EstimatedCost} - {node.IsValid} - {node.IsComplete}");
        //            yield return node;
        //        }
        //    }

        //    public override bool IsValid
        //    {
        //        get
        //        {
        //            return this._stuff.All(i => i.Value >= 0 || i.Key == "ORE");
        //        }
        //    }

        //    public override bool IsComplete
        //    {
        //        get { return false; }
        //    }

        //    public override long CurrentCost
        //    {
        //        get
        //        {
        //            return -(this._stuff.TryGetValue("ORE", out var v) ? v : 0);
        //        }
        //    }

        //    public override long EstimatedCost
        //    {
        //        get
        //        {
        //            return CurrentCost;
        //            //return CurrentCost + (long)(this._stuff.Where(i => i.Value < 0)
        //            //           .Sum(i => this._estimator.GetEstimate(i.Key, -i.Value)));
        //            //return -this._stuff.Values.Where(i => i < 0).Sum();
        //        }
        //    }

        //    protected override string GetKey()
        //    {
        //        return string.Join(",", this._stuff.OrderBy(i => i.Key).Select(i => $"{i.Key}={i.Value}"));
        //    }

        //    public override string Description
        //    {
        //        get
        //        {
        //            return GetKey();
        //            //return $"{GetKey()} - {string.Join(", ", _done)}";
        //        }
        //    }
        //}

        public override void Run()
        {
            //            RunScenario("initial", @"10 ORE => 10 A
            //1 ORE => 1 B
            //7 A, 1 B => 1 C
            //7 A, 1 C => 1 D
            //7 A, 1 D => 1 E
            //7 A, 1 E => 1 FUEL");
            //RunScenario("initial", @"9 ORE => 2 A
            //8 ORE => 3 B
            //7 ORE => 5 C
            //3 A, 4 B => 1 AB
            //5 B, 7 C => 1 BC
            //4 C, 1 A => 1 CA
            //2 AB, 3 BC, 4 CA => 1 FUEL");
            //return;
            RunScenario("initial", @"157 ORE => 5 NZVS
            165 ORE => 6 DCFZ
            44 XJWVT, 5 KHKGT, 1 QDVJ, 29 NZVS, 9 GPVTF, 48 HKGWZ => 1 FUEL
            12 HKGWZ, 1 GPVTF, 8 PSHF => 9 QDVJ
            179 ORE => 7 PSHF
            177 ORE => 5 HKGWZ
            7 DCFZ, 7 PSHF => 2 XJWVT
            165 ORE => 2 GPVTF
            3 DCFZ, 7 NZVS, 5 HKGWZ, 10 PSHF => 8 KHKGT");
            //return;
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
            RunScenario("part1", @"4 SRWZ, 3 ZGSFW, 1 HVJVQ, 1 RWDSX, 12 BDHX, 1 GDPKF, 23 WFPSM, 1 MPKC => 6 VCWNW
3 BXVJK, 3 WTPN => 4 GRQC
5 KWFD => 9 NMZND
1 DNZQ, 5 CDSP => 3 PFDBV
4 VSPSC, 34 MPKC, 9 DFNVL => 9 PZWSP
5 NTXHM => 9 DBKN
4 JNSP, 4 TCKR, 7 PZWSP => 7 DLHG
12 CNBS, 3 FNPC => 2 SRWZ
3 RWDSX, 4 NHSTB, 2 JNSP => 8 TCKR
24 PGHF, 1 NMZND => 3 RWDSX
1 DLHG => 9 QSVN
6 HVJVQ => 2 QSNCW
4 CHDTJ => 9 FDVNC
1 HBXF, 1 RWDSX => 7 BWSPN
2 ZGSFW, 1 KWFD => 8 JNSP
2 BWSPN, 7 GDPKF, 1 BXVJK => 6 FVQM
2 MHBH => 6 FNPC
2 WTPN, 15 GRQC => 3 ZGSFW
9 LXMLX => 6 CLZT
5 DFNVL, 1 KHCQ => 4 MHLBR
21 CNTFK, 3 XHST => 9 CHDTJ
1 CNTFK => 7 MHBH
1 GMQDW, 34 GDPKF, 2 ZDGPL, 1 HVJVQ, 13 QSVN, 1 QSNCW, 1 BXVJK => 2 SGLGN
1 BMVRK, 1 XHST => 8 XHLNT
23 CXKN => 1 BDKN
121 ORE => 9 XHST
4 NTXHM, 4 FNPC, 15 VCMVN => 8 MPKC
2 ZDGPL, 7 JNSP, 3 FJVMD => 4 GMQDW
1 LXMLX, 2 BWSPN => 2 DNZQ
6 WTPN => 9 KCMH
20 CDSP => 2 VSPSC
2 QSNCW, 1 BDHX, 3 HBXF, 8 PFDBV, 17 ZDGPL, 1 MHLBR, 9 ZGSFW => 8 FDWSG
2 VSFTG, 2 DLHG => 9 BDHX
174 ORE => 5 BMVRK
2 BMVRK => 2 KWFD
3 WTPN, 9 TVJPG => 9 CDSP
191 ORE => 2 CNTFK
9 FDVNC, 1 MHBH => 8 NTXHM
3 NHSTB, 2 BXVJK, 1 JNSP => 1 WFPSM
7 FJVMD => 9 CXKN
3 GDPKF, 10 QSNCW => 7 ZDGPL
7 LPXM, 11 VSPSC => 1 LXMLX
6 RWDSX, 2 NMZND, 1 MPKC => 1 KHCQ
6 RWDSX => 4 QMJK
15 MHBH, 28 DBKN, 12 CNBS => 4 PGHF
20 NMZND, 1 PGHF, 1 BXVJK => 2 LPXM
1 CDSP, 17 BXVJK => 5 NHSTB
12 HVJVQ => 3 VSFTG
2 PGHF, 3 VCMVN, 2 NHSTB => 1 DFNVL
5 FNPC => 9 HBXF
3 DPRL => 4 FJVMD
1 KWFD, 1 TVJPG => 8 VCMVN
1 FDWSG, 1 VCWNW, 4 BDKN, 14 FDVNC, 1 CLZT, 62 SGLGN, 5 QMJK, 26 ZDGPL, 60 KCMH, 32 FVQM, 15 SRWZ => 1 FUEL
3 XHLNT => 8 TVJPG
5 HBXF => 2 HVJVQ
3 CHDTJ, 15 KWFD => 9 WTPN
7 CNTFK => 7 CNBS
1 CNBS => 2 JPDF
5 JNSP => 8 DPRL
11 NTXHM => 8 GDPKF
10 JPDF => 9 BXVJK");

        }
    }
}
