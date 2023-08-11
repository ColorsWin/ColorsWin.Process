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

        private string tempDataFolederPath = ProcessMessageConfig.FileMessageCachePath;

        private void Init(bool read)
        {
            if (!Directory.Exists(tempDataFolederPath))
            {
                Directory.CreateDirectory(tempDataFolederPath);
            }
            if (read)
            {
                var watcher = new FileWatcher(OnDataChange);
                watcher.Start(tempDataFolederPath, GetFileName());
            }
            else
            {
                bool isAdmmin = ProcessHelper.IsRunAsAdmin();
                if (isAdmmin)
                {
                    if (!Directory.Exists(tempDataFolederPath))
                    {
                        Directory.CreateDirectory(tempDataFolederPath);
                    }

                    SecurityHelper.AddSecurity(tempDataFolederPath);
                }
            }
        }

        private void OnDataChange(object sender, FileSystemEventArgs e)
        {
            var data = GetData();
            if (data == null)
            {
                return;
            }
            if (AcceptData != null)
            {
                AcceptData(data);
            }
        }


        private byte[] GetData()
        {
            string fullPath = Path.Combine(tempDataFolederPath, GetFileName());
            if (!File.Exists(fullPath))
            {
                return null;
            }

            using (var fs = new FileStream(fullPath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                var data = StreamHelper.ReadData(fs);
                return data;
            }
        }

        private bool SetData(byte[] data)
        {
            Thread.Sleep(ProcessMessageConfig.BatchSendWaitTime);

            string fullPath = Path.Combine(tempDataFolederPath, GetFileName());
            using (var fs = new FileStream(fullPath, FileMode.OpenOrCreate, FileAccess.Write))
            {
                fs.Position = 0;
                StreamHelper.WriteData(fs, data);
                return true;
            }
        }

        #region IProcessMessage 

        public event Action<byte[]> AcceptData;

        public byte[] ReadData()
        {
            return GetData();
        }

        public bool SendData(byte[] data)
        {
            return SetData(data);
        }

        #endregion
    }
}
