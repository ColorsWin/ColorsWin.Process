using ColorsWin.Process;
using System;

namespace Process.ShareTest
{
    public class TestMemoryShare
    {
        public static void OutPut()
        {
            BaseTest.TestSend();
            // BaseTest.TestBatchSend(); 
        } 

        public static void BaseOutput()
        {
            string processKey = "ProcessMessage_t";
            string processKey2 = "ProcessMessage_t2";

            //ProcessMessageManager.AllMessageEvent += (key, message) =>
            //{
            //    Console.WriteLine($"Accept Message By AllMessageEvent,ProcessKey:{key}---Content:----" + message);
            //};

            ProcessMessageManager.AcceptMessage(processKey, (message) =>
            {
                Console.WriteLine(processKey + "  Accept Message--------" + message);
            });

            ProcessMessageManager.AcceptMessage(processKey, (item) =>
            {
                Console.WriteLine(processKey + "  Accept Message【2】---------" + item);
            });

            //ProcessMessageManager.ListenMessage(processKey, (item) =>
            //{
            //    Console.WriteLine(processKey + " Accept Message【New】---------" + item);
            //}, true);


            ProcessMessageManager.AcceptMessage(processKey2, (item) =>
            {
                Console.WriteLine(processKey2 + "  Accept Message------" + item);
            });

            ProcessMessageManager.SendMessage(processKey, " Hello Word");

            ProcessMessageManager.SendMessage(processKey2, " Hello C#");
        }
    }
}
