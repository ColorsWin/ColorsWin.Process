using Process.ShareTest;
using System;

namespace ProcessCoreDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            //TestNamedPipe.OutPutProcessData();
            //System.Threading.Thread.Sleep(1000);
            TestMemoryShare.OutPut();
            Console.Read();
        }
    }
}
