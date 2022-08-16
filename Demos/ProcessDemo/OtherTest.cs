using System;
using System.Collections.Generic;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Security.AccessControl;
using System.Text;

namespace ProcessDemo
{
    class OtherTest
    {
        public static void Output()
        {
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
