using System;
using System.Collections.Generic;
using System.Linq;

namespace ColorsWin.Process
{
    public class ProcessManager
    {
        private static object lockObj = new object();

        private static ProcessMessageProxy gloableProcessIdManager;
        private static ProcessMessageProxy processKeyManager;

        private static string processIdKey = "";
        static ProcessManager()
        {
            gloableProcessIdManager = new ProcessMessageProxy(ProcessMessageConfig.SystemProcessKey, ProcessMessageType.ShareMemory);

            var processId = System.Diagnostics.Process.GetCurrentProcess().Id.ToString();
            gloableProcessIdManager.InitMessageType(true);
            var processData = gloableProcessIdManager.ReadData();
            var idSting = ProcessMessageConfig.Encoding.GetString(processData);
            var ids = idSting.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
            if (!ids.Contains(processId))
            {
                gloableProcessIdManager.InitMessageType(false);
                idSting += ";" + processId;
                var newProcessData = ProcessMessageConfig.Encoding.GetBytes(idSting);
                gloableProcessIdManager.SendData(newProcessData);
            }
            processIdKey = processId + "_ColorsWin";
            processKeyManager = new ProcessMessageProxy(processIdKey, ProcessMessageType.ShareMemory);
        }

        private static void ChangeData(string processKey, string newValue)
        {
            lock (lockObj)
            {
                var data = processKeyManager.ReadData();
                var listInfo = ProcessMessageConfig.Encoding.GetString(data);

                if (listInfo.Contains(processKey))
                {
                    var listKeys = listInfo.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);

                    var oldInfo = listKeys.FirstOrDefault(item => item.Contains(processKey));
                    if (oldInfo != newValue)
                    {
                        listInfo = listInfo.Replace(oldInfo, newValue);
                    }
                }
                else
                {
                    listInfo += ";" + newValue;
                }
                var newData = ProcessMessageConfig.Encoding.GetBytes(listInfo);
                processKeyManager.SendData(newData);
            }
        }

        internal static void RemoveToSystem(string processKey)
        {
            ChangeData(processKey, "");
        }

        internal static void AddToSystem(string processKey, ProcessMessageProxy processMessageProxy)
        {
            if (processKey == ProcessMessageConfig.SystemProcessKey || processKey == processIdKey)
            {
                return;
            }
            var proxyType = processMessageProxy.GetProxyType();
            var messageType = processMessageProxy.GetMessageType();
            var currentInfo = processKey + "/" + (int)messageType + "/" + (int)proxyType;
            ChangeData(processKey, currentInfo);
        }

        public static List<ProcessData> GetAllProcessInfo()
        {
            List<ProcessData> data = new List<ProcessData>();

            var idSting = ProcessMessageManager.ReadMessage(ProcessMessageConfig.SystemProcessKey);

            var ids = idSting.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var processId in ids)
            {
                var temp = new ProcessData { ProcessId = processId, Info = new List<ProcessKeyInfo>() };

                var info = ProcessMessageManager.ReadMessage(processId + "_ColorsWin");

                var listKeys = info.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);

                foreach (var item in listKeys)
                {
                    temp.Info.Add(new ProcessKeyInfo(item));
                }
                data.Add(temp);
            }
            return data;
        }
    }

    public class ProcessData
    {
        public string ProcessId { get; set; }

        public List<ProcessKeyInfo> Info { get; set; }
    }


    public class ProcessKeyInfo
    {
        public ProcessKeyInfo(string info)
        {
            Info = info;

            var keyInfo = Info.Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
            ProcessKey = keyInfo[0];
            MessageType = (ProcessMessageType)int.Parse(keyInfo[1]);
            ProxyType = (ProxyType)int.Parse(keyInfo[2]);
        }

        public string Info { get; internal set; }


        public string ProcessKey { get; internal set; }

        public ProxyType ProxyType { get; internal set; }

        public ProcessMessageType MessageType { get; internal set; }
    }
}
