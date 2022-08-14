using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace ColorsWin.Process.Helpers
{
    /// <summary>
    /// 序列化帮助类
    /// </summary>
    public class ObjectSerializeHelper
    {

        /// <summary>
        /// 对象序列化成字节数组
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static byte[] Serialize(object data)
        {
            byte[] result = null;
            var formatter = new BinaryFormatter();
            using (var stream = new MemoryStream())
            {
                formatter.Serialize(stream, data);
                result = stream.GetBuffer();
            }
            return result;
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static object Deserialize(byte[] data)
        {
            object result = null;
            var formatter = new BinaryFormatter();
            using (var stream = new MemoryStream(data))
            {
                result = formatter.Deserialize(stream);
            }
            return result;
        }
    }
}
