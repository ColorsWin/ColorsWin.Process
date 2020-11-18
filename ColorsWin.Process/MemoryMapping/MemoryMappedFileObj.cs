using System.IO.MemoryMappedFiles;
using System.Text;

namespace ColorsWin.Process
{
    /// <summary>
    /// 内存映射文件
    /// </summary>
    public abstract class MemoryMappedFileObj
    {
        private int fileSize = 10 * 1024 * 1024;
        private string memoryMappedFileName = null;
        protected MemoryMappedFile file = null;
        public MemoryMappedFileObj(string name) : this(name, 10 * 1024 * 1024)
        {
        }

        public MemoryMappedFileObj(string name, int size)
        {
            fileSize = size;
            memoryMappedFileName = name;
            Init();
        }

        private void Init()
        {
            file = MemoryMappedFile.CreateOrOpen(memoryMappedFileName, fileSize);
        }

        /// <summary>
        /// 写入数据
        /// </summary>
        /// <param name="data"></param>
        public abstract void WriteData(string data);
        /// <summary>
        /// 读取数据
        /// </summary>
        /// <returns></returns>
        public abstract string ReadData();
    }


    /// <summary>
    /// 通过视图
    /// </summary>
    public class MemoryMappedByAccessor : MemoryMappedFileObj
    {
        private MemoryMappedViewAccessor viewAccessor = null;
        public MemoryMappedByAccessor(string name) : base(name)
        {
            viewAccessor = file.CreateViewAccessor();
        }
        public override void WriteData(string dataInfo)
        {
            viewAccessor.Write(0, dataInfo.Length);
            var data = Encoding.UTF8.GetBytes(dataInfo);
            viewAccessor.WriteArray<byte>(4, data, 0, data.Length);
        }
        public override string ReadData()
        {
            int dataLength = viewAccessor.ReadInt32(0);
            var data = new byte[dataLength];
            viewAccessor.ReadArray<byte>(4, data, 0, dataLength);
            return Encoding.UTF8.GetString(data);
        }
    }
}


