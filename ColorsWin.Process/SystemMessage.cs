using ColorsWin.Process.Helpers;
using ColorsWin.Process.NamedPipe;
using System;
using System.Threading;

namespace ColorsWin.Process
{
    internal class SystemMessageManager
    {
        private static ProcessMessageProxy sysProcessMessageProxy;

        static SystemMessageManager()
        {
            Init();
        }

        private const string ServiceTag = "ColorsWin.Service";
        private const string ClientTag = "ColorsWin.Client";
        public static void Init()
        {
            if (sysProcessMessageProxy != null)
            {
                return;
            }
            sysProcessMessageProxy = new ProcessMessageProxy("sys_ColorsWin", ProcessMessageType.ShareMemory);
            sysProcessMessageProxy.ChangeAction(OnSystemMessage, true);
            sysProcessMessageProxy.InitMessage();
            Start();
        }

        private static void OnSystemMessage(byte[] data)
        {
            var message = ObjectSerializeHelper.Deserialize(data) as SystemMessage;
            if (message.ProcessId == GetProcessId())
            {
                return;
            }

            if (Token == message.Token)
            {
                LogHelper.Debug("Accept SystemMessage" + message.Data + message.ProcessId);
                sendResult = true;
                autoEvent.Set();
                return;
            }

            // LogHelper.Debug("Accept SystemMessage:" + message.Data + message.ProcessId);

            if (message.CmdType == 1)
            {
                if (message.Data == ServiceTag)
                {
                    LogHelper.Debug("收到服务器启动消息:------" + "开始连接服务器");
                    SystemNamedPipeMessage.Singleton.Connect();
                }
                else
                {
                    LogHelper.Debug("收到客户端启动消息:------");
                }
            }
            else if (message.CmdType == 100)
            {
                if (ProcessMessageManager.IsExistProcessKey(message.Data))
                {
                    ReplayMessage(message);
                }
            }
        }

        private static int GetProcessId()
        {
            return System.Diagnostics.Process.GetCurrentProcess().Id;
        }

        private static void ReplayMessage(SystemMessage message)
        {
            message.ProcessId = GetProcessId();
            message.Time = DateTime.Now;
            LogHelper.Debug("Replay:" + message.Data + message.ProcessId);
            System.Threading.Thread.Sleep(50);
            SendSystemMessage(message);
        }

        private static void SendSystemMessage(SystemMessage message)
        {
            var data = ObjectSerializeHelper.Serialize(message);
            sysProcessMessageProxy.SendData(data);
        }

        private static AutoResetEvent autoEvent = new AutoResetEvent(false);
        private static string Token = null;
        private static bool sendResult = false;
        internal static void SystemSendMessage(int cmd, string data)
        {
            sendResult = false;
            Token = Guid.NewGuid().ToString();
            var message = new SystemMessage
            {
                ProcessId = GetProcessId(),
                Token = Token,
                CmdType = cmd,
                Time = DateTime.Now,
                Data = data
            };
            SendSystemMessage(message);

            //  LogHelper.Debug("SendSystemMessage::" + data + message.ProcessId);
        }



        internal static void Start()
        {
            string startTag = ClientTag;
            if (ProcessMessageConfig.NamePieService)
            {
                startTag = ServiceTag;
                var t = SystemNamedPipeMessage.Singleton;
            }
            else
            {
                SystemNamedPipeMessage.Singleton.Connect();
            }

            SystemSendMessage(1, startTag); 
           

            LogHelper.Debug("程序启动::" + startTag);
        }


        internal static bool ProcessIsRuning(string processKey)
        {
            //SystemSendMessage(100, processKey);
            //autoEvent.WaitOne(1000);
            //return sendResult;

            return SystemNamedPipeMessage.Singleton.ProcessIsRuning(processKey);
        }
    }






    [Serializable]
    internal class SystemMessage
    {
        public int ProcessId { get; set; }
        public string Token { get; set; }
        public string Data { get; set; }
        /// <summary>
        /// 1表示启动消息，100表示判断进程
        /// </summary>
        public int CmdType { get; set; }
        public DateTime Time { get; set; }
    }
}
