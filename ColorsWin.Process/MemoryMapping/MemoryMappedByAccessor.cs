using System.IO.MemoryMappedFiles;
using System.Text;

namespace ColorsWin.Process
{
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
            var data = Encoding.UTF8.GetBytes(dataInfo);

            viewAccessor.Write(0, data.Length);
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
