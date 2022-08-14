﻿using ColorsWin.Process;
using System;

namespace Process.ShareTest
{
    public class TestMemoryShare
    {
        private static bool IsRun()
        {
            var defaultKey = AppDomain.CurrentDomain.FriendlyName;
            defaultKey = "processKey_MemoryShare";
            var defaultData = ProcessMessageManager.ReadMessage(defaultKey);
            bool isRun = !string.IsNullOrEmpty(defaultData);
            if (!isRun)
            {
                ProcessMessageManager.SendMessage(defaultKey, "开始运行");
            }
            return isRun;
        }

        public static void OutPut()
        {
            var processKey = "processKey_MemoryShare";
            bool write = IsRun();
            if (write)
            {
                Console.WriteLine("当前程序将作为发送端: 当前key为" + processKey);
                string tempData = Console.ReadLine();
                while (tempData != "exit")
                {
                    ProcessMessageManager.SendMessage(processKey, tempData);
                    Console.WriteLine("写入内存数据:" + tempData);

                    var data = System.Text.Encoding.Default.GetBytes(tempData);
                    ProcessMessageManager.SendData(processKey, data);

                    tempData = Console.ReadLine();
                }
            }
            else
            {
                Console.WriteLine("当前程序将作为服务端,------:当前key为" + processKey);
                ProcessMessageManager.AcceptMessage(processKey, (item) =>
                {
                    Console.WriteLine(processKey + "：AcceptMessage--------" + string.Join("####", item));
                });

                ProcessMessageManager.AcceptData(processKey, (item) =>
                {
                    var data = System.Text.Encoding.Default.GetString(item);
                    Console.WriteLine(processKey + "：AcceptData:--------" + data);
                });
            }
        }


        public static void BaseOutput()
        {
            string processKey = "ProcessMessage_t";
            string processKey2 = "ProcessMessage_t2";

            ProcessMessageManager.AllMessageEvent += (key, message) =>
            {
                Console.WriteLine($"AllMessage-进程Key:{key}---消息为:----" + message);
            };

            ProcessMessageManager.AcceptMessage(processKey, (item) =>
            {
                Console.WriteLine(processKey + "监听--------" + item);
            });

            ProcessMessageManager.AcceptMessage(processKey, (item) =>
            {
                Console.WriteLine(processKey + "监听[第二次]---------" + item);
            });

            //ProcessMessageManager.ListenMessage(processKey, (item) =>
            //{
            //    Console.WriteLine(processKey + "监听[屏蔽之前消息]---------" + item);
            //}, true);


            ProcessMessageManager.AcceptMessage(processKey2, (item) =>
            {
                Console.WriteLine(processKey2 + "监听--------" + item);
            });

            ProcessMessageManager.SendMessage(processKey, " 你好 进程1");

            ProcessMessageManager.SendMessage(processKey2, " 我是进程2");
        }
    }
}
