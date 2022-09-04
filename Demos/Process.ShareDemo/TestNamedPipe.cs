using ColorsWin.Process;
using System;

namespace Process.ShareTest
{
    /// <summary>
    /// 管道测试
    /// </summary>
    public static class TestNamedPipe
    {
        public static void OutPut()
        {
            ProcessMessageConfig.ProcessMessageType = ProcessMessageType.NamedPipe;

            //BaseTest.TestSend();

            BaseTest.TestBatchSend();
        }

        public static void Test(bool isService = true)
        {
            string processKet = "Test";
            ProcessMessageConfig.NamePieService = isService;
            var runing = ProcessHelper.IsRuning(processKet);

            if (isService)
            {

            }
            else
            {

            }
        }

        private static void AcceptMessage(string message)
        {
            Console.WriteLine("收到消息" + message);
        }
    }
}
