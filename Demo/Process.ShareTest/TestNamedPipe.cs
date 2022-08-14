using ColorsWin.Process;
using System;

namespace Process.ShareTest
{
    /// <summary>
    /// 管道测试
    /// </summary>
    public static class TestNamedPipe
    {
        public const int MaxCount = 5000;

        public static void OutPutProcessData()
        {
            string testKey = "testKey";
            //NamedPipeMessage
            ProcessMessageConfig.ProcessMessageType = ProcessMessageType.NamedPipe;

            bool isRun = ProcessHelper.HadRun(testKey);
            if (isRun)
            {
                Console.WriteLine("当前程序将作为发送端: 因为已经运行该程序------:当前key为" + testKey);
                for (int i = 0; i < MaxCount; i++)
                {
                    ProcessMessageManager.SendMessage(testKey, i.ToString() + "\r\n" + "自己数据");
                    Console.WriteLine("消息发送:" + i);
                }
            }
            else
            {
                //var message = ProcessMessageManager.ReadDataWait(testKey);

                Console.WriteLine("当前程序将作为服务端,监听消息已经启动------:当前key为" + testKey);
                ProcessMessageManager.AcceptMessage("testKey", DefalutMessageManager_AcceptMessage);

                //ProcessMessageManager.ListenMessage("testKey", (item) => { Console.WriteLine("第二次"); }, false);

                //ProcessMessageManagerExt.ListenMessage("testKey");
                //ProcessMessageManagerExt.AcceptMessage += (item) =>
                //{
                //    Console.WriteLine("第二次");
                //};
                //ProcessMessageManagerExt.AcceptMessage += DefalutMessageManager_AcceptMessage;

                //ProcessMessageManagerExt.ListenMessage("testKey", (item) => { Console.WriteLine("第二次" + item); }, false);
            }
            Console.ReadLine();
        }

        private static int i = 0;
        private static void DefalutMessageManager_AcceptMessage(string message)
        {
            Console.WriteLine("消息接受:" + message);
            Console.Title = i.ToString();
            i++;
        }
    }
}
