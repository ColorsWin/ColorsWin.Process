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

            //TestWinService.RunClient();

            return;

            if (isAdmmin)
            {
                System.Console.WriteLine("服务端");
                TestWinService.RunService();
            }
            else
            {
                TestWinService.RunClient();
            }
        }
    }
}
