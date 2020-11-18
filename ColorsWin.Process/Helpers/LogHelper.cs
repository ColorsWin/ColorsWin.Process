using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace ColorsWin.Process.Helpers
{
    public class LogHelper
    {
        public static void Error(string message)
        {
            Trace.WriteLine(message);
        }

        public static void WriteLine(string message)
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
            Debug.WriteLine(message);
        }


        //[Conditional("DEBUG")]
        //public static void WriteLine(string message)
        //{
        //    // Console.WriteLine(message); 
        //}
    }
}
