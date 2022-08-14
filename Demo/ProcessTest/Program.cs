using ColorsWin.Process.Helpers;
using Process.ShareTest;
using System;

namespace ProcessTest
{
    class Program
    {
        static void Main(string[] args)
        {
            //var obj = ByteConvertHelper.Object2Bytes(new string[] { "1\n2323213" });
            //var t = ByteConvertHelper.Bytes2Object<string[]>(obj);

            TestMemoryShare.OutPut();

            //TestNamedPipe.OutPutProcessData();

            Console.Read();
        }
    }
}
