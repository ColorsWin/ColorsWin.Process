//using System.IO;
//using System.Runtime.Serialization.Json;

//namespace ColorsWin.Process.Helpers
//{
//    public class SystemMessageSerializeHelper
//    {
//        internal static byte[] Serialize(SystemMessage data)
//        {
//            var serializer = new DataContractJsonSerializer(data.GetType());
//            MemoryStream stream = new MemoryStream();
//            serializer.WriteObject(stream, data);
//            byte[] dataBytes = new byte[stream.Length];
//            stream.Position = 0;
//            stream.Read(dataBytes, 0, (int)stream.Length);
//            return dataBytes;
//        }

//        internal static SystemMessage Deserialize(byte[] data)
//        {
//            var serializer = new DataContractJsonSerializer(typeof(SystemMessage));
//            MemoryStream mStream = new MemoryStream(data);
//            return serializer.ReadObject(mStream) as SystemMessage;
//        }
//    }
//}
