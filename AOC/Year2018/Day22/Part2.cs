using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AoC.GraphSolver;

namespace AoC.Year2018.Day22
{
    public class Part2 : BasePart
    {
        protected void RunScenario(string title, string input)
        {
            RunScenario(title, () =>
            {
                var lines = input.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

                var depth = lines[0].Numbers()[0];
                var tx = lines[1].Numbers()[0];
                var ty = lines[1].Numbers()[1];
                var maxX = tx * 100;
                var maxY = ty * 100;

                var erosionLevels = new long[maxX + 1, maxY + 1];
                var geologicIndexes = new long[maxX + 1, maxY + 1];
                var types = new char[maxX + 1, maxY + 1];

                void DumpMap()
                {
                    var sb = new StringBuilder();
                    for (var y = 0; y <= ty; y++)
                    {
                        for (var x = 0; x <= tx; x++)
                        {
                            sb.Append(types[x, y]);
                        }

                        sb.AppendLine();
                    }
                    Console.WriteLine(sb.ToString());
                }

                for (long y = 0; y <= maxY; y++)
                {
                    for (long x = 0; x <= maxX; x++)
                    {
                        if (x == 0 && y == 0)
                        {
                            geologicIndexes[x, y] = 0;
                        }
                        else if (x == tx && y == ty)
                        {
                            geologicIndexes[x, y] = 0;
                        }
                        else if (y == 0)
                        {
                            geologicIndexes[x, y] = x * 16807;
                        }
                        else if (x == 0)
                        {
                            geologicIndexes[x, y] = y * 48271;
                        }
                        else
                        {
                            geologicIndexes[x, y] = erosionLevels[x - 1, y] * erosionLevels[x, y - 1];
                        }

                        erosionLevels[x, y] = (geologicIndexes[x, y] + depth) % 20183;
                        var levelMod3 = erosionLevels[x, y] % 3;
                        types[x, y] = levelMod3 == 0 ? ROCKY : levelMod3 == 1 ? WET : NARROW;
                    }
                }

                var startNode = new PathNode(types, 0, 0, 0, Equipment.Torch, tx, ty, null);
                var finalNode = new RealSolver().Evaluate(startNode, "", (i1, i2) => false);

                Console.WriteLine(finalNode.CurrentCost);
            });
        }

        public const int SwitchTime = 7;
        public const int MoveTime = 1;

        public class PathNode : Node<PathNode, string>
        {
            private readonly char[,] _types;
            private readonly int _x;
            private readonly int _y;
            private readonly int _time;
            private readonly Equipment _equipment;
            private readonly int _tx;
            private readonly int _ty;
            private readonly PathNode _prev;

            public PathNode(char[,] types, int x, int y, int time, Equipment equipment, int tx, int ty, PathNode prev)
            {
                _types = types;
                _x = x;
                _y = y;
                _time = time;
                _equipment = equipment;
                _tx = tx;
                _ty = ty;
                _prev = prev;
            }

            public PathNode[] Path => _prev == null ? new PathNode[0] : _prev.Path.Concat(new[] {_prev}).ToArray();

            public override IEnumerable<PathNode> GetAdjacent()
            {
                yield return new PathNode(_types, _x - 1, _y, _time + MoveTime, _equipment, _tx, _ty, this);
                yield return new PathNode(_types, _x + 1, _y, _time + MoveTime, _equipment, _tx, _ty, this);
                yield return new PathNode(_types, _x, _y - 1, _time + MoveTime, _equipment, _tx, _ty, this);
                yield return new PathNode(_types, _x, _y + 1, _time + MoveTime, _equipment, _tx, _ty, this);
                if (_equipment != Equipment.Neither)
                {
                    yield return new PathNode(_types, _x, _y, _time + SwitchTime, Equipment.Neither, _tx, _ty, this);
                }
                if (_equipment != Equipment.ClimbingGear)
                {
                    yield return new PathNode(_types, _x, _y, _time + SwitchTime, Equipment.ClimbingGear, _tx, _ty, this);
                }
                if (_equipment != Equipment.Torch)
                {
                    yield return new PathNode(_types, _x, _y, _time + SwitchTime, Equipment.Torch, _tx, _ty, this);
                }
            }

            public override bool IsValid
            {
                get
                {
                    if (_x < 0 || _y < 0)
                    {
                        return false;
                    }
                    var type = _types[_x, _y];
                    if (type == ROCKY)
                    {
                        return _equipment != Equipment.Neither;
                    }
                    if (type == WET)
                    {
                        return _equipment != Equipment.Torch;
                    }
                    if (type == NARROW)
                    {
                        return _equipment != Equipment.ClimbingGear;
                    }

                    throw new Exception();
                }
            }

            public override bool IsComplete => _x == _tx && _y == _ty && _equipment == Equipment.Torch;
            public override decimal CurrentCost => _time;
            public override decimal EstimatedCost => _time + (Math.Abs(_x - _tx) + Math.Abs(_y - _ty)) * MoveTime;
            protected override string GetKey()
            {
                return $"{_x},{_y},{_equipment}";
            }

            public override string Description => $"{_x},{_y} - {_equipment} -> {CurrentCost} -> {EstimatedCost}";

            public override string ToString()
            {
                return Description;
            }
        }

        public const char ROCKY = '.';
        public const char WET = '=';
        public const char NARROW = '|';

        public enum Equipment
        {
            Torch = 0,
            ClimbingGear = 1,
            Neither = 2,
        }

        public override void Run()
        {
            RunScenario("initial", @"depth: 510
target: 10,10");
            //return;
            RunScenario("part1", @"depth: 8787
target: 10,725");

        }
    }
}
