using ColorsWin.Process.Helpers;
using System.IO.MemoryMappedFiles;

namespace ColorsWin.Process
{

    public class MemoryMappedByStream : MemoryMappedFileObj
    {
        private MemoryMappedViewStream viewStream = null;

        public MemoryMappedByStream(string name) : base(name)
        {
            viewStream = file.CreateViewStream();
            //if (isRead)
            //{
            //    viewStream = file.CreateViewStream(0, fileSize, MemoryMappedFileAccess.Read);
            //}
            //else
            //{
            //    viewStream = file.CreateViewStream();
            //}
        }

        public override void WriteData(byte[] data)
        {
            viewStream.Position = 0;
            StreamHelper.WriteData(viewStream, data, IsString);
        }

        public override byte[] ReadData()
        {
            viewStream.Position = 0;
            bool tempIsString;
            var data = StreamHelper.ReadData(viewStream, out tempIsString);
            IsString = tempIsString;
            return data;
        }
    }
}
