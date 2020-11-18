using ColorsWin.Process.Helpers;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ColorsWin.Process
{
    /// <summary>
    /// 进程消息操作对象
    /// </summary>
    class MemoryMessage : IProcessMessage
    {
        private EventWaitHandle eventWait = null;
        private MemoryMappedFileObj memoryFile = null;
        //标识符防止重复
        private const string MemoryMappedFileNameTag = "_MemoryMappedFileName_ColorsWin";
        private const string EventWaitNameTag = "_EventWaitName_ColorsWin";
        private string processKey = "eventWaitName";


        public MemoryMessage(string processName, bool read)
        {
            this.processKey = processName;
            Init(read);
        }

        /// <summary>
        /// 初始化通知和内存共享文件
        /// </summary>
        private void Init(bool read)
        {
            memoryFile = MemoryMappedFileHelper.CreateMemoryMappedFileObj(processKey + MemoryMappedFileNameTag);
            eventWait = EventWaitHandleHelper.CreateEventHande(processKey + EventWaitNameTag, false);
            if (read)
            {
                Task.Factory.StartNew(WaitForMessage);
            }
        }

        /// <summary>
        /// 一直等待消息
        /// </summary>
        private void WaitForMessage()
        {
            while (true)
            {
                eventWait.WaitOne();
                var message = ReadMessage();
                OnAcceptMessage(message);
                eventWait.Reset();
            }
        }

        /// <summary>
        /// 执行事件
        /// </summary>
        /// <param name="message"></param>
        protected void OnAcceptMessage(string message)
        {
            if (AcceptMessage != null)
            {
                AcceptMessage(message);
            }
        }

        #region 接口实现

        public event Action<string> AcceptMessage;

        /// <summary>
        /// 内存读取数据
        /// </summary>
        /// <returns></returns>
        public string ReadMessage()
        {
            return memoryFile.ReadData();
        }
        /// <summary>
        /// 进程发送消息
        /// </summary>
        /// <returns></returns>
        public bool SendMessage(string message)
        {
            memoryFile.WriteData(message);

            if (eventWait != null)
            {
                eventWait.Set();

                //暂时未处理 批量快速发消息 会导致接受不全【写入速度过快，读取速度跟不上】

                Thread.Sleep(10);

                eventWait.Reset();//如果注释掉这句代码   A先发送消息,B在运行程序也会收到
                return true;
            }
            return false;
        }

        /// <summary>
        /// 等待读取一次消息
        /// </summary>
        public string WaitOneForMessage()
        {
            eventWait.WaitOne();
            var message = ReadMessage();
            eventWait.Reset();
            return message;
        }
        #endregion 
    }
}
