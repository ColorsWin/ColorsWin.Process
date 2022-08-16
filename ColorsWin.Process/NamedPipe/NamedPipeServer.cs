using ColorsWin.Process.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Threading.Tasks;

namespace ColorsWin.Process.NamedPipe
{
    /// <summary>
    /// 管道服务信息
    /// </summary>
    class NamedPipeListenServer
    {
        private List<NamedPipeServerStream> serverPool = new List<NamedPipeServerStream>();
        private string pipName = "ColorsWinpipName";
        private Action<byte[]> actionData;
        private Action<string> actionMessage;
        public NamedPipeListenServer(string pipName, Action<byte[]> actionData, Action<string> actionMessage)
        {
            this.pipName = pipName;
            this.actionData = actionData;
            this.actionMessage = actionMessage;
        }


        protected NamedPipeServerStream CreateNamedPipeServerStream()
        {
            var npss = new NamedPipeServerStream(pipName, PipeDirection.InOut, 10);
            serverPool.Add(npss);
            LogHelper.Debug("创建一个NamedPipeServerStream " + npss.GetHashCode());
            return npss;
        }


        protected void DistroyObject(NamedPipeServerStream npss)
        {
            npss.Close();
            if (serverPool.Contains(npss))
            {
                serverPool.Remove(npss);
            }
            LogHelper.Debug("销毁一个NamedPipeServerStream " + npss.GetHashCode());
        }

        public void Run()
        {
            using (var pipeServer = CreateNamedPipeServerStream())
            {
                pipeServer.WaitForConnection();

                LogHelper.Debug("建立一个连接" + pipeServer.GetHashCode());

                Task.Factory.StartNew(Run);//new Action(Run).BeginInvoke(null, null);//dotnetCore支持有问题
                try
                {
                    bool isRun = true;
                    while (isRun)
                    {
                        bool tempIsString;

                        var data = StringStreamHelper.ReadData(pipeServer, out tempIsString);
                        if (tempIsString)
                        {
                            if (actionMessage != null)
                            {
                                var message = System.Text.Encoding.UTF8.GetString(data);
                                actionMessage(message);
                            }
                        }
                        else
                        {
                            if (actionData != null)
                            {
                                actionData(data);
                            }
                        }

                        if (NamedPipeMessage.Wait)
                        {
                            //ReplyMessageMessage(messages, pipeServer);
                        }

                        if (!pipeServer.IsConnected)
                        {
                            isRun = false;
                            break;
                        }
                    }
                }
                catch (IOException e)
                {
                    LogHelper.Debug($"ERROR: {e.Message}");
                }
                finally
                {
                    DistroyObject(pipeServer);
                }
            }
        }


        /// <summary>
        /// 处理消息【返回接受成功，客户端才能继续发送】
        /// </summary>
        /// <param name="message"></param>
        /// <param name="pipeServer"></param>
        protected virtual void ReplyMessageMessage(string[] message, NamedPipeServerStream pipeServer)
        {
            using (var streamWriter = new StreamWriter(pipeServer))
            {
                streamWriter.AutoFlush = true;
                streamWriter.Write(NamedPipeMessage.ReplyMessageFlat);
            }
        }


        public void Stop()
        {
            for (int i = 0; i < serverPool.Count; i++)
            {
                var item = serverPool[i];
                DistroyObject(item);
            }
        }

        public void SendMessage(string message)
        {
            foreach (var pipeServer in serverPool)
            {
                if (!pipeServer.IsConnected)
                {
                    continue;
                }
                var data = System.Text.Encoding.UTF8.GetBytes(message);
                pipeServer.Write(data,0,data.Length);
                //using (var streamWriter = new StreamWriter(pipeServer))
                //{
                //    streamWriter.AutoFlush = true;
                //    streamWriter.Write(message);
                //}
            }

        }
    }
}
