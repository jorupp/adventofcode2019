using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC.Year2019.Day16
{
    public class Part1 : BasePart
    {
        protected void RunScenario(string title, string input, int numPhases)
        {
            RunScenario(title, () =>
            {
                var lines = input.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

                var numbers = lines[0].Select(i => long.Parse(i.ToString())).ToList();
                var basePattern = new long[] {0, 1, 0, -1};

                //List<long> getPattern(long ix)
                //{
                //    var length = numbers.Count;
                //    return Enumerable.Range(0, (int)(length + ix)).SelectMany(i =>
                //        basePattern.SelectMany(ii => Enumerable.Range(0, (int)ix + 1).Select(iii => ii)))
                //        .Skip(1).Take(length).ToList();
                //}

                List<long> RunPhase(List<long> data)
                {
                    return data.Select((i, ix) =>
                    {
                        //var pattern = patterns[ix];
                        return Math.Abs(
                                   data.Select((j, jx) =>
                                   {
                                       var pattern = basePattern[((jx + 1) / (ix + 1)) % 4];
                                       return (j * pattern) % 10;
                                   }).Sum()) % 10;
                    }).ToList();
                }

                for (var i = 0; i < numPhases; i++)
                {
                    Console.WriteLine($"Running phase {i}");
                    numbers = RunPhase(numbers);
                }

                //Console.WriteLine(string.Join(",", getPattern(0)));
                //Console.WriteLine(string.Join(",", getPattern(1)));
                //Console.WriteLine(string.Join(",", getPattern(2)));
                //Console.WriteLine(string.Join(",", getPattern(3)));

                Console.WriteLine(string.Join("", numbers.Take(8)));
            });
        }

        public override void Run()
        {
            //RunScenario("initial", @"12345678", 100);
            RunScenario("initial", @"80871224585914546619083218645595", 100);
            RunScenario("initial", @"19617804207202209144916044189917", 100);
            RunScenario("initial", @"69317163492948606335995924319873", 100);
            //return;
            RunScenario("part1", @"59734319985939030811765904366903137260910165905695158121249344919210773577393954674010919824826738360814888134986551286413123711859735220485817087501645023012862056770562086941211936950697030938202612254550462022980226861233574193029160694064215374466136221530381567459741646888344484734266467332251047728070024125520587386498883584434047046536404479146202115798487093358109344892308178339525320609279967726482426508894019310795012241745215724094733535028040247643657351828004785071021308564438115967543080568369816648970492598237916926533604385924158979160977915469240727071971448914826471542444436509363281495503481363933620112863817909354757361550", 100);

        }
    }
}
