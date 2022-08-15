using ColorsWin.Process.Helpers;
using System;
using System.IO;
using System.Threading;

namespace ColorsWin.Process
{
    class FileMessage : IProcessMessage
    {
        private string processKey = "fileName";

        public FileMessage(string processName, bool read)
        {
            this.processKey = processName;
            Init(read);
        }

        private string GetFileName()
        {
            return "Global_" + processKey;
        }
        private string tempDataFolederPath = Path.Combine(System.Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "temp");
        private void Init(bool read)
        {
            if (read)
            {
                var watcher = new FileWatcher(OnDataChange);

                if (!Directory.Exists(tempDataFolederPath))
                {
                    Directory.CreateDirectory(tempDataFolederPath);
                }
                watcher.Start(tempDataFolederPath, GetFileName());
            }
        }

        private void OnDataChange(object sender, FileSystemEventArgs e)
        {
            bool isString;
            var data = GetData(out isString);
            if (isString)
            {
                if (AcceptMessage != null)
                {
                    string temp = System.Text.Encoding.Default.GetString(data);
                    AcceptMessage(temp);
                }
            }
            else
            {
                if (AcceptData != null)
                {
                    AcceptData(data);
                }
            }
        }


        private byte[] GetData()
        {
            bool isString;
            var data = GetData(out isString);
            return data;
        }

        private byte[] GetData(out bool isString)
        {
            string fullPath = Path.Combine(tempDataFolederPath, GetFileName());
            if (!File.Exists(fullPath))
            {
                isString = false;
                return null;
            }

            using (var fs = new FileStream(fullPath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                var data = StringStreamHelper.ReadData(fs, out isString);
                return data;
            }
        }

        private bool SetData(byte[] data, bool isString = false)
        {
            Thread.Sleep(ProcessMessageConfig.BatchSendWaitTime);

            string fullPath = Path.Combine(tempDataFolederPath, GetFileName());
            using (var fs = new FileStream(fullPath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                fs.Position = 0;
                StringStreamHelper.WriteData(fs, data, isString);
                return true;
            }
        }

        #region IProcessMessage

        public event Action<string> AcceptMessage;
        public event Action<byte[]> AcceptData;

        public byte[] ReadData()
        {
            return GetData();
        }

        public string ReadMessage()
        {
            var data = ReadData();
            if (data == null)
            {
                return null;
            }
            return System.Text.Encoding.Default.GetString(data);
        }


        public bool SendData(byte[] data)
        {
            return SetData(data);
        }

        public bool SendMessage(string message)
        {
            var data = System.Text.Encoding.Default.GetBytes(message);
            return SetData(data, true);
        }

        #endregion
    }
}
