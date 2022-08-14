using System;
using System.Runtime.InteropServices;

namespace ColorsWin.Process.Helpers
{
    public class ByteConvertHelper
    {
        /// <summary>
        /// 将对象转换为byte数组
        /// </summary>
        /// <param name="obj">被转换对象</param>
        /// <returns>转换后byte数组</returns>
        public static byte[] Object2Bytes(object obj)
        {

            //对象转IntPtr
            var handle = GCHandle.Alloc(obj);
            var ptr = GCHandle.ToIntPtr(handle);

            //IntPtr转对象
            //var a = GCHandle.FromIntPtr(ptr).Target;
            //handle.Free();

            byte[] buff = new byte[128];
            Marshal.Copy(ptr, buff, 0, buff.Length);

            //IntPtr ptr2 = IntPtr.Zero;
            //Marshal.Copy(buff, 0, ptr2, buff.Length);

            //var object2 = GCHandle.FromIntPtr(ptr2).Target;

            //byte[] buff = new byte[Marshal.SizeOf(obj)];
            //IntPtr ptr = Marshal.UnsafeAddrOfPinnedArrayElement(buff, 0);
            // Marshal.StructureToPtr(obj, ptr, true);


            return buff;
        }

        /// <summary>
        /// 将byte数组转换成对象
        /// </summary>
        /// <param name="buff">被转换byte数组</param>
        /// <param name="typ">转换成的类名</param>
        /// <returns>转换完成后的对象</returns>
        public static object Bytes2Object(byte[] buff, Type typ)
        {
            IntPtr ptr = Marshal.UnsafeAddrOfPinnedArrayElement(buff, 0);
            return Marshal.PtrToStructure(ptr, typ);
        }

        public static T Bytes2Object<T>(byte[] buff)
        {
            //GCHandle pinnedArray = GCHandle.Alloc(buff, GCHandleType.Pinned);
            //IntPtr pointer = pinnedArray.AddrOfPinnedObject();

            //IntPtr ptr2 = IntPtr.Zero;
            //Marshal.Copy(buff, 0, ptr2, buff.Length);

            //var object2 = GCHandle.FromIntPtr(ptr2).Target;

            IntPtr ptr = Marshal.UnsafeAddrOfPinnedArrayElement(buff, 0);
            //var a = GCHandle.FromIntPtr(ptr).Target;
            return (T)Marshal.PtrToStructure(ptr, typeof(T));
        }
    }
}
