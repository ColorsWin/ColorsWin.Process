using ColorsWin.Process;

namespace Process.ShareTest
{
    class ManagerTest
    {
        public static void Output()
        {
            bool isAdmmin = ProcessHelper.IsRunAsAdmin();
            if (isAdmmin)
            {
                System.Console.WriteLine("Admin");
            }

            //TestFile.OutPut();
            TestMemoryShare.OutPut();
            //TestNamedPipe.OutPut();

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
