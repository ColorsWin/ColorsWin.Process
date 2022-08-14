using System.Security.AccessControl;
using System.Security.Principal;
using System.Threading;


namespace ColorsWin.Process.Helpers
{
    /// <summary>
    /// 互斥信号量
    /// </summary>
    public class EventWaitHandleHelper
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventName"></param>
        /// <returns></returns>
        public static EventWaitHandle CreateEventHande(string eventName, bool read = false)
        {
            EventWaitHandle handle = null;
            try
            {
                handle = new EventWaitHandle(read, EventResetMode.ManualReset, eventName);
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
            var sid = new SecurityIdentifier(WellKnownSidType.WorldSid, null);
            var rule = new EventWaitHandleAccessRule(sid, EventWaitHandleRights.FullControl, AccessControlType.Allow);
            //var rule = new EventWaitHandleAccessRule("Everyone", EventWaitHandleRights.FullControl, AccessControlType.Allow);
            EventWaitHandleSecurity security = new EventWaitHandleSecurity();
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

        /// <summary>
        /// 打开或者创建
        /// </summary>
        /// <param name="eventName"></param>
        /// <returns></returns>
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
