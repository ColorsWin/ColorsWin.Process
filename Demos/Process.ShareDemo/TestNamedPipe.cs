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
    }
}
