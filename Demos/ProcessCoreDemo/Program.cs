using ColorsWin.Process;
using Process.ShareTest;
using System;
using System.Threading;

namespace ProcessCoreDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            TestNamedPipe.Test(false);

            //TestNamedPipe.OutPut();
            //System.Threading.Thread.Sleep(1000);
            //  TestMemoryShare.OutPut();
            Console.Read();
        }
    }
}
