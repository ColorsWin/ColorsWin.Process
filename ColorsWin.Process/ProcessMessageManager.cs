using ColorsWin.Process.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace ColorsWin.Process
{
    public class ProcessMessageManager
    {
        private static Dictionary<string, ProcessMessageProxy> allMessageProxy;

        static ProcessMessageManager()
        {
            Init();
        }

        public static Dictionary<string, ProcessMessageProxy> GetAllMessageProxy()
        {
            return allMessageProxy;
        }

        private static void Init()
        {
            if (allMessageProxy != null)
            {
                return;
            }
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

        public static void CreateProcessKey(string processKey)
        {
            CreateProcessMessageProxy(processKey);
            allMessageProxy[processKey].InitMessageType(false);
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

                    ProcessManager.RemoveToSystem(processKey);
                }
            }
        }

        public static bool IsExistProcessKey(string processKey)
        {
            return allMessageProxy.ContainsKey(processKey);
        }

        public static byte[] ReadData(string processKey)
        {
            CreateProcessMessageProxy(processKey);
            return allMessageProxy[processKey].ReadData();
        }

        public static void AcceptData(string processKey, Action<byte[]> messageAction, bool resetAction = false)
        {
            CreateProcessMessageProxy(processKey);
            allMessageProxy[processKey].ChangeAction(messageAction, resetAction);
            allMessageProxy[processKey].InitReadMessage();
        }

        public static bool SendData(string processKey, byte[] data)
        {
            CreateProcessMessageProxy(processKey);
            return allMessageProxy[processKey].SendData(data);
        }

        public static void AcceptMessage(string processKey, Action<string> messageAction, bool resetAction = false)
        {
            Action<byte[]> actionData = (data) =>
            {
                var buffer = ByteConvertHelper.FormBytes<string>(data);             
                messageAction.Invoke(buffer);
            };
            AcceptData(processKey, actionData, resetAction);
        }

        public static bool SendMessage(string processKey, string message)
        {
            var data = ByteConvertHelper.ToBytes<string>(message);
            return SendData(processKey, data);
        }

        public static string ReadMessage(string processKey)
        {
            var data = ReadData(processKey);
            return ByteConvertHelper.FormBytes<string>(data);
        }

        public static void AcceptData<T>(string processKey, Action<T> messageAction, bool resetAction = false)
        {
            Action<byte[]> action = (data) =>
            {
                try
                {
                    var buffer = ByteConvertHelper.FormBytes<T>(data);
                    messageAction.Invoke(buffer);
                }
                catch (Exception ex)
                {
                    LogHelper.WriteException(ex, "ToByte Convert Error");
                    throw ex;
                }
            };
            AcceptData(processKey, action, resetAction);
        }

        public static bool SendData<T>(string processKey, T data)
        {
            var buffer = ByteConvertHelper.ToBytes(data);
            return SendData(processKey, buffer);
        }

        public static T ReadData<T>(string processKey)
        {
            var data = ReadData(processKey);
            return ByteConvertHelper.FormBytes<T>(data);
        }


        #region Private

        private static void CreateProcessMessageProxy(string processKey)
        {
            if (!allMessageProxy.Keys.Contains(processKey))
            {
                var proxy = new ProcessMessageProxy(processKey);
                allMessageProxy.Add(processKey, proxy);
            }
        }

        #endregion

        #region Default  

        private static string defaultProcessKey = "ColorsWin_eventWaitName";

        public static string ReadMessage()
        {
            return ReadMessage(defaultProcessKey);
        }

        public static void AcceptMessage(Action<string> messageAction, bool resetAction = false)
        {
            AcceptMessage(defaultProcessKey, messageAction, resetAction);
        }

        public static bool SendMessage(string message)
        {
            return SendMessage(defaultProcessKey, message);
        }

        #endregion

        #region Obsolete Methord 

        [Obsolete("Use ProcessMessageConfig.ProcessMessageType", true), EditorBrowsable(EditorBrowsableState.Never)]
        public static void Init(ProcessMessageType processType)
        {
            ProcessMessageConfig.ProcessMessageType = processType;
            //foreach (var item in allMessageProxy)
            //{
            //    item.Value.Reset(processType);
            //}
        }

        [Obsolete("This method is obsolete, please use AcceptMessage"), EditorBrowsable(EditorBrowsableState.Never)]
        public static void ListenMessage(string processKey, Action<string> messageAction, bool resetAction = false)
        {
            AcceptMessage(processKey, messageAction, resetAction);
        }

        [Obsolete("This method is obsolete, please use SendMessage"), EditorBrowsable(EditorBrowsableState.Never)]
        public static bool WriteData(string processKey, string message)
        {
            return SendMessage(processKey, message);
        }


        [Obsolete("This event is obsolete", true), EditorBrowsable(EditorBrowsableState.Never)]
        public static event Action<string, string> AllMessageEvent;

        internal static void OnAcceptMessage(string processKey, string message)
        {
            if (AllMessageEvent != null)
            {
                AllMessageEvent(processKey, message);
            }
        }

        #endregion
    }
}
