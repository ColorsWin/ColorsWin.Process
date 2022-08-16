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
            ProcessMessageManager.SendMessage(processKey, " Hello App1");
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
#### ProcessMessageType:
|Type       |Complete          |Remark          |
| -------------|:--------------:|:--------------:|
|ShareMemory|default| Many Send , Many Accept |
|NamedPipe|√|Many Send , One Accept|
|Message|√|need IntPtr|
|File|√|Many Send , Many Accept|
|MQ|X|next version||




 


