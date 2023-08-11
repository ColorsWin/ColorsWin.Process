using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ColorsWin.Process.NamedPipe
{
    class NamedPipeMessage : IProcessMessage
    {
        public static bool Wait { get; set; } = false;
        public const string ReplyMessageFlat = "&&Reply&&";
        private string processKey = "eventWaitName";
        private const string ProcessKeyTag = "_NamedPipe_ColorsWin";
        private static Dictionary<string, string> allProcessMessageCache = new Dictionary<string, string>();
        private NamedPipeClient client;
        private NamedPipeListenServer server; 

        public NamedPipeMessage(string processName, bool read)
        {
            this.processKey = processName;
            Init(read);
        }

        public void OnAcceptData(byte[] data)
        {
            if (AcceptData != null)
            {
                AcceptData(data);
            }
        }

        public string ReadMessage()
        {
            if (allProcessMessageCache.ContainsKey(processKey))
            {
                return allProcessMessageCache[processKey];
            }

            //To Do: get message from other Process

            return null;
        } 
      

        public bool SendMessage(string message)
        {
            allProcessMessageCache[processKey] = message;
            return client.SendMessage(message);
        }      

        public void Init(bool read)
        {
            if (read)
            {
                server = new NamedPipeListenServer(ProcessKeyTag + processKey, OnAcceptData);
                Task.Factory.StartNew(() =>
                {
                    server.Run();
                });
            }
            else
            {
                client = new NamedPipeClient(".", ProcessKeyTag + processKey);
            }
        }

        #region IProcessMessage 

        public event Action<byte[]> AcceptData;

        public byte[] ReadData()
        {
            return  null;
        }

        public bool SendData(byte[] data)
        {
            return client.SendData(data);
        }

        #endregion
    }
}
