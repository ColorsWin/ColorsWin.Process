﻿using ColorsWin.Process.Helpers;
using ColorsWin.Process.Win32;
using System;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace ColorsWin.Process
{
    /// <summary>
    /// 进程操作类
    /// </summary>
    public class ProcessHelper
    {
        private const int SW_SHOWNOMAL = 1;
        private static MemoryMappedFileObj fileMapped;
        private static string handelTag = "WindowHandel";
        private static Mutex mutex;

        public static bool HadRun()
        {
            string guid = "Global\\";
            guid += Assembly.GetEntryAssembly().GetCustomAttributes(false).OfType<AssemblyProductAttribute>().FirstOrDefault().Product;
            return HadRun(guid);
        }

        /// <summary>
        /// 是否已经在运行
        /// </summary>
        /// <param name="key"></param>
        /// <returns>返回True标示已经在运行改程序</returns>
        public static bool HadRun(string key)
        {
            bool createNew;
            mutex = new System.Threading.Mutex(true, key, out createNew);
            return !createNew;
        }

        /// <summary>
        /// 将当前主窗口 句柄写入到进程内存，方便激活
        /// </summary>
        /// <param name="processKey"></param>
        /// <param name="windowHandel"></param>
        public static void WriteHandel(string processKey, IntPtr windowHandel)
        {
            fileMapped = MemoryMappedFileHelper.CreateMemoryMappedFileObj(processKey + handelTag);
            fileMapped.WriteData(windowHandel.ToString());
        }

        /// <summary>
        /// 是否已经在运行
        /// </summary>
        /// <param name="processKey">唯一标识</param>
        /// <param name="activeWindow">已经运行是否激活之前窗体</param>
        /// <returns></returns>
        public static bool HadRun(string processKey, bool activeWindow = false)
        {
            if (activeWindow)
            {
                var handelStr = MemoryMappedFileHelper.CreateMemoryMappedFileObj(processKey + handelTag).ReadData();
                if (!string.IsNullOrEmpty(handelStr))
                {
                    var handel = (IntPtr)int.Parse(handelStr);
                    Win32Helper.ShowWindowAsync(handel, SW_SHOWNOMAL);
                    Win32Helper.SetForegroundWindow(handel);
                }
            }
            return HadRun(processKey);
        }
    }
}
