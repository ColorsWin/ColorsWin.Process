using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace ColorsWin.Process.Helpers
{
    public class ByteConvertHelper
    {
        #region Struct And Bytes

        public static byte[] StructToBytes<T>(T structure) where T : struct
        {
            Int32 size = Marshal.SizeOf(structure);
            Console.WriteLine(size);
            IntPtr buffer = Marshal.AllocHGlobal(size);
            try
            {
                Marshal.StructureToPtr(structure, buffer, false);
                byte[] bytes = new byte[size];
                Marshal.Copy(buffer, bytes, 0, size);
                return bytes;
            }
            finally
            {
                Marshal.FreeHGlobal(buffer);
            }
        }

        public static T BytesToStruct<T>(byte[] bytes) where T : struct
        {
            Type strcutType = typeof(T);
            Int32 size = Marshal.SizeOf(strcutType);
            IntPtr buffer = Marshal.AllocHGlobal(size);
            try
            {
                Marshal.Copy(bytes, 0, buffer, size);
                return (T)Marshal.PtrToStructure(buffer, strcutType);
            }
            finally
            {
                Marshal.FreeHGlobal(buffer);
            }
        }
        #endregion

        #region Object And Bytes

        public static byte[] ObjectToBytes<T>(T obj)
        {
            byte[] buff;
            using (MemoryStream ms = new MemoryStream())
            {
                IFormatter iFormatter = new BinaryFormatter();
                iFormatter.Serialize(ms, obj);
                buff = ms.GetBuffer();
            }
            return buff;
        }


        public static T BytesToObject<T>(byte[] buff)
        {
            var obj = default(T);
            using (MemoryStream ms = new MemoryStream(buff))
            {
                IFormatter iFormatter = new BinaryFormatter();
                obj = (T)iFormatter.Deserialize(ms);
            }
            return obj;
        }

        #endregion



        public static byte[] ToBytes<T>(T obj)
        {
            byte[] data = null;
            var type = typeof(T);
            if (type.IsValueType)
            {
                if (type == typeof(bool))
                {
                    data = BitConverter.GetBytes(Convert.ToBoolean(obj));
                }
                else if (type == typeof(char))
                {
                    data = BitConverter.GetBytes(Convert.ToChar(obj));
                }
                else if (type == typeof(short))
                {
                    data = BitConverter.GetBytes(Convert.ToInt16(obj));
                }
                else if (type == typeof(int))
                {
                    data = BitConverter.GetBytes(Convert.ToInt32(obj));
                }
                else if (type == typeof(long))
                {
                    data = BitConverter.GetBytes(Convert.ToInt64(obj));
                }
                else if (type == typeof(ushort))
                {
                    data = BitConverter.GetBytes(Convert.ToUInt16(obj));
                }
                else if (type == typeof(uint))
                {
                    data = BitConverter.GetBytes(Convert.ToUInt32(obj));
                }
                else if (type == typeof(ulong))
                {
                    data = BitConverter.GetBytes(Convert.ToUInt64(obj));
                }
                else if (type == typeof(float))
                {
                    data = BitConverter.GetBytes(Convert.ToSingle(obj));
                }
                else if (type == typeof(double))
                {
                    data = BitConverter.GetBytes(Convert.ToDouble(obj));
                }
                else if (type == typeof(byte))
                {
                    data = BitConverter.GetBytes(Convert.ToByte(obj));
                }
                else if (type == typeof(sbyte))
                {
                    data = BitConverter.GetBytes(Convert.ToSByte(obj));
                }

                else if (type == typeof(DateTime))
                {
                    var longDate = Convert.ToDateTime(obj);
                    data = BitConverter.GetBytes(longDate.Ticks);
                }
                else if (type.IsPrimitive)
                {
                    int size = Marshal.SizeOf(obj);
                    IntPtr buffer = Marshal.AllocHGlobal(size);
                    try
                    {
                        Marshal.StructureToPtr(obj, buffer, false);
                        byte[] bytes = new byte[size];
                        Marshal.Copy(buffer, bytes, 0, size);
                        return bytes;
                    }
                    finally
                    {
                        Marshal.FreeHGlobal(buffer);
                    }
                }
            }
            else if (type == typeof(string))
            {
                data = Encoding.UTF8.GetBytes(obj.ToString());
            }
            return data;
        }


        public static T FormBytes<T>(byte[] bytes)
        {
            object data = null;
            var type = typeof(T);
            if (type.IsValueType)
            {
                if (type == typeof(bool))
                {
                    data = BitConverter.ToBoolean(bytes, 0);
                }
                else if (type == typeof(char))
                {
                    data = BitConverter.ToChar(bytes, 0);
                }
                else if (type == typeof(short))
                {
                    data = BitConverter.ToInt16(bytes, 0);
                }
                else if (type == typeof(int))
                {
                    data = BitConverter.ToInt32(bytes, 0);
                }
                else if (type == typeof(long))
                {
                    data = BitConverter.ToInt64(bytes, 0);
                }
                else if (type == typeof(ushort))
                {
                    data = BitConverter.ToUInt16(bytes, 0);
                }
                else if (type == typeof(uint))
                {
                    data = BitConverter.ToUInt32(bytes, 0);
                }
                else if (type == typeof(ulong))
                {
                    data = BitConverter.ToUInt64(bytes, 0);
                }
                else if (type == typeof(float))
                {
                    data = BitConverter.ToSingle(bytes, 0);
                }
                else if (type == typeof(double))
                {
                    data = BitConverter.ToDouble(bytes, 0);
                }
                else if (type == typeof(byte))
                {
                    data = BitConverter.ToInt16(bytes, 0);
                }
                else if (type == typeof(sbyte))
                {
                    data = BitConverter.ToInt16(bytes, 0);
                }

                else if (type == typeof(DateTime))
                {
                    var longDate = BitConverter.ToInt64(bytes, 0);

                    data = new DateTime(longDate);
                }
                else if (type.IsPrimitive)
                {
                    int size = Marshal.SizeOf(type);
                    IntPtr buffer = Marshal.AllocHGlobal(size);
                    try
                    {
                        Marshal.Copy(bytes, 0, buffer, size);
                        return (T)Marshal.PtrToStructure(buffer, type);
                    }
                    finally
                    {
                        Marshal.FreeHGlobal(buffer);
                    }
                }
            }
            else if (type == typeof(string))
            {
                data = Encoding.UTF8.GetString(bytes);
            }

            if (data == null)
            {
                return default(T);
            }
            return (T)data;
        }
    }
}
