using ColorsWin.Process.Helpers;
using System.IO.MemoryMappedFiles;

namespace ColorsWin.Process
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

        /// <summary>
        /// 写入数据
        /// </summary>
        /// <param name="dataInfo"></param>
        public override void WriteData(byte[] data)
        {
            viewStream.Position = 0;
            StringStreamHelper.WriteData(viewStream, data, IsString);
        }

        public override byte[] ReadData()
        {
            viewStream.Position = 0;
            bool tempIsString;
            var data = StringStreamHelper.ReadData(viewStream, out tempIsString);
            IsString = tempIsString;
            return data;
        }
    }
}
