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


        public static bool IsRuning(string processKey)
        {
            return SystemMessageManager.ProcessIsRuning(processKey);
        }


        public static string RunCMDCommand(params string[] commandLine)
        {
            using (var processs = new System.Diagnostics.Process())
            {
                processs.StartInfo.FileName = "cmd.exe";
                processs.StartInfo.CreateNoWindow = true;
                processs.StartInfo.RedirectStandardError = true;
                processs.StartInfo.RedirectStandardInput = true;
                processs.StartInfo.RedirectStandardOutput = true;
                processs.StartInfo.UseShellExecute = false;
                processs.Start();
                foreach (string com in commandLine)
                {
                    processs.StandardInput.WriteLine(com);
                }
                processs.StandardInput.WriteLine("&exit");

                //如果不执行exit命令，后面调用ReadToEnd()方法会假死
                // 这里使用 & 是批处理命令的符号，表示前面一个命令不管是否执行成功都执行后面(exit)命令，          
                //还有&&和||前者表示必须前一个命令执行成功才会执行后面的命令，后者表示必须前一个命令执行失败才会执行后面的命令

                processs.StandardInput.AutoFlush = true;
                var outPut = processs.StandardOutput.ReadToEnd();
                processs.WaitForExit();
                processs.Close();
                return outPut;
            }
        }
    }
}
