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

        ///// <summary>
        ///// 调试错误信息
        ///// </summary>
        ///// <param name="message"></param>
        //public static void Error(string message)
        //{
        //    ProcessMessageConfig.Log?.Error(message);

        //    Trace.WriteLine(message);
        //}

        //public static void Debug(string message)
        //{
        //    //方式一

        //    //if (Debugger.IsAttached)
        //    //{
        //    //  Console.WriteLine(message);
        //    //}

        //    //方式二
        //    //#if DEBUG
        //    //  Console.WriteLine(message);
        //    //#endif

        //    //Trace.WriteLine(message);

        //    System.Diagnostics.Debug.WriteLine(message);
        //}
        //[Conditional("DEBUG")]
        //public static void WriteLine(string message)
        //{
        //    // Console.WriteLine(message); 
        //}
    }
}
