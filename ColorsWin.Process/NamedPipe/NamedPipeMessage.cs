using System;
using System.Threading.Tasks;
using System.Collections.Generic;

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


        private string processKey = "eventWaitName";
        private const string ProcessKeyTag = "_NamedPipe_ColorsWin";

        private static Dictionary<string, string> allProcessMessageCache = new Dictionary<string, string>();

        private NamedPipeClient client;
        private NamedPipeListenServer server;

        public event Action<string> AcceptMessage;

        public event Action<byte[]> AcceptData;

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

        public void OnAcceptMessage(string message)
        {
            //if (message)
            //{

            //}
            if (AcceptMessage != null)
            {
                AcceptMessage(message);
            }
        }

        public string ReadMessage()
        {
            if (allProcessMessageCache.ContainsKey(processKey))
            {
                return allProcessMessageCache[processKey];
            }
            //get message from other Process

            return null;
        }

        public byte[] ReadData()
        {
            return null;
        }


        public bool SendMessage(string message)
        {
            allProcessMessageCache[processKey] = message;
            if (server != null)
            {
                server.SendMessage(message);
                return true;
            }
            return client.SendMessage(message);
        }

        public bool SendData(byte[] message)
        {
            return client.SendData(message);
        }

        public void Init(bool read)
        {
            if (read)
            {
                //if (server != null)
                //{
                //    return;
                //}
                server = new NamedPipeListenServer(ProcessKeyTag + processKey, OnAcceptData, OnAcceptMessage);
                Task.Factory.StartNew(() =>
                {
                    server.Run();
                });
            }
            else
            {
                //if (client != null)
                //{
                //    return;
                //}
                client = new NamedPipeClient(".", ProcessKeyTag + processKey);
            }
        }
    }
}
