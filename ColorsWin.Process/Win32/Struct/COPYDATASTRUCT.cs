using System;
using System.Runtime.InteropServices;

namespace ColorsWin.Process.Win32.Struct
{
    public struct COPYDATASTRUCT
    {
        /// <summary>
        ///  Any value the sender chooses.  Perhaps its main window handle?
        /// </summary>
        public IntPtr dwData;
        /// <summary>
        /// The count of bytes in the message.
        /// </summary>
        public int cbData;
        /// <summary>
        /// The address of the message.
        /// </summary>
        public IntPtr lpData;

        ///下面定义容易乱码
        //[MarshalAs(UnmanagedType.LPStr)]
        //public string lpData;
    }
}
