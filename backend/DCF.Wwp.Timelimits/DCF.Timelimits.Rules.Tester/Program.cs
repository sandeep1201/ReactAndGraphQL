using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DCF.Timelimits.Rules.Scripting;

namespace DCF.Timelimits.Rules.Tester
{
    class Program
    {
        static void Main(string[] args)
        {
            var stopwatch= new Stopwatch();
            var app = new Engine();
            stopwatch.Start();
            stopwatch.Stop();
            Console.WriteLine($"TestJavascriptNet done in {stopwatch.Elapsed.TotalMilliseconds}ms");

            //stopwatch.Restart();
            //app.TestEdgejs();
            //stopwatch.Stop();
            //Console.WriteLine($"TestEdgejs done in {stopwatch.Elapsed.TotalMilliseconds}ms");


            Console.ReadKey();

        }
    }
}
