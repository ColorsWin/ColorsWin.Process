using ColorsWin.Process.Win32;
using ColorsWin.Process.Win32.Struct;
using System;
using System.Runtime.InteropServices;

namespace ColorsWin.Process.SendMessage
{
    /// <summary>
    /// 消息发送
    /// </summary>
    public class SendMessageHelper
    {
        private const int WM_COPYDATA = 0x004A;
        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="message">消息内容</param>
        /// <param name="handle">窗口句柄</param>
        /// <param name="ownerHandle">当前句柄</param>
        public static void SendMessage(string message, int handle, IntPtr ownerHandle = default(IntPtr))
        {
            var WINDOW_HANDLE = handle;
            if (WINDOW_HANDLE != 0)
            {
                byte[] arr = System.Text.Encoding.Default.GetBytes(message);
                int dataLength = arr.Length;
                COPYDATASTRUCT cdata;
                cdata.dwData = ownerHandle;
                cdata.lpData = Marshal.AllocHGlobal(dataLength);
                cdata.cbData = dataLength;
                Marshal.Copy(arr, 0, cdata.lpData, dataLength);
                Win32Helper.SendMessage(WINDOW_HANDLE, WM_COPYDATA, 0, ref cdata);
            }
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="message">消息内容</param>
        /// <param name="title">窗口标题</param>
        public static void SendMessage(string message, string title)
        {
            int handle = Win32Helper.FindWindow(null, title);
            if (handle != 0)
            {
                SendMessage(message, handle);
            }
        }

        #region WPF用法

        public void WpfListen()
        {
            //var hwnd = new WindowInteropHelper(window).Handle;
            //var source = HwndSource.FromHwnd(hwnd); 
            //if (source != null) source.AddHook(WndProc);

            //new Window().Initialized += (sender, e) =>
            //{
            //    var handle = (new System.Windows.Interop.WindowInteropHelper(sender as Window)).Handle;
            //    System.Windows.Interop.HwndSource.FromHwnd(handle).AddHook(new System.Windows.Interop.HwndSourceHook(WndProc));
            //};
        }

        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if (msg == WM_COPYDATA)
            {
                var cdata = (COPYDATASTRUCT)Marshal.PtrToStructure(lParam, typeof(COPYDATASTRUCT));
                //  var msg2 = Marshal.PtrToStructure<COPYDATASTRUCT>(m.LParam);
                var length = cdata.cbData;
                if (length > 0)
                {
                    byte[] data = new byte[length];
                    Marshal.Copy(cdata.lpData, data, 0, length);
                    string content = System.Text.Encoding.Default.GetString(data);
                }
            }
            return IntPtr.Zero;
        }

        #endregion

        #region WinFrom用此方法 

        /* protected override void DefWndProc(ref Message m)
        {
            switch (m.Msg)
            {
            case WM_COPYDATA:
                COPYDATASTRUCT cdata = new COPYDATASTRUCT();
                Type mytype = cdata.GetType();
                cdata = (COPYDATASTRUCT)m.GetLParam(mytype);
                this.textBox1.Text = cdata.lpData;
                break;
            default:
                base.DefWndProc(ref m);
                break;
            }
        } */

        #endregion
    }
}
