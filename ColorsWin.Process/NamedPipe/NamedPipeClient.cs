
using System;
using System.IO;
using System.IO.Pipes;

namespace ColorsWin.Process.NamedPipe
{
    /// <summary>
    /// 管道客户信息
    /// </summary>
    class NamedPipeClient : IDisposable
    {
        private string serverName;
        private string pipName;
        private NamedPipeClientStream pipeClient;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serverName">服务器地址</param>
        /// <param name="pipName">管道名称</param>
        public NamedPipeClient(string serverName, string pipName)
        {
            this.serverName = serverName;
            this.pipName = pipName;
            pipeClient = new NamedPipeClientStream(serverName, pipName, PipeDirection.InOut);
        }

        public bool SendMessage(string message)
        {
            if (!pipeClient.IsConnected)
            {
                pipeClient.Connect(10000);
            }
            var sw = new StreamWriter(pipeClient);
            sw.WriteLine(message);

            try
            {
                sw.Flush();
            }
            catch (Exception ex)
            {
                Helpers.LogHelper.Error(ex.Message);      
                throw ex;
            }
            if (NamedPipeMessage.Wait)
            {
                //等待服务器响应才能继续发送 
                var streamReader = new StreamReader(pipeClient);
                string returnVal = streamReader.ReadToEnd();
                return returnVal == NamedPipeMessage.ReplyMessageFlat + message;
            }
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
