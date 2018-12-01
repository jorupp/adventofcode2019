using System;
using System.Diagnostics;

namespace AoC
{
    class Program
    {
        static void Main(string[] args)
        {
            //new Year2015.Day22.Part1().Run();
            //new Year2015.Day22.Part2().Run();

            //new Year2016.Day11.Part1().Run();

            //new Year2018.Day0.Part1().Run();
            //new Year2018.Day0.Part2().Run();

            //new Year2018.Day1.Part1().Run();
            new Year2018.Day1.Part2().Run();

            if (Debugger.IsAttached)
            {
                Console.WriteLine("Press enter to exit");
                Console.ReadLine();
            }
        }
    }
}
