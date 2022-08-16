
using ColorsWin.Process.Helpers;
using System;
using System.IO.Pipes;

namespace ColorsWin.Process.NamedPipe
{
    class NamedPipeClient : IDisposable
    {
        private string serverName;
        private string pipName;
        private NamedPipeClientStream pipeClient;
        public NamedPipeClient(string serverName, string pipName)
        {
            this.serverName = serverName;
            this.pipName = pipName;
            pipeClient = new NamedPipeClientStream(serverName, pipName, PipeDirection.InOut);
        }

        public bool SendData(byte[] message)
        {
            if (!pipeClient.IsConnected)
            {
                pipeClient.Connect(10000);
            }
            StreamHelper.WriteData(pipeClient, message, false);
            pipeClient.Flush();

            if (NamedPipeMessage.Wait)
            {
                // Wait service send message  then send  next message
                var streamReader = new System.IO.StreamReader(pipeClient);
                string returnVal = streamReader.ReadToEnd(); 
                return returnVal == NamedPipeMessage.ReplyMessageFlat;
            }

            return true;
        }

        public bool SendMessage(string message)
        {
            var data = System.Text.Encoding.UTF8.GetBytes(message);
            if (!pipeClient.IsConnected)
            {
                pipeClient.Connect(10000);
            }
            StreamHelper.WriteData(pipeClient, data, true);
            pipeClient.Flush();
            return true;
        }

        #region IDisposable 成员

        bool _disposed = false;
        public void Dispose()
        {
            if (!_disposed && pipeClient != null)
            {
                pipeClient.Dispose();
                _disposed = true;
            }
        }

        #endregion
    }
}
