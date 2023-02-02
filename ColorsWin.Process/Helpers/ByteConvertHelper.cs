using System;
using System.IO;
using System.Reflection;
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
            return ByteConverManager.ToBytes(obj);
        }

        public static T FormBytes<T>(byte[] bytes)
        {
            return ByteConverManager.FormBytes<T>(bytes);
        }

        public static T FormBytes<T>(string typeName, byte[] data)
        {
            var type = GetTypeByName(typeName);
            if (type == null)
            {
                return default(T);
            }
            return (T)ByteConverManager.FormBytes(type,data);
        }

        public static byte[] ToBytes2<T>(T obj)
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
            {  //采用序列化
                data = ObjectSerializeHelper.Serialize(obj);
            }
            return data;
        }

        public static T FormBytes2<T>(byte[] bytes)
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


        public static Type GetTypeByName(string typeName)
        {
            var type = Type.GetType(typeName);
            if (type == null)
            {
                Assembly[] assembly = AppDomain.CurrentDomain.GetAssemblies();
                foreach (Assembly ass in assembly)
                {
                    type = ass.GetType(typeName);
                    if (type != null)
                    {
                        return type;
                    }
                }
            }
            return type;
        }

        public static T GetTypeFormString<T>(string typeName, byte[] data)
        {
            var type = GetTypeByName(typeName);
            if (type == null)
            {
                return default(T);
            }
            return FormBytes2<T>(data);
        }

        //public static Type GetTypeByName(string typename)
        //{
        //    Type t = null;
        //    string source = typename;
        //    if (source.IndexOf('<') > 0)
        //    {
        //        List<string> lv = new List<string>();
        //        while (RegexExpand.IsMatch(source, @"<[^<>]+>"))
        //        {
        //            lv.Add(RegexExpand.Match(source, @"(?<=<)[^<>]+(?=>)").Value);
        //            source = RegexExpand.Replace(source, @"<[^<>]+>", "/" + (lv.Count - 1));
        //        }
        //        List<Type[]> args = new List<Type[]>();
        //        for (int i = 0; i < lv.Count; i++)
        //        {
        //            List<Type> arg = new List<Type>();
        //            string[] sp = lv[i].Split(',');
        //            for (int j = 0; j < sp.Length; j++)
        //            {
        //                string s = sp[j].Trim();
        //                if (!string.IsNullOrEmpty(s))
        //                {
        //                    if (RegexExpand.IsMatch(s, @"/\d+$"))
        //                    {
        //                        Match m = RegexExpand.Match(s, @"^([^/\s]+)\s*/(\d+)$");
        //                        if (!m.Success)
        //                        {
        //                            throw new Exception("");
        //                        }
        //                        Type p = GetTypeByName(m.Groups[1].Value);
        //                        Type c = p.MakeGenericType(args[Convert.ToInt32(m.Groups[2].Value)]);
        //                        arg.Add(c);
        //                    }
        //                    else
        //                    {
        //                        arg.Add(GetTypeByName(s));
        //                    }
        //                }
        //            }
        //            args.Add(arg.ToArray());
        //        }
        //        Match f = RegexExpand.Match(source, @"^([^/\s]+)\s*/(\d+)$");
        //        if (!f.Success)
        //        {
        //            throw new Exception("");
        //        }
        //        Type fp = GetTypeByName(f.Groups[1].Value);
        //        Type fc = fp.MakeGenericType(args[Convert.ToInt32(f.Groups[2].Value)]);
        //        return fc;
        //    }
        //    else
        //    {
        //        try
        //        {
        //            t = Type.GetType(source);
        //            if (t != null)
        //            {
        //                return t;
        //            }
        //            Assembly[] assembly = AppDomain.CurrentDomain.GetAssemblies();
        //            foreach (Assembly ass in assembly)
        //            {
        //                t = ass.GetType(source);
        //                if (t != null)
        //                {
        //                    return t;
        //                }
        //                Type[] ts = ass.GetTypes();
        //                foreach (Type st in ts)
        //                {
        //                    if (RegexExpand.IsMatch(st.FullName, @"\." + RegexExpand.FormatRegExp(source) + @"(`?\d+)?$"))
        //                    {
        //                        return st;
        //                    }
        //                }
        //            }
        //        }
        //        catch (Exception ex)
        //        {

        //        }
        //    }
        //    return t;
        //}
    }


    public class TypeToByteAttribute : Attribute
    {

        public Type Type1 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool NotSystem { get; set; }
    }


    public class TypeFromByteAttribute : Attribute
    {

        public Type Type1 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool NotSystem { get; set; }
    }
}
