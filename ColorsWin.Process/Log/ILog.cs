using System;

namespace ColorsWin.Process
{
    public interface ILog
    {
        void Trace(string message);

        void Debug(string message);

        void Info(string message);

        void Warn(string message);

        void Error(string message);

        void Fatal(string message);

        void WriteException(Exception ex);

        void WriteException(Exception ex, string message); 
    }
}
