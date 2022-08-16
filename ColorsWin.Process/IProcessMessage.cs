using System;

namespace ColorsWin.Process
{ 
    public interface IProcessMessage
    {
        event Action<string> AcceptMessage; 
       
        event Action<byte[]> AcceptData;

        bool SendMessage(string message);
      
        bool SendData(byte[] message);
      
        string ReadMessage();
      
        byte[] ReadData(); 
    }
}
