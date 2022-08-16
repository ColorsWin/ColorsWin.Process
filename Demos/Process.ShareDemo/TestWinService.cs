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

        public static void Output()
        {
            ProcessMessageManager.InitMessageType(processCommandLineTag, ProcessMessageType.File);

            // Not use ServiceController, Beacuse The service is not necessarily my own
            // new ServiceController(serviceName).Status == ServiceControllerStatus.Running

            var isRun = ProcessMessageManager.ReadMessage(processCommandLineTag);

            if (!string.IsNullOrEmpty(isRun))
            {
                ProcessMessageManager.InitMessageType(processCommandLine, ProcessMessageType.File);
                ProcessMessageManager.InitMessageType(processCommandLineResult, ProcessMessageType.File);
                ProcessMessageManager.AcceptMessage(processCommandLineResult, (mess) =>
                {
                    Console.WriteLine(mess);
                });

                string message = DateTime.Now + "：Test";
                Console.WriteLine("begin Send" + message);
                ProcessMessageManager.SendMessage(processCommandLine, message);
            }
            else
            {
                //Run();
                Console.WriteLine("Norun");
            }
        }

        private static void Run()
        {
            int rand = new Random().Next(10000);
            ProcessMessageConfig.Log = new FileLog($"d:\\log_{rand}.txt");
            try
            {
                ProcessMessageManager.InitMessageType(processCommandLineTag, ProcessMessageType.File);
                LogHelper.Debug(ProcessMessageManager.GetProcessMessageType(processCommandLineTag).ToString());

                ProcessMessageManager.SendMessage(processCommandLineTag, DateTime.Now.ToString() + "_Runing");

                ProcessMessageManager.InitMessageType(processCommandLine, ProcessMessageType.File);
                ProcessMessageManager.AcceptMessage(processCommandLine, InvokeCommand, true);
            }
            catch (Exception ex)
            {
                LogHelper.Debug("Start Error:" + ex.Message);
            }
        }

        private static void InvokeCommand(string arg)
        {
            Console.WriteLine(arg);
            LogHelper.Debug("Message:" + arg);
        }

    }
}
