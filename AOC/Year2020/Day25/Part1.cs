using System;
using System.Linq;

namespace AoC.Year2020.Day25
{
    public class Part1 : BasePart
    {
        protected void RunScenario(string title, int cardPublicKey, int doorPublicKey)
        {
            RunScenario(title, () =>
            {
                var cardLoopSize = FindLoopSize(7, cardPublicKey);
                var doorLoopSize = FindLoopSize(7, doorPublicKey);

                Console.WriteLine($"cardLoopSize: {cardLoopSize}");
                Console.WriteLine($"doorLoopSize: {doorLoopSize}");

                var key1 = Transform(cardPublicKey, doorLoopSize);
                var key2 = Transform(doorPublicKey, cardLoopSize);

                Console.WriteLine($"key1: {key1}");
                Console.WriteLine($"key2: {key2}");

            });
        }

        // 12495346 is wrong

        long Transform(long subjectNumber, long loopSize)
        {
            long value = 1;
            for(long i=0; i<loopSize; i++)
            {
                value = value * subjectNumber % 20201227;
            }

            return value;
        }

        long FindLoopSize(long subjectNumber, long targetValue)
        {
            long value = 1;
            var loopSize = 0;
            while(value != targetValue)
            {
                // a*x + b mod c
                value = value * subjectNumber % 20201227;
                loopSize++;
            }

            return loopSize;
        }

        public override void Run()
        {
            RunScenario("initial", 5764801, 17807724);
            //return;
            RunScenario("part1", 18499292, 8790390);
        }
    }
}
