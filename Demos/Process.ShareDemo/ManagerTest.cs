using ColorsWin.Process;
using System;

namespace Process.ShareTest
{
    class ManagerTest
    {
        public static void Output(ProcessMessageType processMessageType = ProcessMessageType.None)
        {
            bool isAdmmin = ProcessHelper.IsRunAsAdmin();
            if (isAdmmin)
            {
                System.Console.WriteLine("Admin");
            }

            switch (processMessageType)
            {
                case ProcessMessageType.None:
                    BaseTest.TypeOutput();
                    break;
                case ProcessMessageType.ShareMemory:
                    TestMemoryShare.OutPut();
                    break;
                case ProcessMessageType.NamedPipe:
                    TestNamedPipe.OutPut();
                    break;
                case ProcessMessageType.File:
                    TestFile.OutPut();
                    break;
                default:
                    break;
            }

            var data = ProcessManager.GetAllProcessInfo();
            foreach (var item in data)
            {
                string tempInfo = null;
                foreach (var key in item.Info)
                {
                    tempInfo += $"ProcessKey={key.ProcessKey},MessageType={ key.MessageType},ProxyType={ key.ProxyType} " + "\r\n";
                }
                Console.WriteLine("【" + item.ProcessId + "】:\r\n" + tempInfo);
            }

            //Run ProcessServiceDemo Test RunService
            //TestWinService.RunClient();

            //if (isAdmmin)
            //{          
            //    TestWinService.RunService();
            //}
            //else
            //{
            //    TestWinService.RunClient();
            //}
        }
    }
}
