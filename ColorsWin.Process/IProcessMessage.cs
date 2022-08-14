using System;

namespace ColorsWin.Process
{
    /// <summary>
    /// 进程消息信息
    /// </summary>
    public interface IProcessMessage
    {
        event Action<string> AcceptMessage;

        /// <summary>
        /// 收到消息事件
        /// </summary>
        event Action<byte[]> AcceptData;


        bool SendMessage(string message);

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        bool SendData(byte[] message);


        /// <summary>
        /// 获取消息
        /// </summary>
        /// <returns></returns>
        string ReadMessage();

        /// <summary>
        /// 读取数据
        /// </summary>
        /// <returns></returns>
        byte[] ReadData(); 
    }
}
