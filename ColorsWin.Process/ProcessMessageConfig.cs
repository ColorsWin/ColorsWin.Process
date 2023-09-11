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

        /// <summary>
        /// If it is a window service, the string needs to use "Global\\". 
        /// Note If the current user is not an administrator, the window service needs to be run first, 
        /// and the process key must be created in the window service first, 
        /// because ordinary users do not have permission to create the process key starting with "Global\\"
        /// </summary>
        public static string GlobalTag { get; set; } = "";

        public static int BatchSendWaitTime { get; set; } = 5;

        /// <summary>
        /// Default path C:\temp
        /// </summary>
        public static string FileMessageCachePath { get; set; }


        public static bool EnableLog { get; set; } = true;

        public static ILog Log { get; set; }

        public static Encoding Encoding { get; } = Encoding.Default;

        public static bool UnknowTypeUseSerializable { get; set; } = false;
    }
}
