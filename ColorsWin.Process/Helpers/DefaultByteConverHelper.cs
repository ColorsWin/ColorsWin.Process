using System;
using System.Collections.Generic;
using System.Reflection;
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
    }


    public class ByteConverManager
    {
        internal static byte[] ToBytes<T>(T obj)
        {
            var type = typeof(T);
            if (toByteDictionary.ContainsKey(type))
            {
                var data = toByteDictionary[type].Invoke(null, new object[] { obj });
                return data as byte[];
            }
            else
            {
                if (type.IsValueType && !type.IsPrimitive)
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

                throw new Exception("Type not realization");
            }
        }

        internal static T FormBytes<T>(byte[] bytes)
        {
            var type = typeof(T);
            if (fromByteDictionary.ContainsKey(type))
            {
                var data = fromByteDictionary[type].Invoke(null, new object[] { bytes });

                return (T)data;
            }
            else
            {
                if (type.IsValueType && !type.IsPrimitive)
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
                throw new Exception("Type not realization");
            }
        }

        static Dictionary<Type, MethodInfo> toByteDictionary = new Dictionary<Type, MethodInfo>();

        static Dictionary<Type, MethodInfo> fromByteDictionary = new Dictionary<Type, MethodInfo>();

        static ByteConverManager()
        {
            var methord = typeof(DefaultByteConverHelper).GetMethods(BindingFlags.Public | BindingFlags.Static);
            foreach (var item in methord)
            {
                var info = item.GetCustomAttributes(false);
                if (info.Length > 0)
                {
                    var customAttribute = info[0];
                    if (customAttribute is TypeToByteAttribute typeTo)
                    {
                        toByteDictionary.Add(typeTo.Type1, item);
                    }
                    else if (customAttribute is TypeFromByteAttribute typeFrom)
                    {
                        fromByteDictionary.Add(typeFrom.Type1, item);
                    }
                }
            }
        }

        public static void Test()
        {
            var testType = typeof(long);
            if (toByteDictionary.ContainsKey(testType))
            {
                var data = toByteDictionary[testType].Invoke(null, new object[] { 12 });

                var data2 = fromByteDictionary[testType].Invoke(null, new object[] { data });

            }
        }

        public static bool AddToByte(Type type, MethodInfo method)
        {
            return true;
        }
    }
}
