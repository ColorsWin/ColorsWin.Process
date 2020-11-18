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
        private Action<string> actionMessage;
        public NamedPipeListenServer(string pipName, Action<string> actionMessage = null)
        {
            this.pipName = pipName;
            this.actionMessage = actionMessage;
        }

        /// <summary>
        /// 创建一个NamedPipeServerStream
        /// </summary>
        /// <returns></returns>
        protected NamedPipeServerStream CreateNamedPipeServerStream()
        {
            var npss = new NamedPipeServerStream(pipName, PipeDirection.InOut, 10);
            serverPool.Add(npss);
            LogHelper.WriteLine("启动了一个NamedPipeServerStream " + npss.GetHashCode());
            return npss;
        }

        /// <summary>
        /// 销毁
        /// </summary>
        /// <param name="npss"></param>
        protected void DistroyObject(NamedPipeServerStream npss)
        {
            npss.Close();
            if (serverPool.Contains(npss))
            {
                serverPool.Remove(npss);
            }
            LogHelper.WriteLine("销毁一个NamedPipeServerStream " + npss.GetHashCode());
        }

        public void Run()
        {
            using (var pipeServer = CreateNamedPipeServerStream())
            {
                pipeServer.WaitForConnection();

                LogHelper.WriteLine("建立一个连接 " + pipeServer.GetHashCode());

                Task.Factory.StartNew(Run);//new Action(Run).BeginInvoke(null, null);//dotnetCore支持有问题
                try
                {
                    bool isRun = true;
                    while (isRun)
                    {
                        var sr = new StreamReader(pipeServer);
                        var message = sr.ReadLine();
                        if (actionMessage != null)
                        {
                            actionMessage(message);
                        }
                        if (NamedPipeMessage.Wait)
                        {
                            ProcessMessage(message, pipeServer);
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
                    LogHelper.WriteLine($"ERROR: {e.Message}");
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
        protected virtual void ProcessMessage(string message, NamedPipeServerStream pipeServer)
        {
            using (var sw = new StreamWriter(pipeServer))
            {
                sw.AutoFlush = true;
                sw.Write(NamedPipeMessage.ReplyMessageFlat + message);
            }
        }

        /// <summary>
        /// 停止
        /// </summary>
        public void Stop()
        {
            for (int i = 0; i < serverPool.Count; i++)
            {
                var item = serverPool[i];
                DistroyObject(item);
            }
        }
    }
}
