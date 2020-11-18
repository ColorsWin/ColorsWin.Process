using System;
using System.IO.MemoryMappedFiles;
using System.Text;

namespace ColorsWin.Process.MemoryMapping
{
    /// <summary>
    /// 共享内存文件数据
    /// </summary>
    public class MemoryMappedByStream : MemoryMappedFileObj
    {
        private MemoryMappedViewStream viewStream = null;

        public MemoryMappedByStream(string name) : base(name)
        {
            viewStream = file.CreateViewStream();
        }

        public override void WriteData(string dataInfo)
        {
            var data = Encoding.UTF8.GetBytes(dataInfo);
            var dataLength = BitConverter.GetBytes(data.Length);

            viewStream.Position = 0;
            viewStream.Write(dataLength, 0, dataLength.Length);
            viewStream.Write(data, 0, data.Length);
        }

        public override string ReadData()
        {
            viewStream.Position = 0;
            var byteLengthData = new byte[4];
            viewStream.Read(byteLengthData, 0, byteLengthData.Length);

            var strLength = BitConverter.ToInt32(byteLengthData, 0);
            var data = new byte[strLength];
            viewStream.Read(data, 0, strLength);
            return Encoding.UTF8.GetString(data);
        }
    }
}
