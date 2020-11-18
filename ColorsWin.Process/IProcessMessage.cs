using System;

namespace ColorsWin.Process
{
    /// <summary>
    /// 进程消息信息
    /// </summary>
    public interface IProcessMessage
    {
        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        bool SendMessage(string message);

        /// <summary>
        /// 收到消息事件
        /// </summary>
        event Action<string> AcceptMessage;

        /// <summary>
        /// 获取一次
        /// </summary>
        /// <returns></returns>
        string WaitOneForMessage();

        /// <summary>
        /// 获取消息
        /// </summary>
        /// <returns></returns>
        string ReadMessage();

    }
}
