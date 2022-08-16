using ColorsWin.Process.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Threading.Tasks;

namespace ColorsWin.Process.NamedPipe
{
    class NamedPipeListenServer
    {
        private List<NamedPipeServerStream> serverPool = new List<NamedPipeServerStream>();
        private string pipName = "ColorsWin.PipName";
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
            LogHelper.Debug("Create One NamedPipeServerStream:" + npss.GetHashCode());
            return npss;
        }


        protected void DistroyObject(NamedPipeServerStream npss)
        {
            npss.Close();
            if (serverPool.Contains(npss))
            {
                serverPool.Remove(npss);
            }
            LogHelper.Debug("Distroy One NamedPipeServerStream: " + npss.GetHashCode());
        }

        public void Run()
        {
            using (var pipeServer = CreateNamedPipeServerStream())
            {
                pipeServer.WaitForConnection();

                LogHelper.Debug("Create One Connection" + pipeServer.GetHashCode());

                //new Action(Run).BeginInvoke(null, null);//dotnetCore Run Error

                Task.Factory.StartNew(Run);

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
                pipeServer.Write(data, 0, data.Length);
                //using (var streamWriter = new StreamWriter(pipeServer))
                //{
                //    streamWriter.AutoFlush = true;
                //    streamWriter.Write(message);
                //}
            }

        }
    }
}
