using System;

namespace ColorsWin.Process
{ 
    public interface IProcessMessage
    {
        event Action<byte[]> AcceptData;
        bool SendData(byte[] message);
        byte[] ReadData(); 
    }
}
