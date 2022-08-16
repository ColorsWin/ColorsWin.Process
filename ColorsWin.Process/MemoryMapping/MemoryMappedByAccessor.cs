using System.IO.MemoryMappedFiles;

namespace ColorsWin.Process
{
   
    public class MemoryMappedByAccessor : MemoryMappedFileObj
    {
        private MemoryMappedViewAccessor viewAccessor = null;
        public MemoryMappedByAccessor(string name) : base(name)
        {
            viewAccessor = file.CreateViewAccessor();
        }

        public override void WriteData(byte[] data)
        {
            int position = 0;
            byte flag = (byte)(IsString ? 1 : 0);
            viewAccessor.Write(position, flag);
            position += 1;

            viewAccessor.Write(position, data.Length);
            position += 4;
            viewAccessor.WriteArray<byte>(position, data, 0, data.Length);
        }

        public override byte[] ReadData()
        {
            int position = 0;

            int flag = viewAccessor.ReadByte(position);
            position += 1;

            int dataLength = viewAccessor.ReadInt32(position);
            if (dataLength == 0)
            {
                return null;
            }
            position += 4;
            var data = new byte[dataLength];
            viewAccessor.ReadArray<byte>(position, data, 0, data.Length);
            return data;
        }


    }
}
