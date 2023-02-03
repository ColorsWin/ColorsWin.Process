using ColorsWin.Process;
using System;

namespace Process.ShareTest
{
    class BaseTest
    {
        public static void TestSend()
        {
            var processKey = "processKey_" + ProcessMessageConfig.ProcessMessageType;
            bool isRun = ProcessHelper.HadRun(processKey);          
            if (isRun)
            {
                StartSend(processKey);
            }
            else
            {
                StartAccept(processKey);
            }
        }

        private static void StartSend(string processKey)
        {
            Console.WriteLine("Send: 【 Key】 " + processKey);
            string tempData = Console.ReadLine();
            while (tempData != "exit")
            {
                ProcessMessageManager.SendMessage(processKey, tempData);
                Console.WriteLine("SendMessage:" + tempData);

                var data = System.Text.Encoding.Default.GetBytes(tempData);
                ProcessMessageManager.SendData(processKey, data);
                Console.WriteLine("SendData:" + tempData);

                tempData = Console.ReadLine();
            }
        }

        private static void StartAccept(string processKey)
        {
            Console.WriteLine("Accept 【 Key】 " + processKey);
            ProcessMessageManager.AcceptMessage(processKey, (item) =>
            {
                Console.WriteLine(processKey + "：AcceptMessage--------" + item);
            });

            //ProcessMessageManager.AcceptData(processKey, (item) =>
            //{
            //    if (item == null)
            //    {
            //        return;
            //    }
            //    var data = System.Text.Encoding.Default.GetString(item);
            //    Console.WriteLine(processKey + "：AcceptData:--------" + data);
            //});

            string tempData = Console.ReadLine();
            while (tempData != "exit")
            {
                ProcessMessageManager.SendMessage(processKey, tempData);
                Console.WriteLine("SendMessage:" + tempData);
                tempData = Console.ReadLine();
            }
        }


        private const int MaxCount = 1000;

        public static void TestBatchSend()
        {
            string testKey = "testKey_" + ProcessMessageConfig.ProcessMessageType;
            bool isRun = ProcessHelper.HadRun(testKey);
            if (isRun)
            {
                Console.WriteLine("Send: 【 Key】" + testKey);
                for (int i = 0; i < MaxCount; i++)
                {
                    string message = "Start---" + i.ToString() + "\r\n" + "OwnerMessage---End";

                    ProcessMessageManager.SendMessage(testKey, message);

                    Console.WriteLine("SendMessage:" + message);
                }
            }
            else
            {
                Console.WriteLine("Accept 【 Key】" + testKey);
                ProcessMessageManager.AcceptMessage(testKey, AcceptMessage);
            }
            Console.ReadLine();
        }

        private static int i = 0;
        private static void AcceptMessage(string message)
        {
            Console.WriteLine("AcceptMessage:" + message);
            Console.Title = i.ToString();
            i++;
        }
    }
}
