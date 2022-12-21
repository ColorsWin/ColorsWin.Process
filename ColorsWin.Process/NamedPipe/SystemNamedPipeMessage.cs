using ColorsWin.Process.Helpers;
using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Threading;
using System.Threading.Tasks;

namespace ColorsWin.Process.NamedPipe
{
    class SystemNamedPipeMessage
    {
        public static SystemNamedPipeMessage Singleton { get; } = new SystemNamedPipeMessage();

        public SystemNamedPipeMessage()
        {
            bool service = ProcessMessageConfig.NamePieService;
            Init(service);
        }

        public void Init(bool service)
        {
            if (service)
            {
                Task.Factory.StartNew(() =>
                {
                    RunService();
                });
            }
            else
            {
                pipeClient = new NamedPipeClientStream(".", pipName, PipeDirection.InOut);
            }
        }

        private static int GetProcessId()
        {
            return System.Diagnostics.Process.GetCurrentProcess().Id;
        }

        #region Client


        private static string Token = null;
        private static bool sendResult = false;
        public void OnAcceptData(byte[] data)
        {
            if (data == null)
            {
                return;
            }
            var message = SystemMessageSerializeHelper.Deserialize(data);
            if (message.ProcessId == GetProcessId())
            {
                return;
            }

            LogHelper.Debug("Accept SystemMessage:" + message.Data + message.ProcessId);
            if (message.CmdType == 100)
            {
                bool isExit = ProcessMessageManager.IsExistProcessKey(message.Data);
                if (!isExit)
                {
                    message.Data = null;
                }
                ReplayMessage(message);
            }
        }

        private void ReplayMessage(SystemMessage message)
        {
            message.ProcessId = GetProcessId();
            message.Time = DateTime.Now;
            LogHelper.Debug("Replay:" + message.Data + message.ProcessId);
            System.Threading.Thread.Sleep(50);
            var data = SystemMessageSerializeHelper.Serialize(message);
            SendData(data);
        }

        private NamedPipeClientStream pipeClient;

        private void RunClient()
        {
            bool isRun = true;
            while (isRun)
            {
                bool tempIsString;
                var data = StreamHelper.ReadData(pipeClient, out tempIsString);
                OnAcceptData(data);
                if (!pipeClient.IsConnected)
                {
                    isRun = false;
                    break;
                }
            }
            Console.WriteLine("客户端退出");
        }


        public void Connect(int timeout = 2 * 1000)
        {
            Task.Factory.StartNew(() =>
            {
                try
                {
                    if (!pipeClient.IsConnected)
                    {
                        pipeClient.Connect(timeout);
                    }
                    RunClient();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            });
        }


        public bool SendData(byte[] message)
        {
            if (!pipeClient.IsConnected)
            {
                pipeClient.Connect(10000);
            }
            StreamHelper.WriteData(pipeClient, message, false);
            return true;
        }

        #endregion



        #region Service

        private List<NamedPipeServerStream> serverPool = new List<NamedPipeServerStream>();
        private string pipName = "ColorsWin.PipName";

        protected NamedPipeServerStream CreateNamedPipeServerStream()
        {
            var npss = new NamedPipeServerStream(pipName, PipeDirection.InOut, 100, PipeTransmissionMode.Byte);

            return npss;
        }


        protected void DistroyObject(NamedPipeServerStream npss)
        {
            npss.Close();
            if (serverPool.Contains(npss))
            {
                serverPool.Remove(npss);
            }
            LogHelper.Debug($"Distroy One Server: {pipName}_" + npss.GetHashCode());
        }

        public void RunService()
        {
            while (true)
            {
                var pipeServer = CreateNamedPipeServerStream();
                LogHelper.Debug($"Create One Server:{pipName} " + pipeServer.GetHashCode());

                pipeServer.WaitForConnection();
                serverPool.Add(pipeServer);

                LogHelper.Debug($" One Connection:{pipName} " + pipeServer.GetHashCode());
                Task.Factory.StartNew(RunService);
            }
        }
        #endregion

        private bool SendSystemMessage(SystemMessage message, int millisecondsTimeout = 5 * 100)
        {
            var data = SystemMessageSerializeHelper.Serialize(message);

            bool result = false;

            List<Task> taskList = new List<Task>();
            foreach (var pipeServer in serverPool)
            {
                if (!pipeServer.IsConnected)
                {
                    continue;
                }

                var task1 = Task.Factory.StartNew(() =>
                {
                    return RunTask(message, data, pipeServer);
                });

                taskList.Add(task1);
            }
            Task.WaitAny(taskList.ToArray(), millisecondsTimeout);

            return result;
        }

        private bool RunTask(SystemMessage message, byte[] data, NamedPipeServerStream pipeServer)
        {
            try
            {
                StreamHelper.WriteData(pipeServer, data, false);
                pipeServer.Flush();

                Console.WriteLine("Send Client Done");

                bool tempIsString;
                var replayData = StreamHelper.ReadData(pipeServer, out tempIsString);
                pipeServer.Flush();

                var relpayMessage = SystemMessageSerializeHelper.Deserialize(replayData);
                if (message.CmdType == 100)
                {
                    sendResult = message.Data == relpayMessage.Data;
                    if (sendResult)
                    {
                        autoEvent.Set();
                    }
                }
            }
            catch (Exception ex)
            {
                serverPool.Remove(pipeServer);
                Console.WriteLine(ex.Message);
            }
            return sendResult;
        }


        internal void SystemSendMessage(int cmd, string data)
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
            LogHelper.Debug("SendSystemMessage::" + data + message.ProcessId);
        }

        private static AutoResetEvent autoEvent = new AutoResetEvent(false);

        internal bool ProcessIsRuning(string processKey)
        {
            SystemSendMessage(100, processKey);
            autoEvent.WaitOne(1000);
            return sendResult;
        }
    }
}
