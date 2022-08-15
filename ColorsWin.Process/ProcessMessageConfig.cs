namespace ColorsWin.Process
{
    /// <summary>
    /// 配置信息
    /// </summary>
    public class ProcessMessageConfig
    {
        public static long MemoryCapacity { get; set; } = 10 * 1024 * 1024;

        public static ProcessMessageType ProcessMessageType { get; set; } = ProcessMessageType.ShareMemory;

        internal static string GlobalTag = "Global\\";

        public static int BatchSendWaitTime = 5;
    }
}
