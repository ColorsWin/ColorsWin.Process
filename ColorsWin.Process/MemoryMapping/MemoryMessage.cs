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
                if (AcceptData != null)
                {
                    AcceptData(data);
                }
                eventWait.Reset();
            }
        }

        #region IProcessMessage

        public event Action<byte[]> AcceptData;

        public bool SendData(byte[] message)
        {
            memoryFile.WriteData(message);
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
