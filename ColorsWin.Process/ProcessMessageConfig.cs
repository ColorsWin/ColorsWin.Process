namespace ColorsWin.Process
{
    public class ProcessMessageConfig
    {
        public static long MemoryCapacity { get; set; } = 10 * 1024 * 1024;

        public static ProcessMessageType ProcessMessageType { get; set; } = ProcessMessageType.ShareMemory;

        internal static string GlobalTag = "Global\\";

        public static int BatchSendWaitTime = 5;

        public static ILog Log { get; set; }
    }
}
