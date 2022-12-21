using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Json;

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


    public class SystemMessageSerializeHelper
    {
        internal static byte[] Serialize(SystemMessage data)
        {
            var serializer = new DataContractJsonSerializer(data.GetType());
            MemoryStream stream = new MemoryStream();
            serializer.WriteObject(stream, data);
            byte[] dataBytes = new byte[stream.Length];
            stream.Position = 0;
            stream.Read(dataBytes, 0, (int)stream.Length);
            return dataBytes;            
        }

        internal static SystemMessage Deserialize(byte[] data)
        {
            var serializer = new DataContractJsonSerializer(typeof(SystemMessage));
            MemoryStream mStream = new MemoryStream(data);
            return serializer.ReadObject(mStream) as SystemMessage;
        }
    }
}
