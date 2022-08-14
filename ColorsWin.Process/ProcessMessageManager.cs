using ColorsWin.Process.Ext;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ColorsWin.Process
{
    /// <summary>
    /// 进程消息管理
    /// </summary>
    public class ProcessMessageManager
    {
        private static Dictionary<string, ProcessMessageProxy> allProcessMessage;

        /// <summary>
        /// 收到所有进程消息事件 
        /// </summary>
        public static event Action<string, string> AllMessageEvent;


        static ProcessMessageManager()
        {
            allProcessMessage = new Dictionary<string, ProcessMessageProxy>();
        }


        /// <summary>
        /// 接受进程消息
        /// </summary>
        /// <param name="processKey">要监听的进程的key唯一值</param>
        /// <param name="messageAction">收到消息后处理方法</param>
        /// <param name="resetAction">是否重置接受方法 为true时候之前都不在接受</param>
        public static void AcceptMessage(string processKey, Action<string> messageAction, bool resetAction = false)
        {
            InitProcessMessage(processKey, messageAction, resetAction);
            allProcessMessage[processKey].InitListenMessage();
        }

        /// <summary>
        /// 接受进程消息 多个参数
        /// </summary>
        /// <param name="processKey">要监听的进程的key唯一值</param>
        /// <param name="messageAction">收到消息后处理方法</param>
        /// <param name="resetAction">是否重置接受方法 为true时候之前都不在接受</param>
        public static void AcceptData(string processKey, Action<byte[]> messageAction, bool resetAction = false)
        {
            InitProcessMessage(processKey, messageAction, resetAction);
            allProcessMessage[processKey].InitListenMessage();
        }


        /// <summary>
        /// 向指定进程发送消息
        /// </summary>
        /// <param name="message">发送内容</param>
        /// <returns></returns>
        public static bool SendMessage(string processKey, string message)
        {
            InitProcessMessage(processKey);
            return allProcessMessage[processKey].SendMessage(message);
        }


        /// <summary>
        /// 获取指定进程中数据 
        /// </summary>
        /// <param name="processKey">进程唯一标识</param>
        /// <returns></returns>
        public static byte[] ReadData(string processKey)
        {
            InitProcessMessage(processKey);
            return allProcessMessage[processKey].ReadData();
        }

        /// <summary>
        /// 获取指定进程中数据
        /// </summary>
        /// <param name="processKey">进程唯一标识</param>
        /// <returns></returns>
        public static string ReadMessage(string processKey)
        {
            InitProcessMessage(processKey);
            return allProcessMessage[processKey].ReadMessage();
        }
        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="processKey"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static bool SendData(string processKey, byte[] data)
        {
            InitProcessMessage(processKey);
            return allProcessMessage[processKey].SendData(data);
        }


        public static void InitProcessMessage(string processKey, ProcessMessageType processMessageType = ProcessMessageType.ShareMemory)
        {
            if (!allProcessMessage.Keys.Contains(processKey))
            {
                var mainWait = new ProcessMessageProxy(processKey, processMessageType);
                allProcessMessage.Add(processKey, mainWait);
            }
            else
            {
                allProcessMessage[processKey].ResetListenMessage();
            }
        }

        #region 内部实现

        /// <summary>
        /// 初始化进程信息 如果已经初始化将忽略不计
        /// </summary>
        /// <param name="processKey"></param>
        private static void InitProcessMessage(string processKey)
        {
            if (!allProcessMessage.Keys.Contains(processKey))
            {
                var mainWait = new ProcessMessageProxy(processKey);
                allProcessMessage.Add(processKey, mainWait);
            }
        }

        /// <summary>
        /// 初始化进程信息
        /// </summary>
        /// <param name="processKey"></param>
        /// <param name="read"></param>
        private static void InitProcessMessage(string processKey, Action<string> action, bool resetAction = false)
        {
            InitProcessMessage(processKey);
            allProcessMessage[processKey].ChangeAction(action, resetAction);
        }

        /// <summary>
        ///  初始化进程信息
        /// </summary>
        /// <param name="processKey"></param>
        /// <param name="action"></param>
        /// <param name="resetAction"></param>
        private static void InitProcessMessage(string processKey, Action<byte[]> action, bool resetAction = false)
        {
            InitProcessMessage(processKey);
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
        /// <param name="type"></param>
        public static void UnInit(string processKey, ProxyType type = ProxyType.All)
        {
            if (allProcessMessage.Keys.Contains(processKey))
            {
                allProcessMessage[processKey].Stop(type);
                var remove = type == ProxyType.All || type == allProcessMessage[processKey].GetProxyType();
                if (remove)
                {
                    allProcessMessage.Remove(processKey);
                }
            }
        }

        #region 默认  

        private static string defaultProcessKey = "eventWaitName";
        /// <summary>
        /// 读取进程中数据
        /// </summary>
        /// <returns></returns>
        public static string ReadMessage()
        {
            return ReadMessage(defaultProcessKey);
        }

        /// <summary>
        /// 像进程发送消息
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static bool WriteMessage(string message)
        {
            return SendMessage(defaultProcessKey, message);
        }
        #endregion

        #region 已过期方法

        private static ProcessMessageType processMessageType = ProcessMessageType.ShareMemory;

        /// <summary>
        /// 初始化消息类型
        /// </summary>
        /// <param name="processType"></param>
        [Obsolete("该方法已过期,修改无效，请使用ProcessMessageConfig.ProcessMessageType")]
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
        [Obsolete("该方法已过期，请使用AcceptMessage")]
        public static void ListenMessage(string processKey, Action<string> messageAction, bool resetAction = false)
        {
            AcceptMessage(processKey, messageAction, resetAction);
        }

        /// <summary>
        /// 像进程发送消息
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        [Obsolete("该方法已过期，请使用SendMessage")]
        public static bool WriteData(string processKey, string message)
        {
            return SendMessage(processKey, message);
        }
        #endregion
    }
}
