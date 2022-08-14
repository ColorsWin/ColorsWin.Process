using System;
using System.Threading.Tasks;

namespace ColorsWin.Process.NamedPipe
{
    /// <summary>
    /// 进程消息操作对象
    /// </summary>
    internal class NamedPipeMessage : IProcessMessage
    {
        /// <summary>
        /// 是否等待回应以后才能继续发送第二条消息
        /// </summary>
        public static bool Wait { get; set; } = false;

        /// <summary>
        /// wait=true,收到消息后回复
        /// </summary>
        public const string ReplyMessageFlat = "&&Reply&&";

        /// <summary>
        /// 进程名称
        /// </summary>
        public string ProcessKey { get; set; } = "eventWaitName";


        //标识符防止重复 
        private const string ProcessKeyTag = "_NamedPipe_ColorsWin";

        private NamedPipeClient client;
        private NamedPipeListenServer server;

        public event Action<string> AcceptMessage;

        public event Action<byte[]> AcceptData;

        public NamedPipeMessage(string processName, bool read)
        {
            this.ProcessKey = processName;
            Init(read);

        }


        public void OnAcceptData(byte[] data)
        {
            if (AcceptData != null)
            {
                AcceptData(data);
            }
        }
        public void OnAcceptMessage(string message)
        {
            if (AcceptMessage != null)
            {
                AcceptMessage(message);
            }
        }
        public string ReadMessage()
        {
            return null;
        }
        public byte[] ReadData()
        {
            return null;
        }
        public string WaitOneForMessage()
        {
            return null;
        }

        public bool SendMessage(string message)
        {
            return client.SendMessage(message);
        }
        public bool SendData(byte[] message)
        {
            return client.SendData(message);
        }


        internal void Init(bool read)
        {
            if (read)
            {
                server = new NamedPipeListenServer(ProcessKeyTag + ProcessKey, OnAcceptData, OnAcceptMessage);
                Task.Factory.StartNew(() =>
                {
                    server.Run();
                });
            }
            else
            {
                client = new NamedPipeClient(".", ProcessKeyTag + ProcessKey);
            }
        }


    }
}
