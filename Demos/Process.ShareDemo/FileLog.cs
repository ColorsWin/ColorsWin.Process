using ColorsWin.Process;
using System;
using System.IO;

namespace Process.ShareTest
{
    public class FileLog : ILog
    {
        string filePath;
        public FileLog(string path)
        {
            filePath = path;
        }

        public void Debug(string message)
        {
            using (var write = new StreamWriter(filePath, true))
            {
                write.WriteLine(message);
            }
        }


        public void Error(string message)
        {
            Debug("Error:" + message);
        }

        public void Fatal(string message)
        {
            throw new NotImplementedException();
        }

        public void Info(string message)
        {
            throw new NotImplementedException();
        }

        public void Trace(string message)
        {
            throw new NotImplementedException();
        }

        public void Warn(string message)
        {
            throw new NotImplementedException();
        }

        public void WriteException(Exception ex)
        {
            throw new NotImplementedException();
        }

        public void WriteException(Exception ex, string message)
        {
            throw new NotImplementedException();
        }
    }
}
