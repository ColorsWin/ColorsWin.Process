using ColorsWin.Process;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Security.AccessControl;
using System.Threading;
using System.Threading.Tasks;

namespace ProcessDemo
{
    class OtherTest
    {
        private static EventWaitHandle handle;
        public static void Output()
        {
            TestIsRuning();

            // Not Admin  Run Error
            var security = new MemoryMappedFileSecurity();
            security.AddAccessRule(new AccessRule<MemoryMappedFileRights>(
                                                "Everyone",
                                                MemoryMappedFileRights.FullControl,
                                                AccessControlType.Allow));

            var memoryMappedFile = MemoryMappedFile.CreateOrOpen("Global\\SharedMap",
                                        1024,
                                        MemoryMappedFileAccess.ReadWrite,
                                        MemoryMappedFileOptions.DelayAllocatePages,
                                        security,
                                        HandleInheritability.Inheritable);
        }

        private static void TestIsRuning()
        {
            var tempProcessKey = "test111";
            ProcessMessageManager.SendMessage(tempProcessKey, "1234");

            var result = ProcessHelper.IsRuning(tempProcessKey);
            if (result)
            {
                System.Console.WriteLine("Runing");
            }
            else
            {
                System.Console.WriteLine("Not Find");
            }
        }

        private void Output2()
        {

            bool exist = false;

            string user = System.Net.Dns.GetHostName() + "\\Administrator";

            //var rule = new AccessRule<MemoryMappedFileRights>(user, MemoryMappedFileRights.FullControl, AccessControlType.Allow);

            //MemoryMappedFileSecurity fileSecurity = file.GetAccessControl();

            //foreach (AccessRule<MemoryMappedFileRights> accessRules in fileSecurity.GetAccessRules(true, true, typeof(NTAccount)))
            //{
            //    //LogManager.Info("user:" + accessRules.IdentityReference);
            //    //LogManager.Info("right:" + accessRules.Rights);
            //    //LogManager.Info("type:" + accessRules.AccessControlType);

            //    if (accessRules.IdentityReference == rule.IdentityReference && accessRules.AccessControlType == rule.AccessControlType && accessRules.Rights == rule.Rights)
            //    {
            //        exist = true;
            //        break;
            //    }
            //}

            //if (!exist)
            //{
            //    fileSecurity.AddAccessRule(rule);
            //    file.SetAccessControl(fileSecurity);
            //}

        }

    }
}
