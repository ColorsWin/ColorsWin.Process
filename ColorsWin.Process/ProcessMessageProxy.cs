using ColorsWin.Process.NamedPipe;
using System;

namespace ColorsWin.Process.Ext
{
    /// <summary>
    /// 进程读写消息代理
    /// </summary>
    public class ProcessMessageProxy
    {
        private IProcessMessage readMessage;
        private IProcessMessage sendMessage;
        private string processKey;
        private ProcessMessageType processMessageType;
        private Action<string> actionMessage;

        public ProcessMessageProxy(string processName, ProcessMessageType processMessageType = ProcessMessageType.ShareMemory)
        {
            this.processKey = processName;
            this.processMessageType = processMessageType;
        }

        internal void InitListenMessage()
        {
            InitProcessMessage(true);
        }

        public string ReadData()
        {
            InitProcessMessage(true);
            return readMessage.ReadMessage();
        }

        public string ReadDataWait()
        {
            InitProcessMessage(true);
            return readMessage.WaitOneForMessage();
        }

        public bool WriteData(string message)
        {
            InitProcessMessage(false);
            return sendMessage.SendMessage(message);
        }

        protected virtual void OnMessage(string message)
        {
            ProcessMessageManager.OnAcceptMessage(processKey, message);
            if (actionMessage != null)
            {
                actionMessage(message);
            }
        }

        /// <summary>
        /// 初始进程信息
        /// </summary>
        /// <param name="read"></param>
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

        private void CreateShareMemoryMessage(bool read)
        {
            if (read)
            {
                if (readMessage == null)
                {
                    readMessage = new MemoryMessage(processKey, read);
                    readMessage.AcceptMessage += OnMessage;
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

        private void CreateNamedPipeMessage(bool read)
        {
            if (read)
            {
                if (readMessage == null)
                {
                    readMessage = new NamedPipeMessage(processKey, read);
                    readMessage.AcceptMessage += OnMessage;
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
        /// 停止
        /// </summary>
        public void Stop()
        {
            readMessage = null;
            sendMessage = null;
        }
    }
}
