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
            memoryFile = MemoryMappedFileHelper.CreateMemoryMappedFileObj("Global\\" + processKey + MemoryMappedFileNameTag);
            eventWait = EventWaitHandleHelper.CreateEventHande("Global\\" + processKey + EventWaitNameTag, false);
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
                var data = ReadData();
                if (memoryFile.IsString)
                {
                    if (AcceptMessage != null)
                    {
                        var message = System.Text.Encoding.Default.GetString(data);
                        AcceptMessage(message);
                    }
                }
                else
                {
                    if (AcceptData != null)
                    {
                        AcceptData(data);
                    }
                }

                eventWait.Reset();
            }
        }



        #region 接口实现

        public event Action<string> AcceptMessage;
        public event Action<byte[]> AcceptData;

        /// <summary>
        /// 内存读取数据
        /// </summary>
        /// <returns></returns>
        public string ReadMessage()
        {
            var data = ReadData();
            if (data == null)
            {
                return null;
            }
            return System.Text.Encoding.Default.GetString(data);
        }

        /// <summary>
        /// 进程发送消息
        /// </summary>
        /// <returns></returns>
        public bool SendMessage(string message)
        {
            var data = System.Text.Encoding.Default.GetBytes(message);
            memoryFile.IsString = true;
            return SendData(data);
        }


        public bool SendData(byte[] message)
        {
            memoryFile.WriteData(message);
            memoryFile.IsString = false;
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

        public byte[] ReadData()
        {
            return memoryFile.ReadData();
        }

        #endregion
    }
}
