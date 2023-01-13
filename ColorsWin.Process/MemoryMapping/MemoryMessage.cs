using ColorsWin.Process.Helpers;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ColorsWin.Process
{
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

        private string GetProcessKey(string appendTag = null)
        {
            //bool isAdmmin = ProcessHelper.IsRunAsAdmin();
            var tag = ProcessMessageConfig.GlobalTag;
            return tag + processKey + appendTag;
        }

        private void Init(bool read)
        {
            memoryFile = MemoryMappedFileHelper.CreateMemoryMappedFileObj(GetProcessKey(MemoryMappedFileNameTag));

            eventWait = EventWaitHandleHelper.CreateEventHande(GetProcessKey(EventWaitNameTag), read);

            if (read)
            {
                Task.Factory.StartNew(WaitForMessage);
            }
        }

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
                        string message = string.Empty;
                        if (data != null)
                        {
                            message = ProcessMessageConfig.Encoding.GetString(data);
                        }
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

        #region IProcessMessage

        public event Action<string> AcceptMessage;
        public event Action<byte[]> AcceptData;

        public string ReadMessage()
        {
            var data = ReadData();
            if (data == null)
            {
                return null;
            }
            return ProcessMessageConfig.Encoding.GetString(data);
        }

        public bool SendMessage(string message)
        {
            var data = ProcessMessageConfig.Encoding.GetBytes(message);
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
                Thread.Sleep(ProcessMessageConfig.BatchSendWaitTime);
                eventWait.Reset();
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
