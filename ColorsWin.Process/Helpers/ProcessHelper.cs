using ColorsWin.Process.Helpers;
using ColorsWin.Process.Win32;
using System;
using System.Security.Principal;

namespace ColorsWin.Process
{
    public class ProcessHelper
    {
        private const int SW_SHOWNOMAL = 1;
        private static MemoryMappedFileObj fileMapped;
        private static string handlFlat = "WindowHandel";
        private static System.Threading.Mutex mutex;

        public static bool IsRunAsAdmin()
        {
            var id = WindowsIdentity.GetCurrent();
            var principal = new WindowsPrincipal(id);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }

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
        public static bool HadRunEx(string processKey, bool activeWindow = false)
        {
            var processHandle = ProcessMessageManager.ReadMessage(processKey);
            bool handRun = !string.IsNullOrEmpty(processHandle);
            if (activeWindow && handRun)
            {
                ProcessMessageManager.SendMessage(processKey, "NormalWindow");
            }
            return handRun;
        }
    }
}
