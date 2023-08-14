using System;
using System.Runtime.InteropServices;
using System.Text;

namespace ColorsWin.Process.Helpers
{
    public class DefaultByteConverHelper
    {
        #region ToByte
        [TypeToByte(Type1 = typeof(bool))]
        public static byte[] ToByte(bool value)
        {
            return BitConverter.GetBytes(value);
        }

        [TypeToByte(Type1 = typeof(char))]
        public static byte[] ToByte(char value)
        {
            return BitConverter.GetBytes(value);
        }
        [TypeToByte(Type1 = typeof(short))]
        public static byte[] ToByte(short value)
        {
            return BitConverter.GetBytes(value);
        }

        [TypeToByte(Type1 = typeof(int))]
        public static byte[] ToByte(int value)
        {
            return BitConverter.GetBytes(value);
        }

        [TypeToByte(Type1 = typeof(long))]
        public static byte[] ToByte(long value)
        {
            return BitConverter.GetBytes(value);
        }
        [TypeToByte(Type1 = typeof(ushort))]
        public static byte[] ToByte(ushort value)
        {
            return BitConverter.GetBytes(value);
        }

        [TypeToByte(Type1 = typeof(uint))]
        public static byte[] ToByte(uint value)
        {
            return BitConverter.GetBytes(value);
        }

        [TypeToByte(Type1 = typeof(ulong))]
        public static byte[] ToByte(ulong value)
        {
            return BitConverter.GetBytes(value);
        }

        [TypeToByte(Type1 = typeof(double))]
        public static byte[] ToByte(double value)
        {
            return BitConverter.GetBytes(value);
        }

        [TypeToByte(Type1 = typeof(float))]
        public static byte[] ToByte(float value)
        {
            return BitConverter.GetBytes(value);
        }

        [TypeToByte(Type1 = typeof(byte))]
        public static byte[] ToByte(byte value)
        {
            return BitConverter.GetBytes(value);
        }

        [TypeToByte(Type1 = typeof(sbyte))]
        public static byte[] ToByte(sbyte value)
        {
            return BitConverter.GetBytes(value);
        }

        [TypeToByte(Type1 = typeof(DateTime))]
        public static byte[] ToByte(DateTime value)
        {
            return BitConverter.GetBytes(value.Ticks);
        }

        [TypeToByte(Type1 = typeof(string))]
        public static byte[] ToByte(string value)
        {
            return Encoding.UTF8.GetBytes(value);
        }
        #endregion

        #region FromByte

        [TypeFromByte(Type1 = typeof(bool))]
        public static bool BoolFromByte(byte[] bytes)
        {
            return BitConverter.ToBoolean(bytes, 0);
        }

        [TypeFromByte(Type1 = typeof(char))]
        public static char CharFromByte(byte[] bytes)
        {
            return BitConverter.ToChar(bytes, 0);
        }


        [TypeFromByte(Type1 = typeof(short))]
        public static short ShortFromByte(byte[] bytes)
        {
            return BitConverter.ToInt16(bytes, 0);
        }

        [TypeFromByte(Type1 = typeof(int))]
        public static int IntFromByte(byte[] bytes)
        {
            return BitConverter.ToInt32(bytes, 0);
        }

        [TypeFromByte(Type1 = typeof(long))]
        public static long LongFromByte(byte[] bytes)
        {
            return BitConverter.ToInt64(bytes, 0);
        }
        [TypeFromByte(Type1 = typeof(ushort))]
        public static ushort UShortFromByte(byte[] bytes)
        {
            return BitConverter.ToUInt16(bytes, 0);
        }

        [TypeFromByte(Type1 = typeof(uint))]
        public static uint UIntFromByte(byte[] bytes)
        {
            return BitConverter.ToUInt32(bytes, 0);
        }

        [TypeFromByte(Type1 = typeof(ulong))]
        public static ulong ULongFromByte(byte[] bytes)
        {
            return BitConverter.ToUInt64(bytes, 0);
        }

        [TypeFromByte(Type1 = typeof(double))]
        public static double DoubleFromByte(byte[] bytes)
        {
            return BitConverter.ToDouble(bytes, 0);
        }

        [TypeFromByte(Type1 = typeof(float))]
        public static float FloatFromByte(byte[] bytes)
        {
            return BitConverter.ToSingle(bytes, 0);
        }

        [TypeFromByte(Type1 = typeof(byte))]
        public static byte ByteFromByte(byte[] bytes)
        {
            return bytes[0];
        }

        //[TypeFromByte(Type1 = typeof(sbyte))]
        //public static sbyte SByteFromByte(byte[] bytes)
        //{
        //    return bytes[0];
        //}

        [TypeFromByte(Type1 = typeof(DateTime))]
        public static DateTime DateTimeFromByte(byte[] bytes)
        {
            long timeTick = BitConverter.ToInt64(bytes, 0);

            return new DateTime(timeTick);
        }

        [TypeFromByte(Type1 = typeof(string))]
        public static string StringFromByte(byte[] bytes)
        {
            return Encoding.UTF8.GetString(bytes);
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
                else if (!type.IsPrimitive)
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
            else
            {
                data = ObjectSerializeHelper.Serialize(obj);
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
                else if (!type.IsPrimitive)
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
            else
            {
                data = ObjectSerializeHelper.Deserialize(bytes);
            }
            if (data == null)
            {
                return default(T);
            }
            return (T)data;
        }
    }
}
