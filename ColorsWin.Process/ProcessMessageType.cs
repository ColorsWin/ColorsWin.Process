namespace ColorsWin.Process
{
    /// <summary>
    /// 进程消息类型
    /// </summary>
    public enum ProcessMessageType
    {
        None,
        /// <summary>
        /// 共享内存
        /// </summary>
        ShareMemory,

        /// <summary>
        /// 管道
        /// </summary>
        NamedPipe,

        /// <summary>
        /// 文件方式
        /// </summary>
        File
    }
}
