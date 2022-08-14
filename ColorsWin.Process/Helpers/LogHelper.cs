using System.Diagnostics;

namespace ColorsWin.Process.Helpers
{
    public class LogHelper
    {
        /// <summary>
        /// 调试错误信息
        /// </summary>
        /// <param name="message"></param>
        public static void Error(string message)
        {
            Trace.WriteLine(message);
        }

        public static void Debug(string message)
        {
            //方式一

            //if (Debugger.IsAttached)
            //{
            //  Console.WriteLine(message);
            //}

            //方式二
            //#if DEBUG
            //  Console.WriteLine(message);
            //#endif

            //Trace.WriteLine(message);

            System.Diagnostics.Debug.WriteLine(message);
        }


        //[Conditional("DEBUG")]
        //public static void WriteLine(string message)
        //{
        //    // Console.WriteLine(message); 
        //}
    }
}
