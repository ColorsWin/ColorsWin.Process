using Process.ShareTest;
using System;

namespace ProcessCoreTest
{
    class Program
    {
        static void Main(string[] args)
        {
            //TestNamedPipe.OutPutProcessData();
            TestMemoryShare.OutPut();
            Console.Read();
        }
    }
}
