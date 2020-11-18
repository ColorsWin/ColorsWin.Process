using Process.ShareTest;
using System;

namespace ProcessTest
{
    class Program
    {
        static void Main(string[] args)
        {
            //TestMemoryShare.OutPut();
            TestNamedPipe.OutPutProcessData();

            Console.Read();
        }
    }
}
