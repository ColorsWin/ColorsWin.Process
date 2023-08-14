using ColorsWin.Process;
using ColorsWin.Process.Helpers;
using ColorsWin.Process.Manager;
using System;

namespace Process.ShareTest
{
    class BaseTest
    {
        public static void TestSend()
        {
            var processKey = "processKey_" + ProcessMessageConfig.ProcessMessageType;
            bool isRun = ProcessHelper.HadRun(processKey);
            if (isRun)
            {
                StartSend(processKey);
            }
            else
            {
                StartAccept(processKey);
            }
        }

        private static void StartSend(string processKey)
        {
            Console.WriteLine("Send: 【 Key】 " + processKey);
            string tempData = Console.ReadLine();
            while (tempData != "exit")
            {
                ProcessMessageManager.SendMessage(processKey, tempData);
                Console.WriteLine("SendMessage:" + tempData);

                var data = System.Text.Encoding.Default.GetBytes(tempData);
                ProcessMessageManager.SendData(processKey, data);
                Console.WriteLine("SendData:" + tempData);

                tempData = Console.ReadLine();
            }
        }

        private static void StartAccept(string processKey)
        {
            Console.WriteLine("Accept 【 Key】 " + processKey);
            ProcessMessageManager.AcceptMessage(processKey, (item) =>
            {
                Console.WriteLine(processKey + "：AcceptMessage--------" + item);
            });

            //ProcessMessageManager.AcceptData(processKey, (item) =>
            //{
            //    if (item == null)
            //    {
            //        return;
            //    }
            //    var data = System.Text.Encoding.Default.GetString(item);
            //    Console.WriteLine(processKey + "：AcceptData:--------" + data);
            //});

            string tempData = Console.ReadLine();
            while (tempData != "exit")
            {
                ProcessMessageManager.SendMessage(processKey, tempData);
                Console.WriteLine("SendMessage:" + tempData);
                tempData = Console.ReadLine();
            }
        }


        private const int MaxCount = 1000;

        public static void TestBatchSend()
        {
            string testKey = "testKey_" + ProcessMessageConfig.ProcessMessageType;
            bool isRun = ProcessHelper.HadRun(testKey);
            if (isRun)
            {
                Console.WriteLine("Send: 【 Key】" + testKey);
                for (int i = 0; i < MaxCount; i++)
                {
                    string message = "Start---" + i.ToString() + "\r\n" + "OwnerMessage---End";

                    ProcessMessageManager.SendMessage(testKey, message);

                    Console.WriteLine("SendMessage:" + message);
                }
            }
            else
            {
                Console.WriteLine("Accept 【 Key】" + testKey);
                ProcessMessageManager.AcceptMessage(testKey, AcceptMessage);
            }
            Console.ReadLine();
        }

        private static int i = 0;
        private static void AcceptMessage(string message)
        {
            Console.WriteLine("AcceptMessage:" + message);
            Console.Title = i.ToString();
            i++;
        }


        #region TypeOutput

        public static void TypeOutput()
        {
            string processKey = "ProcessMessage_type";

            ProcessMessageManager.AcceptData<int>(processKey, (message) =>
            {
                Console.WriteLine(processKey + "  Accept Message--------" + message.ToString());
            }, true);

            ProcessMessageManager.SendData(processKey, 123);

            // ProcessMessageManager.SendData(processKey, 123L);// Error type not match


            string structKey = "ProcessMessage_struct";
            ProcessMessageManager.AcceptData<Student>(structKey, (message) =>
            {
                Console.WriteLine(processKey + "  Accept Message--------" + message.Name);
            }, true);

            ProcessMessageManager.SendData(structKey, new Student { Name = "Test" });



            ByteConverManager.AddToByte(typeof(Class), ClassToByte);
            ByteConverManager.AddFormByte(typeof(Class), ClassFromByte);

            string classKey = "ProcessMessage_class";
            ProcessMessageManager.AcceptData<Class>(classKey, (message) =>
            {
                Console.WriteLine(processKey + "  Accept Message--------" + message.Name);
            }, true);

            ProcessMessageManager.SendData(classKey, new Class { Name = "Test" });
        }

        private static byte[] ClassToByte(object data)
        {
            //Customize converting objects to byte arrays
            return ObjectSerializeHelper.Serialize(data);
        }

        private static Class ClassFromByte(byte[] data)
        {
            return (Class)ObjectSerializeHelper.Deserialize(data);
        }

        #endregion
    }


    public struct Student
    {
        public int ID { get; set; }

        public string Name { get; set; }
    }
    [Serializable]
    public class Class
    {
        public int ID { get; set; }

        public string Name { get; set; }
    }
}
