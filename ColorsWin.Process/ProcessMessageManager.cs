using ColorsWin.Process.Ext;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ColorsWin.Process
{
    public class ProcessMessageManager
    {
        private static Dictionary<string, ProcessMessageProxy> allProcessMessage;

        /// <summary>
        /// 收到消息事件 
        /// </summary>
        public static event Action<string, string> AllMessageEvent;


        private static ProcessMessageType processMessageType = ProcessMessageType.ShareMemory;

        static ProcessMessageManager()
        {
            allProcessMessage = new Dictionary<string, ProcessMessageProxy>();
        }

        public static void Init(ProcessMessageType processType)
        {
            processMessageType = processType;
        }

        /// <summary>
        /// 监听进程消息
        /// </summary>
        /// <param name="processKey"></param>
        /// <param name="messageAction"></param>
        /// <param name="resetAction">是否重置接受方法 为true时候之前都不在接受</param>
        public static void ListenMessage(string processKey, Action<string> messageAction, bool resetAction = false)
        {
            InitProcessMessage(processKey, messageAction, resetAction);
            allProcessMessage[processKey].InitListenMessage();
        }

        /// <summary>
        /// 读取进程中数据
        /// </summary>
        /// <returns></returns>
        public static string ReadData(string processKey)
        {
            InitProcessMessage(processKey, null);
            return allProcessMessage[processKey].ReadData();
        }


        /// <summary>
        /// 像进程发送消息
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static bool WriteData(string processKey, string message)
        {
            InitProcessMessage(processKey, null);
            return allProcessMessage[processKey].WriteData(message);
        }


        #region 内部实现

        /// <summary>
        /// 初始化进程信息
        /// </summary>
        /// <param name="processKey"></param>
        /// <param name="read"></param>
        private static void InitProcessMessage(string processKey, Action<string> action, bool resetAction = false)
        {
            if (!allProcessMessage.Keys.Contains(processKey))
            {
                var mainWait = new ProcessMessageProxy(processKey, processMessageType);
                allProcessMessage.Add(processKey, mainWait);
            }
            allProcessMessage[processKey].ChangeAction(action, resetAction);
        }

        /// <summary>
        /// 收到消息事件
        /// </summary>
        /// <param name="message"></param>
        internal static void OnAcceptMessage(string processKey, string message)
        {
            if (AllMessageEvent != null)
            {
                AllMessageEvent(processKey, message);
            }
        }
        #endregion

        /// <summary>
        /// 卸载进程消息
        /// </summary>
        /// <param name="processKey"></param>
        public static void UnInit(string processKey, bool read)
        {
            if (allProcessMessage.Keys.Contains(processKey))
            {
                allProcessMessage[processKey].Stop();
                allProcessMessage.Remove(processKey);
            }
        }

        #region 默认  

        private static string defaultProcessKey = "eventWaitName";
        /// <summary>
        /// 读取进程中数据
        /// </summary>
        /// <returns></returns>
        public static string ReadData()
        {
            return ReadData(defaultProcessKey);
        }

        /// <summary>
        /// 像进程发送消息
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static bool WriteData(string message)
        {
            return WriteData(defaultProcessKey, message);
        }
        #endregion
    }
}
