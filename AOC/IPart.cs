using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC
{
    public interface IPart
    {
        void Run();
    }

    public abstract class BasePart : IPart
    {
        [DebuggerNonUserCode]

        protected void RunScenario(string name, Action run)
        {
            var sw = Stopwatch.StartNew();
            try
            {
                run();
                Console.WriteLine($"{name} completed in {sw.Elapsed}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{name} failed in {sw.Elapsed}: {ex}");
            }
        }

        public abstract void Run();
    }
}
