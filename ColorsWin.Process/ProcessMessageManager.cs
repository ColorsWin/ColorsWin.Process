using System;
using System.Collections.Generic;
using System.Linq;

namespace ColorsWin.Process
{
    public class ProcessMessageManager
    {
        private static Dictionary<string, ProcessMessageProxy> allMessageProxy;
        public static event Action<string, string> AllMessageEvent;

        static ProcessMessageManager()
        {
            allMessageProxy = new Dictionary<string, ProcessMessageProxy>();
        }

        public static ProcessMessageType GetProcessMessageType(string processKey)
        {
            if (allMessageProxy.ContainsKey(processKey))
            {
                return allMessageProxy[processKey].GetProcessMessageType();
            }
            return ProcessMessageType.None;
        }

        public static void AcceptMessage(string processKey, Action<string> messageAction, bool resetAction = false)
        {
            InitProcessMessage(processKey, messageAction, resetAction);
            allMessageProxy[processKey].InitMessage();
        }

        public static void AcceptData(string processKey, Action<byte[]> messageAction, bool resetAction = false)
        {
            InitProcessMessage(processKey, messageAction, resetAction);
            allMessageProxy[processKey].InitMessage();
        }

        public static bool SendMessage(string processKey, string message)
        {
            InitProcessMessage(processKey);
            return allMessageProxy[processKey].SendMessage(message);
        }

        public static byte[] ReadData(string processKey)
        {
            InitProcessMessage(processKey);
            return allMessageProxy[processKey].ReadData();
        }

        public static string ReadMessage(string processKey)
        {
            InitProcessMessage(processKey);
            return allMessageProxy[processKey].ReadMessage();
        }

        public static bool SendData(string processKey, byte[] data)
        {
            InitProcessMessage(processKey);
            return allMessageProxy[processKey].SendData(data);
        }

        public static void InitMessageType(string processKey, ProcessMessageType processMessageType = ProcessMessageType.ShareMemory)
        {
            if (!allMessageProxy.Keys.Contains(processKey))
            {
                var proxy = new ProcessMessageProxy(processKey, processMessageType);
                allMessageProxy.Add(processKey, proxy);
            }
            else
            {
                allMessageProxy[processKey].Reset(processMessageType);
            }
        }

        public static void UnInit(string processKey, ProxyType type = ProxyType.All)
        {
            if (allMessageProxy.Keys.Contains(processKey))
            {
                allMessageProxy[processKey].Stop(type);
                var remove = type == ProxyType.All || type == allMessageProxy[processKey].GetProxyType();
                if (remove)
                {
                    allMessageProxy.Remove(processKey);
                }
            }
        }

        #region Private

        private static void InitProcessMessage(string processKey)
        {
            if (!allMessageProxy.Keys.Contains(processKey))
            {
                var proxy = new ProcessMessageProxy(processKey);
                allMessageProxy.Add(processKey, proxy);
            }
        }

        private static void InitProcessMessage(string processKey, Action<string> action, bool resetAction = false)
        {
            InitProcessMessage(processKey);
            allMessageProxy[processKey].ChangeAction(action, resetAction);
        }

        private static void InitProcessMessage(string processKey, Action<byte[]> action, bool resetAction = false)
        {
            InitProcessMessage(processKey);
            allMessageProxy[processKey].ChangeAction(action, resetAction);
        }

        internal static void OnAcceptMessage(string processKey, string message)
        {
            if (AllMessageEvent != null)
            {
                AllMessageEvent(processKey, message);
            }
        }

        #endregion 

        #region Default  

        private static string defaultProcessKey = "ColorsWin_eventWaitName";

        public static string ReadMessage()
        {
            return ReadMessage(defaultProcessKey);
        }

        public static bool SendMessage(string message)
        {
            return SendMessage(defaultProcessKey, message);
        }

        #endregion

    }
}
