using ColorsWin.Process.Log;
using System;
using System.Diagnostics;

namespace ColorsWin.Process.Helpers
{
    public class LogHelper
    {
        static LogHelper()
        {
            if (ProcessMessageConfig.Log == null)
            {
                ProcessMessageConfig.Log = new DefaultLog();
            }
        }

        public static void Trace(string message)
        {
            ProcessMessageConfig.Log?.Trace(message);
        }

        public static void Debug(string message)
        {
            ProcessMessageConfig.Log?.Debug(message);
        }

        public static void Info(string message)
        {
            ProcessMessageConfig.Log?.Info(message);
        }

        public static void Warn(string message)
        {
            ProcessMessageConfig.Log?.Warn(message);
        }

        public static void Error(string message)
        {
            ProcessMessageConfig.Log?.Error(message);
        }

        public static void Fatal(string message)
        {
            ProcessMessageConfig.Log?.Fatal(message);
        }

        public static void WriteException(Exception ex)
        {
            ProcessMessageConfig.Log?.WriteException(ex);
        }

        public static void WriteException(Exception ex, string message)
        {
            ProcessMessageConfig.Log?.WriteException(ex, message);
        }
    }
}
