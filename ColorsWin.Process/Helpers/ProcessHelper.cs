﻿using ColorsWin.Process.Helpers;
using ColorsWin.Process.Win32;
using System;

namespace ColorsWin.Process
{
  
    public class ProcessHelper
    {
        private const int SW_SHOWNOMAL = 1;
        private static MemoryMappedFileObj fileMapped;
        private static string handlFlat = "WindowHandel";
        private static System.Threading.Mutex mutex;

        public static bool HadRun(string key)
        {
            //string guid = "Global\\";
            //guid += Assembly.GetEntryAssembly().GetCustomAttributes(false).OfType<AssemblyProductAttribute>().FirstOrDefault().Product;

            bool createNew;
            mutex = new System.Threading.Mutex(true, key, out createNew);
            return !createNew;
        }

        public static void WriteHandel(string processKey, IntPtr windowHandel)
        {
            fileMapped = MemoryMappedFileHelper.CreateMemoryMappedFileObj(processKey + handlFlat);
            fileMapped.WriteMessage(windowHandel.ToString());
        }

        public static bool HadRun(string processKey, bool activeWindow = false)
        {
            if (activeWindow)
            {
                var handelStr = MemoryMappedFileHelper.CreateMemoryMappedFileObj(processKey + handlFlat).ReadMessage();
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
