using System;

namespace ColorsWin.Process.Log
{
    class DefaultLog : ILog
    {
        public void Debug(string message)
        {
            Console.WriteLine(message);
            System.Diagnostics.Trace.WriteLine(message);
        }

        public void Error(string message)
        {
            System.Diagnostics.Trace.TraceError(message);
        }

        public void Fatal(string message)
        {
            System.Diagnostics.Trace.Fail(message);
        }

        public void Info(string message)
        {
            System.Diagnostics.Trace.TraceInformation(message);
        }

        public void Trace(string message)
        {
            throw new NotImplementedException();
        }

        public void Warn(string message)
        {
            System.Diagnostics.Trace.TraceWarning(message);
        }

        public void WriteException(Exception ex)
        {
            System.Diagnostics.Trace.TraceError(ex.Message);
        }

        public void WriteException(Exception ex, string message)
        {
            System.Diagnostics.Trace.TraceError(message + ";" + ex.Message);
        }
    }
}
