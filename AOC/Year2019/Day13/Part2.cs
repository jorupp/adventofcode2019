﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC.Year2019.Day13
{
    public class Part2 : BasePart
    {
        protected void RunScenario(string title, string input)
        {
            RunScenario(title, () =>
            {
                var lines = input.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                var initialData = lines[0].Split(',').Select(i =>
                {
                    //Console.WriteLine(i);
                    return long.Parse(i);
                }).ToArray();
                var data = initialData.Select((i, ii) => new { i, ii }).ToDictionary(i => i.ii, i => i.i);

                data[0] = 2;

                var panels = new Dictionary<(long, long), long>();
                var started = false;

                var outputs = new List<long>();
                long score = 0;

                void Dump()
                {
                    var nmx = panels.Keys.Min(i => i.Item1);
                    var nmy = panels.Keys.Min(i => i.Item2);
                    var mx = panels.Keys.Max(i => i.Item1);
                    var my = panels.Keys.Max(i => i.Item2);

                    Console.WriteLine($"Score: {score}");

                    for (var y = nmx; y <= my; y++)
                    {
                        for (var x = nmy; x <= mx; x++)
                        {
                            var value = (panels.TryGetValue((x, y), out var v) ? v : 0);
                            Console.Write(value == 0 ? " " : value == 1 ? "W" : value == 2 ? "B" : value == 3 ? "=" : value == 4 ? "o" : "X");
                        }
                        Console.WriteLine();
                    }
                }

                Simulate(data, () =>
                {
                    ////Dump();

                    //if (!started)
                    //{
                    //    started = true;
                    //    Dump();
                    //}

                    var bX = panels.Single(i => i.Value == 4).Key.Item1;
                    var pX = panels.Single(i => i.Value == 3).Key.Item1;

                    int GetMove()
                    {
                        if (bX < pX)
                        {
                            return -1;
                        }
                        if (bX > pX)
                        {
                            return 1;
                        }

                        return 0;
                    }


                    Dump();
                    var m = GetMove();
                    Console.WriteLine($"Move: {m}");
                    Console.ReadLine();
                    return m;


                    //while (true)
                    //{
                    //    var c = Console.ReadKey();
                    //    var s = c.KeyChar.ToString().ToLower();
                    //    if (s == "a")
                    //    {
                    //        return -1;
                    //    }
                    //    if (s == "w")
                    //    {
                    //        return 0;
                    //    }
                    //    if (s == "d")
                    //    {
                    //        return 1;
                    //    }
                    //}
                }, (o) =>
                {
                    outputs.Add(o);
                    if (outputs.Count == 3)
                    {
                        if (outputs[0] == -1 && outputs[1] == 0)
                        {
                            score = outputs[2];
                        }
                        else
                        {
                            panels[(outputs[0], outputs[1])] = outputs[2];
                        }
                        outputs = new List<long>();
                    }
                });

                Dump();
                Console.WriteLine($"Score: {score}");
            });
        }

        private void Simulate(IDictionary<int, long> data, Func<int> getInput, Action<long> sendOutput)
        {
            long ip = 0;
            long relativeBase = 0;

            long GetValue(long dataIx)
            {
                return data.TryGetValue((int)dataIx, out var v) ? v : 0;
            }

            while (true)
            {
                var i = GetValue(ip);

                long GetParamIx(long pNum)
                {
                    var pMode = i / (10 * (long)Math.Pow(10, pNum)) % 10;
                    var pix = pMode == 0 ? GetValue(ip + pNum) : pMode == 1 ? ip + pNum : relativeBase + GetValue(ip + pNum);
                    //Console.WriteLine($"PMode: {pMode} for {pNum} -> {pix}");
                    return pix;
                }

                long GetParam(long pNum)
                {
                    return GetValue(GetParamIx(pNum));
                }
                void SetByParam(long pNum, long value)
                {
                    data[(int)GetParamIx(pNum)] = value;
                }

                //Console.WriteLine($"Running opcode {i} @ {ip}");
                //Console.WriteLine(string.Join(",", data));
                //Console.WriteLine($"Relative base: {relativeBase}");
                switch (i % 100)
                {
                    case 1:
                        SetByParam(3, GetParam(1) + GetParam(2));
                        ip += 4;
                        break;
                    case 2:
                        SetByParam(3, GetParam(1) * GetParam(2));
                        ip += 4;
                        break;
                    case 3:
                        SetByParam(1, getInput());
                        ip += 2;
                        break;
                    case 4:
                        var oval = GetParam(1);
                        //Console.WriteLine($"Writing output {oval} in {ix}");
                        sendOutput(oval);
                        ip += 2;
                        break;
                    case 5:
                        if (GetParam(1) != 0)
                        {
                            ip = GetParam(2);
                        }
                        else
                        {
                            ip += 3;
                        }
                        break;
                    case 6:
                        if (GetParam(1) == 0)
                        {
                            ip = GetParam(2);
                        }
                        else
                        {
                            ip += 3;
                        }
                        break;
                    case 7:
                        SetByParam(3, GetParam(1) < GetParam(2) ? 1 : 0);
                        ip += 4;
                        break;
                    case 8:
                        SetByParam(3, GetParam(1) == GetParam(2) ? 1 : 0);
                        ip += 4;
                        break;
                    case 9:
                        relativeBase = relativeBase + GetParam(1);
                        ip += 2;
                        break;
                    case 99:
                        //Console.WriteLine($"Terminating {ix}");
                        ip += 1;
                        return;
                    default:
                        throw new NotImplementedException();
                }
            }
        }
        public override void Run()
        {
            RunScenario("paul", @"1,380,379,385,1008,2639,310356,381,1005,381,12,99,109,2640,1101,0,0,383,1101,0,0,382,20102,1,382,1,21002,383,1,2,21101,0,37,0,1105,1,578,4,382,4,383,204,1,1001,382,1,382,1007,382,40,381,1005,381,22,1001,383,1,383,1007,383,25,381,1005,381,18,1006,385,69,99,104,-1,104,0,4,386,3,384,1007,384,0,381,1005,381,94,107,0,384,381,1005,381,108,1106,0,161,107,1,392,381,1006,381,161,1101,-1,0,384,1106,0,119,1007,392,38,381,1006,381,161,1102,1,1,384,21002,392,1,1,21101,23,0,2,21101,0,0,3,21102,138,1,0,1105,1,549,1,392,384,392,20102,1,392,1,21102,1,23,2,21102,1,3,3,21102,161,1,0,1106,0,549,1101,0,0,384,20001,388,390,1,21002,389,1,2,21101,180,0,0,1105,1,578,1206,1,213,1208,1,2,381,1006,381,205,20001,388,390,1,21001,389,0,2,21102,1,205,0,1106,0,393,1002,390,-1,390,1102,1,1,384,20102,1,388,1,20001,389,391,2,21102,228,1,0,1106,0,578,1206,1,261,1208,1,2,381,1006,381,253,21001,388,0,1,20001,389,391,2,21102,253,1,0,1105,1,393,1002,391,-1,391,1101,0,1,384,1005,384,161,20001,388,390,1,20001,389,391,2,21101,0,279,0,1105,1,578,1206,1,316,1208,1,2,381,1006,381,304,20001,388,390,1,20001,389,391,2,21101,0,304,0,1105,1,393,1002,390,-1,390,1002,391,-1,391,1101,0,1,384,1005,384,161,20102,1,388,1,20101,0,389,2,21101,0,0,3,21101,0,338,0,1106,0,549,1,388,390,388,1,389,391,389,20101,0,388,1,21001,389,0,2,21102,4,1,3,21101,0,365,0,1105,1,549,1007,389,24,381,1005,381,75,104,-1,104,0,104,0,99,0,1,0,0,0,0,0,0,348,18,20,1,1,20,109,3,21201,-2,0,1,21202,-1,1,2,21102,1,0,3,21101,414,0,0,1105,1,549,21202,-2,1,1,21201,-1,0,2,21101,429,0,0,1105,1,601,1201,1,0,435,1,386,0,386,104,-1,104,0,4,386,1001,387,-1,387,1005,387,451,99,109,-3,2106,0,0,109,8,22202,-7,-6,-3,22201,-3,-5,-3,21202,-4,64,-2,2207,-3,-2,381,1005,381,492,21202,-2,-1,-1,22201,-3,-1,-3,2207,-3,-2,381,1006,381,481,21202,-4,8,-2,2207,-3,-2,381,1005,381,518,21202,-2,-1,-1,22201,-3,-1,-3,2207,-3,-2,381,1006,381,507,2207,-3,-4,381,1005,381,540,21202,-4,-1,-1,22201,-3,-1,-3,2207,-3,-4,381,1006,381,529,22101,0,-3,-7,109,-8,2106,0,0,109,4,1202,-2,40,566,201,-3,566,566,101,639,566,566,1201,-1,0,0,204,-3,204,-2,204,-1,109,-4,2105,1,0,109,3,1202,-1,40,593,201,-2,593,593,101,639,593,593,21001,0,0,-2,109,-3,2105,1,0,109,3,22102,25,-2,1,22201,1,-1,1,21101,0,503,2,21101,366,0,3,21102,1,1000,4,21101,630,0,0,1105,1,456,21201,1,1639,-2,109,-3,2106,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,0,0,0,0,2,0,2,2,2,0,0,0,2,0,0,2,0,2,2,2,0,0,2,0,0,2,0,2,2,2,2,0,2,2,2,0,0,0,1,1,0,0,0,2,2,0,2,2,2,2,0,2,0,2,2,2,2,2,0,2,0,0,0,2,2,2,2,2,2,0,0,0,2,0,2,2,0,0,1,1,0,0,2,2,0,2,0,0,2,2,0,2,2,2,0,2,0,0,0,0,2,2,2,2,0,2,2,0,2,0,0,0,0,2,2,2,2,0,1,1,0,2,2,0,0,0,0,2,2,2,0,2,2,2,0,2,0,0,2,2,2,2,0,2,2,2,0,2,0,2,2,0,0,0,2,2,2,0,1,1,0,2,2,2,2,2,2,2,0,2,0,2,0,0,2,0,2,0,2,0,2,0,2,2,0,2,0,0,0,2,2,0,2,2,2,0,0,0,1,1,0,0,0,2,0,0,2,0,2,0,0,2,0,0,0,2,2,0,2,0,0,0,0,0,2,2,0,2,0,2,2,2,0,2,0,0,2,0,1,1,0,2,0,2,2,2,0,0,2,2,0,2,0,2,0,0,0,2,2,2,2,0,0,0,0,0,0,0,0,2,0,2,0,0,0,2,2,0,1,1,0,0,2,2,2,0,0,2,2,2,2,0,0,2,0,0,2,2,2,2,2,2,0,2,0,0,0,2,2,0,2,2,2,2,0,2,0,0,1,1,0,0,2,2,0,0,2,2,0,2,2,0,0,2,2,2,0,0,0,0,2,2,0,2,0,2,0,2,0,0,0,0,0,0,0,2,2,0,1,1,0,0,2,0,2,2,2,2,2,2,0,0,2,2,2,0,0,2,2,2,2,2,0,0,2,0,0,2,0,2,0,2,2,0,0,0,2,0,1,1,0,2,0,2,0,2,0,2,2,2,0,0,0,0,2,0,2,0,2,0,2,2,2,2,2,2,2,2,2,2,2,2,2,0,2,2,2,0,1,1,0,2,2,2,0,2,2,2,2,2,0,0,2,2,2,0,0,0,0,0,2,0,2,0,2,2,0,2,0,0,0,0,2,2,2,2,0,0,1,1,0,2,2,2,0,2,0,2,0,0,0,0,2,0,0,2,0,0,2,2,2,2,2,0,2,0,2,0,2,2,2,0,0,2,0,0,2,0,1,1,0,0,2,2,2,2,0,2,2,2,2,0,2,0,2,2,0,2,0,2,2,0,0,2,2,2,2,2,0,2,2,2,2,2,0,2,2,0,1,1,0,2,0,2,2,2,0,0,2,0,2,2,0,2,2,0,2,0,0,2,2,0,2,2,0,2,2,0,2,2,0,2,2,2,0,2,0,0,1,1,0,2,0,0,2,2,0,0,0,0,2,0,0,2,0,2,2,2,0,2,2,0,2,2,2,0,2,0,2,0,2,0,0,2,2,0,0,0,1,1,0,2,2,2,2,2,2,0,2,2,0,2,2,2,0,2,2,2,2,2,0,2,0,2,0,0,2,2,2,2,2,2,0,0,0,2,0,0,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,4,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,24,35,13,44,44,95,83,45,69,64,58,57,22,91,79,21,65,90,94,24,82,6,96,96,64,21,91,4,36,76,6,74,41,72,32,87,50,48,47,93,86,73,24,78,50,10,95,14,50,78,6,90,98,26,68,75,40,73,80,89,1,41,68,42,47,58,32,23,48,11,83,74,68,41,55,89,46,8,27,5,3,81,42,88,49,51,55,91,22,93,13,12,10,87,42,90,35,88,12,94,79,76,89,39,71,69,32,5,72,45,12,79,57,35,60,46,28,34,79,3,97,32,52,77,66,26,55,8,89,2,76,20,49,64,72,50,15,21,22,63,19,22,44,11,44,36,4,77,24,25,29,8,31,27,68,91,90,89,18,53,67,92,68,59,7,56,2,88,83,82,83,5,73,19,53,81,85,65,93,10,21,46,69,90,32,17,37,31,69,96,93,10,98,32,86,73,91,95,13,15,83,72,10,4,52,64,35,52,42,55,4,76,13,39,54,31,51,78,62,40,14,11,81,34,93,97,47,67,26,46,86,80,69,6,8,56,12,80,88,49,20,79,40,7,54,63,15,46,64,59,74,28,11,48,27,41,20,27,29,70,73,46,18,21,48,26,42,63,7,80,54,8,43,31,3,39,10,30,7,98,87,33,62,81,61,31,64,27,94,38,42,39,55,9,61,38,76,8,48,13,94,8,85,23,72,84,6,60,18,25,30,64,37,97,59,71,16,83,83,18,92,53,39,17,73,39,37,30,9,2,87,32,23,56,11,24,1,84,82,5,8,60,55,44,57,43,14,88,72,51,83,20,3,70,33,33,1,6,86,17,4,77,69,33,65,93,97,66,42,23,34,96,4,25,76,46,2,34,52,5,17,87,69,15,22,3,87,80,36,1,70,43,56,64,11,47,39,5,64,1,41,54,34,95,42,17,8,68,73,45,54,84,16,83,59,27,56,75,34,44,78,70,19,25,90,52,65,58,1,72,2,70,3,26,11,69,73,74,29,8,22,2,93,18,98,16,10,62,92,44,70,69,86,53,2,43,62,45,18,22,46,87,48,21,56,36,71,91,94,84,95,28,74,64,16,44,27,35,33,41,66,9,74,3,94,78,3,47,91,66,92,10,2,6,45,57,24,83,4,56,25,24,51,77,39,36,28,20,6,27,14,25,54,15,84,5,29,16,98,21,32,94,93,5,75,67,65,89,32,16,79,71,31,89,9,5,39,12,14,34,61,9,80,1,65,59,48,48,46,60,98,1,29,98,57,17,18,76,49,93,13,28,37,88,37,46,4,19,48,10,58,37,47,13,85,23,10,48,77,68,92,62,74,63,7,21,31,20,53,87,74,9,32,80,91,70,9,95,90,37,61,60,26,22,56,26,79,65,58,88,51,7,42,43,89,90,11,10,27,19,10,76,96,34,55,36,2,67,11,25,15,96,35,27,50,78,12,8,77,76,26,49,77,60,41,14,24,3,52,52,49,25,35,45,21,98,1,61,2,32,55,86,55,48,28,15,69,97,42,85,90,58,1,75,8,91,60,26,9,70,86,16,50,95,52,90,17,54,1,98,12,25,13,26,94,47,24,23,54,54,65,65,94,61,14,58,35,72,23,98,32,4,84,36,58,38,98,59,1,6,56,1,43,56,33,31,39,64,88,60,30,41,98,17,89,7,15,76,20,43,44,60,65,94,32,71,12,67,87,38,35,56,84,31,12,33,5,42,66,87,47,21,4,52,16,74,18,10,32,97,76,68,76,59,77,92,65,6,15,32,32,14,2,64,67,14,34,3,44,39,56,60,88,56,88,1,76,14,20,67,53,98,74,88,90,67,40,41,56,27,81,58,93,41,78,31,28,12,25,28,94,20,18,41,40,79,10,96,1,64,57,90,30,83,87,71,75,73,63,48,18,10,39,96,60,87,24,54,73,96,6,7,32,26,18,20,4,42,33,63,76,14,21,74,72,3,85,59,16,43,3,22,11,29,96,8,51,32,5,35,94,84,48,58,17,37,58,98,64,63,63,96,31,24,67,29,85,34,29,63,42,68,53,10,47,61,87,33,74,6,76,71,38,52,56,69,32,4,11,44,34,67,13,2,92,55,69,31,15,21,24,7,54,71,93,64,53,67,24,61,25,90,4,95,85,15,44,32,86,11,10,3,32,26,43,18,98,89,82,19,34,30,74,24,96,14,79,46,87,22,53,66,60,91,40,75,92,66,13,33,13,29,55,69,77,34,87,49,83,57,76,42,11,53,27,42,82,28,46,91,310356");
            //RunScenario("part1", @"1,380,379,385,1008,2151,871073,381,1005,381,12,99,109,2152,1102,1,0,383,1101,0,0,382,21001,382,0,1,20102,1,383,2,21102,37,1,0,1106,0,578,4,382,4,383,204,1,1001,382,1,382,1007,382,36,381,1005,381,22,1001,383,1,383,1007,383,21,381,1005,381,18,1006,385,69,99,104,-1,104,0,4,386,3,384,1007,384,0,381,1005,381,94,107,0,384,381,1005,381,108,1106,0,161,107,1,392,381,1006,381,161,1102,1,-1,384,1106,0,119,1007,392,34,381,1006,381,161,1101,0,1,384,21002,392,1,1,21102,1,19,2,21102,0,1,3,21102,1,138,0,1106,0,549,1,392,384,392,20101,0,392,1,21102,19,1,2,21102,3,1,3,21101,161,0,0,1106,0,549,1101,0,0,384,20001,388,390,1,21001,389,0,2,21102,1,180,0,1105,1,578,1206,1,213,1208,1,2,381,1006,381,205,20001,388,390,1,20101,0,389,2,21102,205,1,0,1105,1,393,1002,390,-1,390,1101,1,0,384,20102,1,388,1,20001,389,391,2,21101,228,0,0,1105,1,578,1206,1,261,1208,1,2,381,1006,381,253,21002,388,1,1,20001,389,391,2,21101,0,253,0,1105,1,393,1002,391,-1,391,1101,1,0,384,1005,384,161,20001,388,390,1,20001,389,391,2,21102,1,279,0,1105,1,578,1206,1,316,1208,1,2,381,1006,381,304,20001,388,390,1,20001,389,391,2,21102,304,1,0,1105,1,393,1002,390,-1,390,1002,391,-1,391,1102,1,1,384,1005,384,161,21002,388,1,1,20101,0,389,2,21102,0,1,3,21101,338,0,0,1106,0,549,1,388,390,388,1,389,391,389,21001,388,0,1,20102,1,389,2,21101,0,4,3,21102,1,365,0,1105,1,549,1007,389,20,381,1005,381,75,104,-1,104,0,104,0,99,0,1,0,0,0,0,0,0,180,16,16,1,1,18,109,3,22101,0,-2,1,21201,-1,0,2,21102,1,0,3,21101,414,0,0,1105,1,549,22102,1,-2,1,21201,-1,0,2,21102,429,1,0,1106,0,601,1202,1,1,435,1,386,0,386,104,-1,104,0,4,386,1001,387,-1,387,1005,387,451,99,109,-3,2105,1,0,109,8,22202,-7,-6,-3,22201,-3,-5,-3,21202,-4,64,-2,2207,-3,-2,381,1005,381,492,21202,-2,-1,-1,22201,-3,-1,-3,2207,-3,-2,381,1006,381,481,21202,-4,8,-2,2207,-3,-2,381,1005,381,518,21202,-2,-1,-1,22201,-3,-1,-3,2207,-3,-2,381,1006,381,507,2207,-3,-4,381,1005,381,540,21202,-4,-1,-1,22201,-3,-1,-3,2207,-3,-4,381,1006,381,529,22101,0,-3,-7,109,-8,2106,0,0,109,4,1202,-2,36,566,201,-3,566,566,101,639,566,566,1201,-1,0,0,204,-3,204,-2,204,-1,109,-4,2106,0,0,109,3,1202,-1,36,593,201,-2,593,593,101,639,593,593,21001,0,0,-2,109,-3,2105,1,0,109,3,22102,21,-2,1,22201,1,-1,1,21101,383,0,2,21101,0,250,3,21101,756,0,4,21101,630,0,0,1106,0,456,21201,1,1395,-2,109,-3,2105,1,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,0,0,0,2,2,0,2,0,0,0,0,0,0,2,2,0,2,2,0,2,2,0,0,2,2,2,0,0,0,0,2,2,2,0,1,1,0,0,0,0,2,0,2,2,0,0,2,2,0,0,2,2,0,0,2,2,2,2,0,2,0,2,2,2,2,0,2,0,0,0,1,1,0,0,2,2,0,0,0,0,0,0,2,2,2,0,0,0,0,0,2,0,2,0,0,0,0,0,2,0,2,2,0,2,0,0,1,1,0,0,0,2,0,0,0,2,0,0,0,0,0,0,0,2,0,0,2,0,2,0,0,0,0,0,0,0,2,2,2,0,2,0,1,1,0,0,2,2,2,2,0,0,0,0,0,0,0,0,0,2,0,0,0,2,2,0,0,0,2,0,0,0,2,0,2,2,2,0,1,1,0,0,0,0,2,2,2,2,2,0,2,0,0,0,0,2,0,2,0,2,0,2,0,0,2,2,0,2,0,2,0,2,0,0,1,1,0,2,2,0,2,2,2,2,0,2,2,2,2,0,0,2,0,0,2,0,2,0,0,0,2,0,0,2,2,2,0,0,0,0,1,1,0,0,2,0,0,2,2,0,0,0,0,2,2,0,0,2,0,0,0,2,2,0,2,0,2,2,0,2,2,0,0,0,2,0,1,1,0,0,0,0,0,0,0,2,0,0,0,2,0,2,0,0,0,2,2,2,2,0,2,2,0,2,2,0,0,0,2,0,0,0,1,1,0,0,0,0,0,0,0,0,2,2,2,0,0,0,0,2,0,2,0,0,0,0,2,2,0,2,2,2,0,0,2,0,2,0,1,1,0,0,0,0,2,2,0,2,2,0,2,0,2,2,2,0,0,2,2,0,0,0,0,0,0,2,0,2,2,2,0,2,2,0,1,1,0,2,0,0,2,0,0,0,0,0,2,0,0,2,2,0,2,2,2,0,2,2,0,0,2,2,0,0,2,2,0,0,2,0,1,1,0,2,0,2,0,2,0,2,0,0,2,2,0,0,2,0,0,0,2,0,2,2,0,2,0,0,0,2,0,2,2,0,2,0,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,4,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,52,20,53,45,54,10,5,35,28,96,68,78,29,94,94,57,42,27,61,91,60,22,54,59,33,71,63,62,97,30,76,40,87,10,30,83,68,41,63,55,24,65,56,57,21,91,17,7,60,94,34,54,75,10,16,88,32,34,41,36,57,39,14,89,23,47,7,94,89,60,56,36,44,77,29,17,93,55,58,62,61,18,50,54,22,75,45,1,29,64,32,97,98,50,37,64,39,61,23,39,61,85,85,10,37,56,84,13,43,91,20,73,77,34,87,33,36,42,48,3,39,6,18,58,38,63,48,38,96,32,72,51,22,37,76,4,95,17,3,79,89,19,12,22,71,98,95,22,82,31,70,98,48,46,6,80,95,98,1,81,27,91,14,98,13,46,21,6,75,59,73,9,52,6,44,92,9,11,65,71,19,52,84,71,38,60,43,10,78,25,22,27,90,4,23,96,19,42,54,80,63,64,26,29,58,75,35,95,38,48,1,47,61,20,74,39,85,33,10,70,90,39,93,61,9,65,19,56,84,59,57,30,76,19,52,66,89,93,19,86,4,67,59,37,28,71,1,21,40,18,92,72,57,63,88,42,17,92,42,88,93,17,19,26,63,31,1,8,76,62,31,49,36,18,19,63,50,50,13,77,22,45,11,92,7,92,69,66,49,34,2,58,61,4,18,26,20,7,51,84,81,38,72,22,83,92,16,97,20,81,25,74,13,84,71,2,81,35,83,6,73,93,60,47,2,98,27,55,68,59,67,63,61,48,65,28,71,56,39,30,93,96,3,47,93,77,11,28,86,79,90,83,39,21,68,2,49,50,78,68,81,97,49,9,44,79,31,69,81,76,93,17,31,66,46,26,18,1,17,72,1,28,47,15,85,50,95,75,52,86,5,35,59,51,41,88,33,9,7,77,1,46,6,40,39,36,52,10,12,34,87,64,13,23,96,15,89,13,64,65,29,27,28,50,57,91,68,97,5,38,57,28,45,6,10,90,7,26,79,89,93,74,77,58,51,86,75,49,80,80,28,94,11,56,36,69,88,50,10,22,77,51,83,47,53,2,46,33,45,44,23,4,28,62,21,88,61,58,72,16,4,6,47,25,37,46,72,65,74,9,69,60,62,39,82,63,17,4,79,43,68,80,17,20,20,49,59,70,5,3,69,44,95,38,90,11,98,76,36,59,80,74,85,1,46,19,97,14,89,96,14,65,68,13,90,13,46,24,39,63,73,84,46,66,92,84,56,86,44,33,23,6,91,13,25,75,76,31,68,4,40,83,51,85,70,84,27,71,40,53,75,59,77,79,98,90,33,94,63,19,65,44,90,18,71,17,72,40,32,16,43,16,55,28,93,76,68,40,25,1,11,79,42,49,46,80,14,41,75,10,84,67,94,91,83,51,41,78,57,75,10,71,33,47,69,34,5,81,26,82,39,51,55,38,23,2,87,54,45,3,34,44,65,54,5,74,3,51,18,42,37,52,20,57,80,66,91,66,62,38,56,36,77,18,27,55,97,43,53,25,92,14,55,87,91,81,33,65,12,18,76,21,77,90,40,35,36,30,87,32,12,86,10,93,49,12,25,44,15,37,11,57,2,2,16,21,58,35,77,26,15,86,49,62,57,90,8,10,81,35,85,25,76,76,61,40,69,9,34,59,29,16,71,41,61,87,62,17,37,51,14,59,67,66,65,87,4,85,82,98,48,17,9,92,12,71,871073");

        }
    }
}