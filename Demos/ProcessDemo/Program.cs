using Process.ShareTest;
using System;

namespace ProcessDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            //OtherTest.Output();
            //TestMemoryShare.OutPut();
            // TestFile.OutPut();
            //TestNamedPipe.OutPut();
            TestNamedPipe.Test(true);
            //TestWinService.Output();
            Console.Read();
        }
    }
}
