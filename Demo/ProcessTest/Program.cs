using ColorsWin.Process;
using Process.ShareTest;
using System;

namespace ProcessTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var exit = ProcessHelper.HadRun();

            TestMemoryShare.OutPut();
            // TestNamedPipe.OutPutProcessData();

            Console.Read();
        }
    }
}
