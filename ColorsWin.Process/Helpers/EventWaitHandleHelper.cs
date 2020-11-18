using System;
using System.Collections.Generic;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Text;
using System.Threading;


namespace ColorsWin.Process.Helpers
{
    /// <summary>
    /// 互斥信号量
    /// </summary>
    public class EventWaitHandleHelper
    {
        public static EventWaitHandle OpenEventHande(string eventName)
        {
            EventWaitHandle handle = null;
            try
            {
                handle = EventWaitHandle.OpenExisting(eventName);
            }
            catch (WaitHandleCannotBeOpenedException)
            {

            }
            return handle;
        }
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
            catch (WaitHandleCannotBeOpenedException)
            {

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
