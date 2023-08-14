using ColorsWin.Process.Helpers;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;

namespace ColorsWin.Process.Manager
{
    public class ByteConverManager
    {
        private static Dictionary<Type, Func<object, byte[]>> customToByteDictionary;
        private static Dictionary<Type, Func<byte[], object>> customFromByteDictionary;

        private static Dictionary<Type, MethodInfo> toByteDictionary = new Dictionary<Type, MethodInfo>();
        private static Dictionary<Type, MethodInfo> fromByteDictionary = new Dictionary<Type, MethodInfo>();
        static ByteConverManager()
        {
            customToByteDictionary = new Dictionary<Type, Func<object, byte[]>>();
            customFromByteDictionary = new Dictionary<Type, Func<byte[], object>>();

            var type = typeof(DefaultByteConverHelper);
            InitType(type);
        }

        public static void ScanTypeConver(Type type)
        {
            InitType(type);
        }

        private static void InitType(Type type)
        {
            var methord = type.GetMethods(BindingFlags.Public | BindingFlags.Static);
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

        internal static byte[] ToBytes<T>(T obj)
        {
            var type = typeof(T);
            if (customToByteDictionary.ContainsKey(type))
            {
                return customToByteDictionary[type].Invoke(obj);
            }

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

                if (ProcessMessageConfig.UnknowTypeUseSerializable)
                {
                    return ObjectSerializeHelper.Serialize(obj);
                }
                else
                {
                    throw new Exception(typeof(T).FullName + "  not realization");
                }
            }
        }

        internal static T FormBytes<T>(byte[] bytes)
        {
            var type = typeof(T);
            var data = FormBytes(type, bytes);
            return (T)data;
        }

        internal static object FormBytes(Type type, byte[] bytes)
        {
            if (customFromByteDictionary.ContainsKey(type))
            {
                return customFromByteDictionary[type].Invoke(bytes);
            }

            if (fromByteDictionary.ContainsKey(type))
            {
                var data = fromByteDictionary[type].Invoke(null, new object[] { bytes });
                return data;
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
                        return Marshal.PtrToStructure(buffer, type);
                    }
                    finally
                    {
                        Marshal.FreeHGlobal(buffer);
                    }
                }

                if (ProcessMessageConfig.UnknowTypeUseSerializable)
                {
                    return ObjectSerializeHelper.Deserialize(bytes);
                }
                else
                {
                    throw new Exception("Type not realization");
                }
            }
        }


        public static bool AddToByte(Type type, Func<object, byte[]> method)
        {
            if (method == null)
            {
                return false;
            }
            customToByteDictionary[type] = method;
            return true;
        }

        public static bool AddFormByte(Type type, Func<byte[], object> method)
        {
            if (method == null)
            {
                return false;
            }
            customFromByteDictionary[type] = method;
            return true;
        }
    }
}
