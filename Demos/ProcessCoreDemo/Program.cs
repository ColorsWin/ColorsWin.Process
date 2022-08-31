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
            //var handle = new EventWaitHandle(false, EventResetMode.ManualReset, "12333");
            //handle.Reset();

            var tempProcessKey = "test111";
            ProcessMessageManager.SendMessage(tempProcessKey, "1234");
            Console.ReadLine();

            //ProcessMessageManager.AcceptMessage(tempProcessKey, me =>
            //{
            //    System.Console.WriteLine(me);
            //}
            //);
            return;

            //var mess = ProcessMessageManager.ReadMessage(tempProcessKey);
            //if (!string.IsNullOrEmpty(mess))
            //{
            //    System.Console.WriteLine(mess);
            //}



            //TestNamedPipe.OutPutProcessData();
            //System.Threading.Thread.Sleep(1000);
            TestMemoryShare.OutPut();
            Console.Read();
        }
    }
}
