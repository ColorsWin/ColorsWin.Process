using ColorsWin.Process;

namespace Process.ShareTest
{
    public static class TestNamedPipe
    {
        public static void OutPut()
        {
            ProcessMessageConfig.ProcessMessageType = ProcessMessageType.NamedPipe;
            //BaseTest.TestSend();
            BaseTest.TestBatchSend();
        }
    }
}
