namespace ColorsWin.Process
{
    /// <summary>
    /// 配置信息
    /// </summary>
    public class ProcessMessageConfig
    {
        /// <summary>
        /// 内存共享方式通信时,内存文件大小 
        /// </summary>
        public static long MemoryCapacity { get; set; } = 10 * 1024 * 1024;

        /// <summary>
        ///进程通信方式
        /// </summary>
        public static ProcessMessageType ProcessMessageType { get; set; } = ProcessMessageType.ShareMemory;
    }
}
