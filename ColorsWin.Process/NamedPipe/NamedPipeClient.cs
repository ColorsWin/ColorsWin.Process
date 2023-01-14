
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
            var data = ProcessMessageConfig.Encoding.GetBytes(message);
            if (!pipeClient.IsConnected)
            {
                pipeClient.Connect(10 * 1000);
            }
            StreamHelper.WriteData(pipeClient, data, true);
            pipeClient.Flush();
            return true;
        }

        public void Connect(int timeout = 2 * 1000)
        {
            if (!pipeClient.IsConnected)
            {
                pipeClient.Connect(timeout);
            }
        }

        #region IDisposable  

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
