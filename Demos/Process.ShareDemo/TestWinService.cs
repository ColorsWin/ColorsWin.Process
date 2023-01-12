using ColorsWin.Process;
using ColorsWin.Process.Helpers;
using System;

namespace Process.ShareTest
{
    class TestWinService
    {
        private static string processCommandLineTag = "ProcessCommandLineTag";
        private static string processCommandLine = "ProcessCommandLine";
        private static string processCommandLineResult = "processCommandLineResult";

        static TestWinService()
        {
            ProcessMessageManager.InitMessageType(processCommandLineTag, ProcessMessageType.File);
            ProcessMessageManager.InitMessageType(processCommandLine, ProcessMessageType.File);
            ProcessMessageManager.InitMessageType(processCommandLineResult, ProcessMessageType.File);
        }

        public static void RunService()
        {
            ProcessMessageConfig.Log = new FileLog("d:\\log.txt");
            ProcessMessageManager.SendMessage(processCommandLineTag, DateTime.Now.ToString() + "_Runing");

            try
            {
                ProcessMessageManager.AcceptMessage(processCommandLine, InvokeCommand, true);
            }
            catch (Exception ex)
            {
                LogHelper.Debug("Start Error:" + ex.Message);
            }
        }

        public static void StopService()
        {
            ProcessMessageManager.SendMessage(processCommandLineTag, "");
        }

        private static void InvokeCommand(string arg)
        {
            LogHelper.Debug("Message:" + arg);

            System.Threading.Thread.Sleep(new Random().Next(10, 600));

            ProcessMessageManager.SendMessage(processCommandLineResult, DateTime.Now + " Action Dome");
        }

        public static void RunClient()
        {
            var isRun = ProcessMessageManager.ReadMessage(processCommandLineTag);

            if (string.IsNullOrEmpty(isRun))
            {
                Console.WriteLine("WinService is not run");
                return;
            }

            string messageData = DateTime.Now + ":----Test----";
            Console.WriteLine("begin Send" + messageData);

            ProcessMessageManager.SendMessage(processCommandLine, messageData);
            ProcessMessageManager.AcceptMessage(processCommandLineResult, (message) =>
            {
                Console.WriteLine("Accept Message:" + message);
            });
        }

    }
}
