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
            ProcessMessageManager.WriteData(processKey, " Hello App1");
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

            ProcessMessageManager.ListenMessage(processKey, (item) =>
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
|ShareMemory|default| 写入后暂停10毫秒,因为循环写入,导致获取消息来不及处理有丢失， |
|NamedPipe|√||
|Message|√||
|MQ|X||
|File|X|||



#### 其他:如果发现有问题请联系QQ 81867376


