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
            // TestNamedPipe.OutPut();
            TestWinService.Output();

            Console.Read();
        }
    }
}
