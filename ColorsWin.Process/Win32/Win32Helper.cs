using ColorsWin.Process.Win32.Struct;
using System;
using System.Runtime.InteropServices;

namespace ColorsWin.Process.Win32
{
    public class Win32Helper
    {
        /// <summary>
        /// 窗体获得焦点
        /// </summary>
        /// <param name="hWnd"></param>
        /// <returns></returns>
        [DllImport("User32.dll")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);


        [DllImport("User32.dll")]
        public static extern bool ShowWindowAsync(IntPtr hWnd, int cmdShow);


        [DllImport("User32.dll")]
        public static extern int SendMessage(int hwnd, int msg, int wParam, ref COPYDATASTRUCT IParam);

        [DllImport("User32.dll")]
        public static extern int FindWindow(string lpClassName, string lpWindowName);
    }
}
