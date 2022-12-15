using System;
using System.Diagnostics;

namespace AOC
{
    class Program
    {
        static void Main(string[] args)
        {
            new Year2022.Day5.Part1().Run();
            new Year2022.Day5.Part2().Run();

            if (Debugger.IsAttached)
            {
                Console.WriteLine("Press enter to exit");
                Console.ReadLine();
            }
        }
    }
}
