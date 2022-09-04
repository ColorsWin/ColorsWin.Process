using System;
using System.IO;
using System.Text;

namespace ColorsWin.Process
{
    public class ProcessMessageConfig
    {
        static ProcessMessageConfig()
        {
            var basePath = Path.GetPathRoot(System.Environment.GetFolderPath(Environment.SpecialFolder.System));
            FileMessageCachePath = Path.Combine(basePath, "temp");
        }

        public static long MemoryCapacity { get; set; } = 10 * 1024 * 1024;

        public static ProcessMessageType ProcessMessageType { get; set; } = ProcessMessageType.ShareMemory;

        internal static string GlobalTag = "Global\\";

        public static int BatchSendWaitTime = 5;

        /// <summary>
        /// Default path C:\temp
        /// </summary>
        public static string FileMessageCachePath { get; set; }

        public static ILog Log { get; set; }

        public static Encoding Encoding { get; } = Encoding.Default;

        public static bool NamePieService { get; set; } = false;
    }
}
