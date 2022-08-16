using ColorsWin.Process;
using System;

namespace Process.ShareTest
{
    public class TestMemoryShare
    {
        private static bool HadRun(string processKey)
        {
            var defaultData = ProcessMessageManager.ReadMessage(processKey);
            bool isRun = !string.IsNullOrEmpty(defaultData);
            if (!isRun)
            {
                ProcessMessageManager.SendMessage(processKey, "Runing");
            }
            return isRun;
        }

        public static void OutPut()
        {
            HadRun("TestProcess_Key");
            BaseTest.TestSend();
           // BaseTest.TestBatchSend();
        }

        public static void BaseOutput()
        {
            string processKey = "ProcessMessage_t";
            string processKey2 = "ProcessMessage_t2";

            ProcessMessageManager.AllMessageEvent += (key, message) =>
            {
                Console.WriteLine($"AllMessage-进程Key:{key}---消息为:----" + message);
            };

            ProcessMessageManager.AcceptMessage(processKey, (item) =>
            {
                Console.WriteLine(processKey + "监听--------" + item);
            });

            ProcessMessageManager.AcceptMessage(processKey, (item) =>
            {
                Console.WriteLine(processKey + "监听[第二次]---------" + item);
            });

            //ProcessMessageManager.ListenMessage(processKey, (item) =>
            //{
            //    Console.WriteLine(processKey + "监听[屏蔽之前消息]---------" + item);
            //}, true);


            ProcessMessageManager.AcceptMessage(processKey2, (item) =>
            {
                Console.WriteLine(processKey2 + "监听--------" + item);
            });

            ProcessMessageManager.SendMessage(processKey, " 你好 进程1");

            ProcessMessageManager.SendMessage(processKey2, " 我是进程2");
        }
    }
}
