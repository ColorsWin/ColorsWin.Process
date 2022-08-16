using ColorsWin.Process;

namespace Process.ShareTest
{
    public class TestFile
    {
        public static void OutPut()
        {
            ProcessMessageConfig.ProcessMessageType = ProcessMessageType.File;
            //BaseTest.TestSend();
            BaseTest.TestBatchSend();
        }
    }
}
