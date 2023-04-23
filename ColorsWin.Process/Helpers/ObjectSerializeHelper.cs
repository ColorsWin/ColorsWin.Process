using System.IO;
using System.Runtime.Serialization.Formatters.Binary; 

namespace ColorsWin.Process.Helpers
{
    public class ObjectSerializeHelper
    {
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
