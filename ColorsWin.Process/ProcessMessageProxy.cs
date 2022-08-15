using ColorsWin.Process.NamedPipe;
using System;

namespace ColorsWin.Process
{
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

                case ProcessMessageType.File:
                    CreateFileMessage(read);
                    break;

                default:
                    throw new NotImplementedException();

            }
        }


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


        private void CreateFileMessage(bool read)
        {
            if (read)
            {
                if (acceptMessage == null)
                {
                    acceptMessage = new FileMessage(processKey, read);
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
                    sendMessage = new FileMessage(processKey, read);
                }
            }
        }

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

        public ProcessMessageType GetProcessMessageType()
        {
            return processMessageType;
        }
    }


    public enum ProxyType
    {
        None,
        Read,
        Write,
        All
    }
}
