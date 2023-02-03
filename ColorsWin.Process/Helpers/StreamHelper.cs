using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ColorsWin.Process.Helpers
{
    public class StreamHelper
    {
        public static void WriteData(Stream stream, byte[] data)
        {        
            var headerData = BitConverter.GetBytes(data.Length);
            stream.Write(headerData, 0, headerData.Length);
            stream.Write(data, 0, data.Length);
        }

        static byte[] emptyData = new byte[0];
        public static byte[] ReadData(Stream stream)
        {
            var headerData = new byte[4];
            stream.Read(headerData, 0, headerData.Length);
            var contentLength = BitConverter.ToInt32(headerData, 0);
            if (contentLength == 0)
            {
                return emptyData;
            }
            var contentData = new byte[contentLength];
            stream.Read(contentData, 0, contentData.Length);
            return contentData;
        } 
    }


    public class StringStreamHelper
    {
        public static void WriteData(Stream stream, byte[] data, bool isString = false)
        {
            byte flag = (byte)(isString ? 1 : 0);
            stream.WriteByte(flag);
            var headerData = BitConverter.GetBytes(data.Length);
            stream.Write(headerData, 0, headerData.Length);
            stream.Write(data, 0, data.Length);
        }

        static byte[] emptyData = new byte[0];
        public static byte[] ReadData(Stream stream, out bool isString)
        {
            var flag = stream.ReadByte();
            isString = flag == 1;
            var headerData = new byte[4];
            stream.Read(headerData, 0, headerData.Length);
            var contentLength = BitConverter.ToInt32(headerData, 0);
            if (contentLength == 0)
            {
                return emptyData;
            }
            var contentData = new byte[contentLength];
            stream.Read(contentData, 0, contentData.Length);
            return contentData;
        }

      
    }
}
