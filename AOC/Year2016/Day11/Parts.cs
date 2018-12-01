using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using AoC.GraphSolver;

namespace AoC.Year2016.Day11
{
    public class StaticInfo
    {
        public int Floors;
        public int TargetFloor = 4;
        public string[] Types;
        public int[][] SwapSets;
    }

    [DebuggerDisplay("{Description}")]
    public class FacilityNode : Node<FacilityNode, long>
    {
        public StaticInfo StaticInfo;
        public sbyte ElevatorFloor;
        public sbyte[] GeneratorFloors;
        public sbyte[] MicrochipFloors;
        public int Moves;

        public FacilityNode()
        {
        }

        public FacilityNode(FacilityNode parent)
        {
            this.StaticInfo = parent.StaticInfo;
            this.ElevatorFloor = parent.ElevatorFloor;
            this.GeneratorFloors = new sbyte[parent.GeneratorFloors.Length];
            Array.Copy(parent.GeneratorFloors, this.GeneratorFloors, parent.GeneratorFloors.Length);
            this.MicrochipFloors = new sbyte[parent.MicrochipFloors.Length];
            Array.Copy(parent.MicrochipFloors, this.MicrochipFloors, parent.MicrochipFloors.Length);
            this.Moves = parent.Moves + 1;
        }

        public override bool IsValid
        {
            get
            {
                if (!(1 <= ElevatorFloor && ElevatorFloor <= StaticInfo.TargetFloor))
                    // floor out of range
                    return false;
                for (var ix = 0; ix < MicrochipFloors.Length; ix++)
                {
                    if (ix == -1)
                        // doesn't exist
                        continue;
                    if (GeneratorFloors[ix] == MicrochipFloors[ix])
                        // protected
                        continue;
                    if (GeneratorFloors.Any(f => f == MicrochipFloors[ix]))
                        // zap
                        return false;
                }

                // let's apply swapset-based pruning....
                foreach (var set in StaticInfo.SwapSets)
                {
                    for (var i = 0; i < set.Length - 1; i++)
                    {
                        // if any member of the swapset (j) has both chip and generator above this (set[i]), it is invalid
                        if (set.Skip(i + 1).Any(j =>
                            MicrochipFloors[j] > MicrochipFloors[set[i]] &&
                            GeneratorFloors[j] > GeneratorFloors[set[i]]))
                        {
                            return false;
                        }
                    }
                }

                return true;
            }
        }

        public override bool IsComplete => MicrochipFloors.All(i => i == -1 || i == StaticInfo.TargetFloor) &&
                                           GeneratorFloors.All(i => i == -1 || i == StaticInfo.TargetFloor);

        public override decimal CurrentCost => Moves;

        public override decimal EstimatedCost =>
            // we can move one pair of things to the top right away.  all ther rest come one-at-a-time and require 2x moves
            Moves + Math.Max(0, 
                MicrochipFloors.Sum(i => StaticInfo.TargetFloor - i)*2 +
                GeneratorFloors.Sum(i => StaticInfo.TargetFloor - i)*2 - StaticInfo.TargetFloor * 2);

        protected override long GetKey()
        {
            long val = ElevatorFloor - 1;
            foreach (var g in GeneratorFloors)
            {
                val = (val << 2) + (g - 1);
            }
            foreach (var g in MicrochipFloors)
            {
                val = (val << 2) + (g - 1);
            }
            return val;
        }
        //new object[] { ElevatorFloor + "_" + string.Join("_", GeneratorFloors) + "_" + string.Join("_", MicrochipFloors) };
        //new object[] {ElevatorFloor}.Concat(GeneratorFloors.Cast<object>()).Concat(MicrochipFloors.Cast<object>()).ToArray();

        public override string Description
        {
            get
            {
                return String.Join(Environment.NewLine, Enumerable.Range(1, StaticInfo.Floors).Reverse().Select(f => 
                    $"F{f} {(ElevatorFloor == f ? "E" : ".")} " 
                    + string.Join(" ", GeneratorFloors.Select((g, ix) => g == f ? StaticInfo.Types[ix][0].ToString().ToUpperInvariant() + "G" : ". ")) 
                    + string.Join(" ", MicrochipFloors.Select((g, ix) => g == f ? StaticInfo.Types[ix][0].ToString().ToUpperInvariant() + "M" : ". "))
                ));
            }
        }

        public override IEnumerable<FacilityNode> GetAdjacent()
        {
            foreach (var dir in new sbyte[] { -1, 1 })
            {
                if (!(1 <= this.ElevatorFloor + dir && this.ElevatorFloor + dir <= StaticInfo.TargetFloor))
                    continue;
                // ok, so what are we taking...
                var options = this.MicrochipFloors.Select((f, ix) => new { type = 1, ix, f}).Where(i => i.f == this.ElevatorFloor)
                    .Concat(this.GeneratorFloors.Select((f, ix) => new { type = 2, ix, f }).Where(i => i.f == this.ElevatorFloor))
                    .ToArray();

                // can take one or two up, but going down, only ever take one
                var sets = options.Select(i => new[] {i});
                if (dir == 1)
                {
                    sets = sets.Concat(options.SelectMany((i, ix) => options.Skip(ix + 1).Select(ii => new[] {i, ii})));
                }
                foreach (var set in sets)
                {
                    var next = new FacilityNode(this);
                    next.ElevatorFloor += dir;
                    foreach (var opt in set)
                    {
                        if (opt.type == 1)
                        {
                            next.MicrochipFloors[opt.ix] += dir;
                        }
                        else
                        {
                            next.GeneratorFloors[opt.ix] += dir;
                        }
                    }
                    yield return next;
                }
            }
        }
    }

    public class Part1 : BasePart
    {
        protected void RunScenario(string title, string input, Action<FacilityNode> modifyInitialState = null)
        {
            RunScenario(title, () =>
            {
                var lines = input.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                // https://regex101.com/
                var pattern = new Regex(@"a ([^ -]*)(|-compatible) (generator|microchip)");
                var initialState = new FacilityNode() { ElevatorFloor = 1, GeneratorFloors = new sbyte[0], MicrochipFloors = new sbyte[0] };
                var types = new List<string>();
                for(var i = 0; i < lines.Length; i++)
                {
                    var line = lines[i];
                    foreach (Match match in pattern.Matches(line))
                    {
                        var type = match.Groups[1].Value;
                        var item = match.Groups[3].Value;
                        if (!types.Contains(type))
                        {
                            types.Add(type);
                        }
                        var typeIx = types.IndexOf(type);
                        while (initialState.GeneratorFloors.Length <= typeIx)
                        {
                            initialState.GeneratorFloors = initialState.GeneratorFloors.Concat(new sbyte[] { -1 }).ToArray();
                            initialState.MicrochipFloors = initialState.MicrochipFloors.Concat(new sbyte[] { -1 }).ToArray();
                        }

                        if (item == "generator")
                        {
                            initialState.GeneratorFloors[typeIx] = (sbyte)(i + 1);
                        }
                        else if (item == "microchip")
                        {
                            initialState.MicrochipFloors[typeIx] = (sbyte)(i + 1);
                        }
                        else
                        {
                            throw new ArgumentException($"item: {item}");
                        }
                    }
                }
                initialState.StaticInfo = new StaticInfo() {  Floors = lines.Length, TargetFloor = lines.Length, Types = types.ToArray() };
                initialState.StaticInfo.SwapSets =
                    initialState.GeneratorFloors
                        .Select((i, ix) => new {ix, g = i, m = initialState.MicrochipFloors[ix]})
                        .Where(i => i.g == i.m)
                        .GroupBy(i => i.g, i => i.ix)
                        .Select(g => g.ToArray()).ToArray();
                modifyInitialState?.Invoke(initialState);

                var finalState = new ParallelSolver(8).Evaluate(initialState, initialState.Key);
                Console.WriteLine($"{title} - moves: {finalState.CurrentCost}");
            });
        }

        public override void Run()
        {
            RunScenario("initial", @"The first floor contains a hydrogen-compatible microchip and a lithium-compatible microchip.
The second floor contains a hydrogen generator.
The third floor contains a lithium generator.
The fourth floor contains nothing relevant.");
            RunScenario("real part 1", @"The first floor contains a polonium generator, a thulium generator, a thulium-compatible microchip, a promethium generator, a ruthenium generator, a ruthenium-compatible microchip, a cobalt generator, and a cobalt-compatible microchip.
The second floor contains a polonium-compatible microchip and a promethium-compatible microchip.
The third floor contains nothing relevant.
The fourth floor contains nothing relevant.");
            RunScenario("real part 2", @"The first floor contains a polonium generator, a thulium generator, a thulium-compatible microchip, a promethium generator, a ruthenium generator, a ruthenium-compatible microchip, a cobalt generator, and a cobalt-compatible microchip, a elerium generator, a elerium-compatible microchip, a dilithium generator, a dilithium-compatible microchip.
The second floor contains a polonium-compatible microchip and a promethium-compatible microchip.
The third floor contains nothing relevant.
The fourth floor contains nothing relevant.");
        }
    }
}
