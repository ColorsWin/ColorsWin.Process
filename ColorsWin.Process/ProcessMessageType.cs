using System;
using System.Collections.Generic;
using System.Text;

namespace ColorsWin.Process
{
    /// <summary>
    /// 进程消息类型
    /// </summary>
    public enum ProcessMessageType
    {
        /// <summary>
        /// 共享内存
        /// </summary>
        ShareMemory,
        /// <summary>
        /// 管道
        /// </summary>
        NamedPipe
    }
}
