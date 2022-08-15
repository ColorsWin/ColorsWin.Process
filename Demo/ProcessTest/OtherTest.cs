using System;
using System.Collections.Generic;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text;

namespace ProcessDemo
{
    class OtherTest
    {
        public static void Output()
        {
            var user = System.Environment.UserName;

            return;

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

    }
}
