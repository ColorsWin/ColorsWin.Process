using ColorsWin.Process.NamedPipe;
using System;

namespace ColorsWin.Process.Ext
{
    /// <summary>
    /// 进程读写消息代理
    /// </summary>
    public class ProcessMessageProxy
    {
        private IProcessMessage acceptMessage;
        private IProcessMessage sendMessage;
        private string processKey;
        private ProcessMessageType processMessageType;
        private Action<string> actionMessage;
        private Action<byte[]> actionData;

        public ProcessMessageProxy(string processName)
        {
            this.processKey = processName;
            this.processMessageType = ProcessMessageConfig.ProcessMessageType;
        }

        public ProcessMessageProxy(string processName, ProcessMessageType processMessageType = ProcessMessageType.ShareMemory)
        {
            this.processKey = processName;
            this.processMessageType = processMessageType;
        }

        internal void InitListenMessage()
        {
            InitProcessMessage(true);
        }

        internal void ResetListenMessage()
        {
            if (acceptMessage != null)
            {
                acceptMessage = null;
                InitProcessMessage(true);
            }
            if (sendMessage != null)
            {
                sendMessage = null;
                InitProcessMessage(false);
            }
        }

        public bool SendData(byte[] data)
        {
            InitProcessMessage(false);
            return sendMessage.SendData(data);
        }

        public byte[] ReadData()
        {
            InitProcessMessage(true);
            return acceptMessage.ReadData();
        }

        public bool SendMessage(string message)
        {
            InitProcessMessage(false);
            return sendMessage.SendMessage(message);
        }
        public string ReadMessage()
        {
            InitProcessMessage(true);
            return acceptMessage.ReadMessage();
        }

        /// <summary>
        /// 收到消息内容
        /// </summary>
        /// <param name="message"></param>
        protected virtual void OnMessage(string message)
        {
            if (message == null)
            {
                return;
            }
            ProcessMessageManager.OnAcceptMessage(processKey, message);
            if (actionMessage != null)
            {
                actionMessage(message);
            }
        }

        /// <summary>
        /// 初始进程信息
        /// </summary>
        /// <param name="read"> true创建读取信息，false创建写入</param>
        private void InitProcessMessage(bool read)
        {
            switch (processMessageType)
            {
                case ProcessMessageType.ShareMemory:
                    CreateShareMemoryMessage(read);
                    break;
                case ProcessMessageType.NamedPipe:
                    CreateNamedPipeMessage(read);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 创建内存消息
        /// </summary>
        /// <param name="read"></param>
        private void CreateShareMemoryMessage(bool read)
        {
            if (read)
            {
                if (acceptMessage == null)
                {
                    acceptMessage = new MemoryMessage(processKey, read);
                    acceptMessage.AcceptMessage += OnMessage;
                    acceptMessage.AcceptData += (data) =>
                    {
                        if (actionData != null)
                        {
                            actionData(data);
                        }
                    };
                }
            }
            else
            {
                if (sendMessage == null)
                {
                    sendMessage = new MemoryMessage(processKey, read);
                }
            }
        }

        /// <summary>
        /// 创建命名管道
        /// </summary>
        /// <param name="read"></param>
        private void CreateNamedPipeMessage(bool read)
        {
            if (read)
            {
                if (acceptMessage == null)
                {
                    acceptMessage = new NamedPipeMessage(processKey, read);
                    acceptMessage.AcceptMessage += OnMessage;
                    acceptMessage.AcceptData += (data) =>
                    {
                        if (actionData != null)
                        {
                            actionData(data);
                        }
                    };
                }
            }
            else
            {
                if (sendMessage == null)
                {
                    sendMessage = new NamedPipeMessage(processKey, read);
                }
            }
        }


        /// <summary>
        /// 改变接受消息
        /// </summary>
        public void ChangeAction(Action<string> actionMessage, bool reset)
        {
            if (reset || this.actionMessage == null)
            {
                this.actionMessage = actionMessage;
            }
            else
            {
                this.actionMessage += actionMessage;
            }
        }

        /// <summary>
        /// 改变接受消息
        /// </summary>
        /// <param name="actionData"></param>
        /// <param name="reset"></param>
        public void ChangeAction(Action<byte[]> actionData, bool reset)
        {
            if (reset || this.actionData == null)
            {
                this.actionData = actionData;
            }
            else
            {
                this.actionData += actionData;
            }
        }

        /// <summary>
        /// 停止
        /// </summary>
        public void Stop(ProxyType type = ProxyType.All)
        {
            if (type == ProxyType.All || type == ProxyType.Read)
            {
                acceptMessage = null;
            }
            else if (type == ProxyType.All || type == ProxyType.Write)
            {
                sendMessage = null;
            }
        }

        /// <summary>
        /// 获取代理消息类型，读或者写
        /// </summary>
        /// <returns></returns>
        public ProxyType GetProxyType()
        {
            if (acceptMessage == null || sendMessage == null)
            {
                return ProxyType.None;
            }
            else if (acceptMessage != null && sendMessage != null)
            {
                return ProxyType.All;
            }
            else if (acceptMessage == null)
            {
                return ProxyType.Read;
            }
            else
            {
                return ProxyType.Write;
            }
        }
    }

    /// <summary>
    /// 代理消息类型
    /// </summary>
    public enum ProxyType
    {
        None,
        Read,
        Write,
        All
    }
}
