﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ColorsWin.Process.Helpers
{
    class StreamHelperGeneric
    {
        public static void WriteData(Stream stream, byte[] data, string typeName)
        {
            // 1、typeName length （1 byte）
            // 2、content length (4 byte)
            // 3、data typeName 
            // 4、data Content 

            var typeData = ProcessMessageConfig.Encoding.GetBytes(typeName);

            var typeNameLength = typeData.Length;
            if (typeNameLength > 255)
            {
                throw new ArgumentOutOfRangeException(typeName + " Length exceeds 255");
            }

            byte flag = (byte)typeNameLength;
            stream.WriteByte(flag);

            var headerData = BitConverter.GetBytes(data.Length);
            stream.Write(headerData, 0, headerData.Length);


            stream.Write(typeData, 0, typeData.Length);
            stream.Write(data, 0, data.Length);
        }

        private static byte[] emptyData = new byte[0];

        public static byte[] ReadData(Stream stream, out string typeName)
        {
            typeName = null;

            var typeNameLength = stream.ReadByte();

            var headerData = new byte[4];
            stream.Read(headerData, 0, headerData.Length);

            var contentLength = BitConverter.ToInt32(headerData, 0);
            if (contentLength == 0)
            {
                return emptyData;
            }

            var typeData = new byte[typeNameLength];
            stream.Read(typeData, 0, typeData.Length);

            typeName = ProcessMessageConfig.Encoding.GetString(typeData);


            var contentData = new byte[contentLength];
            stream.Read(contentData, 0, contentData.Length);
            return contentData;
        }

        #region Owner Format

        // 前面4个字节存储数组个数N, N里面每一个占用4个字节存储每个字符串字节大小

        public static void OwnerWriteData(Stream stream, params string[] dataInfo)
        {
            var dataLength = BitConverter.GetBytes(dataInfo.Length);
            stream.Write(dataLength, 0, dataLength.Length);

            List<byte[]> listData = new List<byte[]>();
            for (int i = 0; i < dataInfo.Length; i++)
            {
                var data = Encoding.UTF8.GetBytes(dataInfo[i]);
                listData.Add(data);
                var itemLength = BitConverter.GetBytes(data.Length);
                stream.Write(itemLength, 0, itemLength.Length);
            }

            for (int i = 0; i < dataInfo.Length; i++)
            {
                var data = listData[i];
                stream.Write(data, 0, data.Length);
            }
        }

        public static string[] OwnerReadData(Stream stream)
        {
            var dataLength = new byte[4];
            stream.Read(dataLength, 0, dataLength.Length);
            var length = BitConverter.ToInt32(dataLength, 0);
            if (length == 0)
            {
                return null;
            }
            int[] lengthInfo = new int[length];
            for (int i = 0; i < length; i++)
            {
                var itemData = new byte[4];
                stream.Read(itemData, 0, itemData.Length);
                lengthInfo[i] = BitConverter.ToInt32(itemData, 0);
            }
            string[] result = new string[length];
            for (int i = 0; i < length; i++)
            {
                var data = new byte[lengthInfo[i]];
                stream.Read(data, 0, data.Length);
                result[i] = Encoding.UTF8.GetString(data);
            }
            return result;
        }

        #endregion
    }

}
