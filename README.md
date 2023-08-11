# ColorsWin.Process
-----------------------------------

### ProcessMessageType

|Type       |Complete          |Remark          |
| -------------|:--------------:|:--------------:|
|ShareMemory|√| default ProcessMessageType |
|NamedPipe|√|Many Send , One Accept|
|Message|√|need IntPtr|
|File|√|Many Send , Many Accept|
|MQ|X|Dependent on third-party ,so will not be realized||


-----------------------------------


#### The sendMessage process

```C++
using System;

namespace ColorsWin.Process.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            string processKey = "ProcessMessage_Key";
            ProcessMessageManager.SendMessage(processKey, " Hello App1");
            Console.Read();
        }
    }
}

```

#### The acceptMessage process

```C++
using System;

namespace ColorsWin.Process.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            string processKey = "ProcessMessage_Key";
            ProcessMessageManager.AcceptMessage(processKey, item =>
            {
                Console.WriteLine(processKey + "-----Message:" + item);
            });
            Console.Read();
        }
    }
}
```


-----------------------------------


##  V2.0  Supports generics      

```C++
using System;

namespace ColorsWin.Process.Test
{
    class Program
    {
        static void Main(string[] args)
        {
          TypeOutput();
            Console.Read();
        }

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


           //Test owner Serialize
		   
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
    }
}
```











 




