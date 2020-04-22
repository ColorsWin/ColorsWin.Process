# ColorsWin.Process

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
            ProcessMessageManager.WriteData(processKey, " Hello App1");
            Console.Read();
        }
    }
}

```

-----------------------------------

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
#### ProcessMessageType:
|Type       |Remark          |Remark          |
| -------------|:--------------:|
|ShareMemory|default| every Message wait 10 Millisecond ,Need to optimize |
|NamedPipe|√|very fast,Like Socket|
|Message|√||
|MQ|X|next version|
|File|X|next version||

![avatar](https://github.com/ColorsWin/ColorsWin.Process/blob/master/MessageType.png)

#### Others:


