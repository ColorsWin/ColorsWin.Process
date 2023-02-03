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
        private Action<byte[]> actionData;

        public ProcessMessageProxy(string processKey)
        {
            this.processKey = processKey;
            this.processMessageType = ProcessMessageConfig.ProcessMessageType;
        }

        public ProcessMessageProxy(string processKey, ProcessMessageType processMessageType = ProcessMessageType.ShareMemory)
        {
            this.processKey = processKey;
            this.processMessageType = processMessageType;
        }

        public ProcessMessageType GetProcessMessageType()
        {
            return processMessageType;
        }
        public bool SendData(byte[] data)
        {
            InitMessageType(false);
            return sendMessage.SendData(data);
        }

        public byte[] ReadData()
        {
            InitMessageType(true);
            return acceptMessage.ReadData();
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

        internal void InitMessage()
        {
            InitMessageType(true);
        }

        public void Reset(ProcessMessageType processMessageType = ProcessMessageType.ShareMemory)
        {
            this.processMessageType = processMessageType;
            if (acceptMessage != null)
            {
                acceptMessage = null;
                InitMessageType(true);
            }

            if (sendMessage != null)
            {
                sendMessage = null;
                InitMessageType(false);
            }
        }

        internal void InitMessageType(bool read)
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


    public enum ProxyType
    {
        None,
        Read,
        Write,
        All
    }
}
