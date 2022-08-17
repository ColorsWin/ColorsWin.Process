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


#### The sendMessage process by File

```C++
using System;

namespace ColorsWin.Process.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            ProcessMessageConfig.ProcessMessageType = ProcessMessageType.File;
            string processKey = "ProcessMessage_Key";
            ProcessMessageManager.SendMessage(processKey, " Hello App1");
            Console.Read();
        }
    }
}

```

#### The acceptMessage process by File

```C++
using System;

namespace ColorsWin.Process.Test
{
    class Program
    {
        static void Main(string[] args)
        {
		    ProcessMessageConfig.ProcessMessageType = ProcessMessageType.File;
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


#### The sendData process

```C++
using System;

namespace ColorsWin.Process.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            byte[] data = System.Text.Encoding.Default.GetBytes(" Hello App1");
            string processKey = "ProcessMessage_Key";
            ProcessMessageManager.SendData(processKey, data);
            Console.Read();
        }
    }
}

```

#### The acceptData process

```C++
using System;

namespace ColorsWin.Process.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            string processKey = "ProcessMessage_Key";
            ProcessMessageManager.AcceptData(processKey, data =>
            {               
                var message = System.Text.Encoding.Default.GetString(data);
                Console.WriteLine(message);
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
|ShareMemory|√| default ProcessMessageType |
|NamedPipe|√|Many Send , One Accept|
|Message|√|need IntPtr|
|File|√|Many Send , Many Accept|
|MQ|X|Dependent on third-party ,so will not be realized||




 


