# ColorsWin.Process

#### 给进程发送消息

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

-----------------------------------

#### 接受进程消息

```C++
using System;

namespace ColorsWin.Process.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            string processKey = "ProcessMessage_Key";

            ProcessMessageManager.AcceptMessage(processKey, (item) =>
            {
                Console.WriteLine(processKey + "-----Message:" + item);
            });
            Console.Read();
        }
    }
}
```

-----------------------------------
#### 进程消息类型:
|类型       |是否实现          |说明          |
| -------------|:--------------:|:--------------:|
|ShareMemory|default| 多对多，任意一个发送其他都能收到 |
|NamedPipe|√|多个发送端,一个接收端|
|Message|√|需要 IntPtr|
|File|√|多对多，任意一个发送其他都能收到|
|MQ|X|||






