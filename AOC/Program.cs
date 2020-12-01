using System;
using System.Diagnostics;

namespace AoC
{
    class Program
    {
        static void Main(string[] args)
        {
            //new Year2020.Day1.Part1().Run();
            //new Year2020.Day1.Part2().Run();

            new Year2020.Day2.Part1().Run();
            //new Year2020.Day2.Part2().Run();

            if (Debugger.IsAttached)
            {
                Console.WriteLine("Press enter to exit");
                Console.ReadLine();
            }
        }
    }
}
