using System;
using System.Security.AccessControl;
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
                handle = EventWaitHandle.OpenExisting(eventName);
            }
            catch (WaitHandleCannotBeOpenedException)
            {
                doesNotExist = true;
            }
            catch (UnauthorizedAccessException ex)
            {
                Console.WriteLine(eventName + "Unauthorized access: {0}", ex.Message);
                unauthorized = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            if (doesNotExist)
            {
                handle = new EventWaitHandle(!read, EventResetMode.ManualReset, eventName, out wasCreated);
                
                SetAccessControl(handle);

                if (wasCreated)
                {
                    Console.WriteLine("Created the named event." + eventName);
                }                
            }
            else if (unauthorized)
            {
                try
                {
#if NET40
                    handle = EventWaitHandle.OpenExisting(eventName, EventWaitHandleRights.ReadPermissions | EventWaitHandleRights.ChangePermissions);

                    UpdateAccessControl(handle);
                    handle = EventWaitHandle.OpenExisting(eventName);
                    Console.WriteLine("Updated event security.");
#endif
                }
                catch (UnauthorizedAccessException ex)
                {
                    Console.WriteLine("Unable to change permissions: {0}", ex.Message);
                    throw ex;
                }
            }

            return handle;
        }

        private static void UpdateAccessControl(EventWaitHandle handle)
        {
#if NET40            
            EventWaitHandleSecurity ewhSec = handle.GetAccessControl();

            string user = Environment.UserDomainName + "\\" + Environment.UserName;
            user = "Everyone";

            var rule = new EventWaitHandleAccessRule(user, EventWaitHandleRights.Synchronize | EventWaitHandleRights.Modify, AccessControlType.Deny);

            ewhSec.RemoveAccessRule(rule);


            rule = new EventWaitHandleAccessRule(user, EventWaitHandleRights.Synchronize | EventWaitHandleRights.Modify, AccessControlType.Allow);
            ewhSec.AddAccessRule(rule);

            handle.SetAccessControl(ewhSec);
#endif
        }


        private static void SetAccessControl(EventWaitHandle handle)
        {

#if NET40 
            string user = Environment.UserDomainName + "\\" + Environment.UserName;
            user = "Everyone";

            var security = new EventWaitHandleSecurity();

            //var sid = new SecurityIdentifier(WellKnownSidType.WorldSid, null);
            //var rule = new EventWaitHandleAccessRule(sid, EventWaitHandleRights.FullControl, AccessControlType.Allow);         

            var rule = new EventWaitHandleAccessRule(user, EventWaitHandleRights.FullControl, AccessControlType.Allow);
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
