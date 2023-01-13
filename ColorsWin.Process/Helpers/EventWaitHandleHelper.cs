using System;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Threading;

namespace ColorsWin.Process.Helpers
{
    public class EventWaitHandleHelper
    {
        public static EventWaitHandle CreateEventHande(string eventName, bool read = false)
        {
            EventWaitHandle handle = null;
            bool doesNotExist = false;
            bool unauthorized = false;
            bool wasCreated;

            try
            {
#if NET40
                try
                {
                    //handle = new EventWaitHandle(!read, EventResetMode.ManualReset, eventName);
                    handle = EventWaitHandle.OpenExisting(eventName);
                    //SetAccessControl(handle);
                }
                catch (WaitHandleCannotBeOpenedException)
                {
                    Console.WriteLine(eventName + "Named event does not exist.");
                    doesNotExist = true;
                }
                catch (UnauthorizedAccessException ex)
                {
                    Console.WriteLine(eventName + "Unauthorized access: {0}", ex.Message);
                    unauthorized = true;
                }
                catch (Exception ex)
                {
                    bool isAdmmin = ProcessHelper.IsRunAsAdmin();
                    var tag = ProcessMessageConfig.GlobalTag;
                    if (!isAdmmin && eventName.StartsWith(tag))
                    {
                        eventName = eventName.Replace(tag, "");
                    }
                }
                if (doesNotExist)
                {
                    // The event does not exist, so create it.

                    // Create an access control list (ACL) that denies the
                    // current user the right to wait on or signal the 
                    // event, but allows the right to read and change
                    // security information for the event.
                    //
                    string user = Environment.UserDomainName + "\\" + Environment.UserName;
                    user = "Everyone";
                    EventWaitHandleSecurity ewhSec =
                        new EventWaitHandleSecurity();

                    EventWaitHandleAccessRule rule =
                        new EventWaitHandleAccessRule(user,
                            EventWaitHandleRights.Synchronize |
                            EventWaitHandleRights.Modify,
                            AccessControlType.Deny);
                    ewhSec.AddAccessRule(rule);

                    rule = new EventWaitHandleAccessRule(user,
                        EventWaitHandleRights.ReadPermissions |
                        EventWaitHandleRights.ChangePermissions,
                        AccessControlType.Allow);
                    ewhSec.AddAccessRule(rule);

                    // Create an EventWaitHandle object that represents
                    // the system event named by the constant 'ewhName', 
                    // initially signaled, with automatic reset, and with
                    // the specified security access. The Boolean value that 
                    // indicates creation of the underlying system object
                    // is placed in wasCreated.
                    //
                    handle = new EventWaitHandle(!read, EventResetMode.AutoReset, eventName, out wasCreated, ewhSec);

                    // If the named system event was created, it can be
                    // used by the current instance of this program, even 
                    // though the current user is denied access. The current
                    // program owns the event. Otherwise, exit the program.
                    // 
                    if (wasCreated)
                    {
                        Console.WriteLine("Created the named event.");
                    }
                    else
                    {
                        Console.WriteLine("Unable to create the event.");
                        return null;
                    }
                }
                else if (unauthorized)
                {
                    // Open the event to read and change the access control
                    // security. The access control security defined above
                    // allows the current user to do this.
                    //
                    try
                    {
                        handle = EventWaitHandle.OpenExisting(eventName,
                            EventWaitHandleRights.ReadPermissions |
                            EventWaitHandleRights.ChangePermissions);

                        // Get the current ACL. This requires 
                        // EventWaitHandleRights.ReadPermissions.
                        EventWaitHandleSecurity ewhSec = handle.GetAccessControl();

                        string user = Environment.UserDomainName + "\\" + Environment.UserName;
                        user = "Everyone";
                        // First, the rule that denied the current user 
                        // the right to enter and release the event must
                        // be removed.
                        EventWaitHandleAccessRule rule =
                            new EventWaitHandleAccessRule(user,
                                EventWaitHandleRights.Synchronize |
                                EventWaitHandleRights.Modify,
                                AccessControlType.Deny);
                        ewhSec.RemoveAccessRule(rule);

                        // Now grant the user the correct rights.
                        // 
                        rule = new EventWaitHandleAccessRule(user,
                            EventWaitHandleRights.Synchronize |
                            EventWaitHandleRights.Modify,
                            AccessControlType.Allow);
                        ewhSec.AddAccessRule(rule);

                        // Update the ACL. This requires
                        // EventWaitHandleRights.ChangePermissions.
                        handle.SetAccessControl(ewhSec);

                        Console.WriteLine("Updated event security.");

                        // Open the event with (EventWaitHandleRights.Synchronize 
                        // | EventWaitHandleRights.Modify), the rights required
                        // to wait on and signal the event.
                        //
                        handle = EventWaitHandle.OpenExisting(eventName);
                    }
                    catch (UnauthorizedAccessException ex)
                    {
                        Console.WriteLine("Unable to change permissions: {0}", ex.Message);
                        return null;
                    }
                }

                //if (handle == null)
                //{
                //    handle = new EventWaitHandle(!read, EventResetMode.ManualReset, eventName);
                //}

#endif
            }
            catch (WaitHandleCannotBeOpenedException ex)
            {
                throw ex;
            }
            return handle;
        }

        private static void SetAccessControl(EventWaitHandle handle)
        {
#if NET40
            //var sid = new SecurityIdentifier(WellKnownSidType.WorldSid, null);
            //var rule = new EventWaitHandleAccessRule(sid, EventWaitHandleRights.ReadPermissions, AccessControlType.Allow);
            //var rule = new EventWaitHandleAccessRule("Everyone", EventWaitHandleRights.ReadPermissions, AccessControlType.Deny);
            //EventWaitHandleSecurity security = new EventWaitHandleSecurity();
            //security.AddAccessRule(rule);
            //handle.SetAccessControl(security);


            var users = new SecurityIdentifier(WellKnownSidType.BuiltinUsersSid, null);
            var rule = new EventWaitHandleAccessRule(users, EventWaitHandleRights.ReadPermissions,
                                      AccessControlType.Allow);
            var security = new EventWaitHandleSecurity();
            security.AddAccessRule(rule);
            handle.SetAccessControl(security);
#endif
        }

        public static EventWaitHandle OpenEventHande(string eventName)
        {
            EventWaitHandle handle = null;
            try
            {
                handle = EventWaitHandle.OpenExisting(eventName);
            }
            catch (WaitHandleCannotBeOpenedException ex)
            {
                //LogHelper.Error();
                throw ex;
            }
            return handle;
        }


        public static EventWaitHandle OpenOrCreateEventHande(string eventName)
        {
            var handle = OpenEventHande(eventName);
            if (handle == null)
            {
                handle = CreateEventHande(eventName);
            }
            return handle;
        }
    }
}
