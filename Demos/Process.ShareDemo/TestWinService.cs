using ColorsWin.Process;
using ColorsWin.Process.Helpers;
using System;

namespace Process.ShareTest
{
    class TestWinService
    {
        private static string processCommandLineTag = "ProcessCommandLineTag3";
        private static string processCommandLine = "ProcessCommandLine3";
        private static string processCommandLineResult = "processCommandLineResult3";

        static TestWinService()
        {
            ProcessMessageConfig.GlobalTag = "Global\\";

            //ProcessMessageManager.InitMessageType(processCommandLineTag, ProcessMessageType.File);
            //ProcessMessageManager.InitMessageType(processCommandLine, ProcessMessageType.File);
            //ProcessMessageManager.InitMessageType(processCommandLineResult, ProcessMessageType.File);
        }


        public static void RunService()
        {
            ProcessMessageConfig.Log = new FileLog("d:\\log.txt");

            // Client use the process key send message  
            ProcessMessageManager.CreateProcessKey(processCommandLineResult);

            string message = DateTime.Now.ToString() + "_Runing";
            LogHelper.Debug("Service Send Message:" + message);
            ProcessMessageManager.SendMessage(processCommandLineTag, message);

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
            string message = DateTime.Now + " Action Stop ";
            LogHelper.Debug("Service Send Message:" + message);
            ProcessMessageManager.SendMessage(processCommandLineResult, message);

            ProcessMessageManager.SendMessage(processCommandLineTag, "");
        }

        private static void InvokeCommand(string arg)
        {
            LogHelper.Debug("Service Accept Message:" + arg);
            System.Threading.Thread.Sleep(new Random().Next(10, 50));


            string message = DateTime.Now + " Action Message";
            LogHelper.Debug("Service Send Message:" + message);
            ProcessMessageManager.SendMessage(processCommandLineResult, message);
        }

        public static void RunClient()
        {
            var isRun = ProcessMessageManager.ReadMessage(processCommandLineTag);
            if (string.IsNullOrEmpty(isRun))
            {
                Console.WriteLine("WinService is not run");
                //return;
            }
            else
            {
                Console.WriteLine("WinService is  runing");

                string messageData = DateTime.Now + ":----Test----";
                Console.WriteLine("begin Send" + messageData);
                ProcessMessageManager.SendMessage(processCommandLine, messageData);
            }

            ProcessMessageManager.AcceptMessage(processCommandLineResult, (m) =>
            {
                Console.WriteLine("Accept Message:" + m);
            });
        }
    }
}
