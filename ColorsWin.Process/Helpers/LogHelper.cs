using ColorsWin.Process.Log;
using System;

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
            if (!ProcessMessageConfig.EnableLog)
            {
                return;
            }
            ProcessMessageConfig.Log?.Trace(message);
        }

        public static void Debug(string message)
        {
            if (!ProcessMessageConfig.EnableLog)
            {
                return;
            }
            ProcessMessageConfig.Log?.Debug(message);
        }

        public static void Info(string message)
        {
            if (!ProcessMessageConfig.EnableLog)
            {
                return;
            }
            ProcessMessageConfig.Log?.Info(message);
        }

        public static void Warn(string message)
        {
            if (!ProcessMessageConfig.EnableLog)
            {
                return;
            }
            ProcessMessageConfig.Log?.Warn(message);
        }

        public static void Error(string message)
        {
            if (!ProcessMessageConfig.EnableLog)
            {
                return;
            }
            ProcessMessageConfig.Log?.Error(message);
        }

        public static void Fatal(string message)
        {
            ProcessMessageConfig.Log?.Fatal(message);
        }

        public static void WriteException(Exception ex)
        {
            if (!ProcessMessageConfig.EnableLog)
            {
                return;
            }
            ProcessMessageConfig.Log?.WriteException(ex);
        }

        public static void WriteException(Exception ex, string message)
        {
            if (!ProcessMessageConfig.EnableLog)
            {
                return;
            }
            ProcessMessageConfig.Log?.WriteException(ex, message);
        }
    }
}
