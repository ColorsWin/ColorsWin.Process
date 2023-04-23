using ColorsWin.Process;

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
