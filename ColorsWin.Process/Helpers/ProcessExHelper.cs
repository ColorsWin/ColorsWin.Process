using ColorsWin.Process.Win32;
using ColorsWin.Process.Win32.Struct;
using System;
using System.Runtime.InteropServices;

namespace ColorsWin.Process.Helpers
{
    /// <summary>
    /// Process的一些扩展
    /// </summary>
    public class ProcessExHelper
    {
        /// <summary>
        ///  模拟父进程打开进程
        /// </summary>
        /// <param name="parentProcessId">父进程id</param>
        /// <param name="binaryPath">子程序路径</param>
        /// <param name="commandLine">调用传递参数</param>
        /// <returns>成功后返回子进程id，失败返回0</returns>
        public static int Run(int parentProcessId, string binaryPath, string commandLine = null)
        {
            // STARTUPINFOEX members
            const int PROC_THREAD_ATTRIBUTE_PARENT_PROCESS = 0x00020000;

            // dwCreationFlags
            const uint EXTENDED_STARTUPINFO_PRESENT = 0x00080000;
            const uint CREATE_NEW_CONSOLE = 0x00000010;

            var pInfo = new PROCESS_INFORMATION();
            var siEx = new STARTUPINFOEX();

            //siEx.StartupInfo.cb = Marshal.SizeOf(siEx);

            var lpSize = IntPtr.Zero;

            Win32Helper.InitializeProcThreadAttributeList(IntPtr.Zero, 1, 0, ref lpSize);
            siEx.lpAttributeList = Marshal.AllocHGlobal(lpSize);
            Win32Helper.InitializeProcThreadAttributeList(siEx.lpAttributeList, 1, 0, ref lpSize);

            IntPtr parentHandle = Win32Helper.OpenProcess(ProcessAccessFlags.CreateProcess | ProcessAccessFlags.DuplicateHandle, false, parentProcessId);

            var lpValueProc = Marshal.AllocHGlobal(IntPtr.Size);
            Marshal.WriteIntPtr(lpValueProc, parentHandle);

            Win32Helper.UpdateProcThreadAttribute(siEx.lpAttributeList, 0, (IntPtr)PROC_THREAD_ATTRIBUTE_PARENT_PROCESS,
                lpValueProc, (IntPtr)IntPtr.Size, IntPtr.Zero, IntPtr.Zero);

            var ps = new SECURITY_ATTRIBUTES();
            var ts = new SECURITY_ATTRIBUTES();
            ps.nLength = Marshal.SizeOf(ps);
            ts.nLength = Marshal.SizeOf(ts);
            bool ret = Win32Helper.CreateProcess(binaryPath, commandLine, ref ps, ref ts, true, EXTENDED_STARTUPINFO_PRESENT | CREATE_NEW_CONSOLE, IntPtr.Zero, null, ref siEx, out pInfo);
            if (ret)
            {
                return pInfo.dwProcessId;
            }
            return 0;
            //var stringPid = pInfo.dwProcessId.ToString();
        }
    }
}
