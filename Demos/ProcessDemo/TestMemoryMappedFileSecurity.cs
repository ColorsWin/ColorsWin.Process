using ColorsWin.Process;
using System;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;

namespace ProcessDemo
{
    class TestMemoryMappedFileSecurity
    {

        public static void Output()
        {
            //TestIsRuning();

            // Not Admin  Run Error
            //var security = new MemoryMappedFileSecurity();
            //security.AddAccessRule(new AccessRule<MemoryMappedFileRights>(
            //                                    "Everyone",
            //                                    MemoryMappedFileRights.FullControl,
            //                                    AccessControlType.Allow));

            //var memoryMappedFile = MemoryMappedFile.CreateOrOpen("Global\\SharedMap",
            //                            1024,
            //                            MemoryMappedFileAccess.ReadWrite,
            //                            MemoryMappedFileOptions.DelayAllocatePages,
            //                            security,
            //                            HandleInheritability.Inheritable);


            MemoryMappedFile handle = null;
            bool doesNotExist = false;
            bool unauthorized = false;

            string eventName = "Global\\SharedMap";
            eventName = "SharedMap";

            try
            {
                handle = MemoryMappedFile.OpenExisting(eventName);
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine("Named event does not exist.");
                doesNotExist = true;
            }
            catch (UnauthorizedAccessException ex)
            {
                Console.WriteLine("Unauthorized access: {0}", ex.Message);
                unauthorized = true;
            }


            if (doesNotExist)
            {
                long capacity = 1024 * 1024;

                bool isAdmmin = ProcessHelper.IsRunAsAdmin();
                //if (isAdmmin)
                //{
                //    eventName = "Global\\" + eventName;
                //}

                if (isAdmmin)
                {
                    var memoryMapSecurity = new MemoryMappedFileSecurity();
                    string user = Environment.UserDomainName + "\\" + Environment.UserName;
                    //user = "Everyone";
                    memoryMapSecurity.AddAccessRule(new AccessRule<MemoryMappedFileRights>(user, MemoryMappedFileRights.ReadWrite, AccessControlType.Allow));
                    handle = MemoryMappedFile.CreateNew(eventName, capacity, MemoryMappedFileAccess.ReadWrite, MemoryMappedFileOptions.None, memoryMapSecurity, HandleInheritability.None);
                }
                else
                {
                    try
                    {
                        //非管理员用户 创建Global\开头的会异常 没有权限
                        handle = MemoryMappedFile.CreateNew(eventName, 1024 * 1024);
                    }
                    catch (UnauthorizedAccessException ex)
                    {
                        Console.WriteLine("no permissions to create: {0},{1}", eventName, ex.Message);
                        throw ex;
                    }
                }

                Console.WriteLine("Created the named event.");

            }
            else if (unauthorized)
            {
                try
                {
                    handle = MemoryMappedFile.OpenExisting(eventName, MemoryMappedFileRights.ReadPermissions | MemoryMappedFileRights.ChangePermissions);

                    var ewhSec = handle.GetAccessControl();
                    string user = Environment.UserDomainName + "\\" + Environment.UserName;

                    //var rule = new AccessRule<MemoryMappedFileRights>(sid, MemoryMappedFileRights.FullControl, AccessControlType.Allow);
                    //var sid = new SecurityIdentifier(WellKnownSidType.WorldSid, null);

                    var rule = new AccessRule<MemoryMappedFileRights>(user, MemoryMappedFileRights.FullControl, AccessControlType.Allow);
                    ewhSec.RemoveAccessRule(rule);


                    //rule = new AccessRule<MemoryMappedFileRights>(user, MemoryMappedFileRights. | EventWaitHandleRights.Modify, AccessControlType.Allow);

                    ewhSec.AddAccessRule(rule);
                    handle.SetAccessControl(ewhSec);

                    Console.WriteLine("Updated event security.");

                    handle = MemoryMappedFile.OpenExisting(eventName);
                }
                catch (UnauthorizedAccessException ex)
                {
                    Console.WriteLine("Unable to change permissions: {0}", ex.Message);
                    throw ex;
                }
            }

            var stream = handle.CreateViewStream();
            if (doesNotExist)
            {
                BinaryWriter writer = new BinaryWriter(stream);
                writer.Write(108);

                Console.WriteLine("我创建 写入108");
            }
            else
            {
                BinaryReader reader = new BinaryReader(stream);
                var data = reader.ReadInt32();
                Console.WriteLine("获取数据：" + data);


                Console.WriteLine("写入新数据98");
                BinaryWriter writer = new BinaryWriter(stream);
                writer.Write(98);
            }
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
