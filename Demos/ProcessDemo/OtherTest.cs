using ColorsWin.Process.Helpers;
using System;
using System.Runtime.InteropServices;

namespace ProcessDemo
{
    public class OtherTest
    {
        public static void Output()
        {
            //var info = new TestInfo { Id = 20, Name = "David" };  
            //var data = ByteConvertHelper.StructToBytes(info);
            //var info2 = ByteConvertHelper.BytesToStruct<TestInfo>(data);

            var intData = ByteConvertHelper.ToBytes(int.MaxValue);

            var intValue = ByteConvertHelper.FormBytes<int>(intData);

            //error
            //var intValue2 = ByteConvertHelper.FormBytes<long>(intData);


            var stringData = ByteConvertHelper.ToBytes("大神你好");
            var stringValue = ByteConvertHelper.FormBytes<string>(stringData);


            var decimalData = ByteConvertHelper.ToBytes(5.0M);
            var decimalValue = ByteConvertHelper.FormBytes<decimal>(decimalData);

            var dateTimeData = ByteConvertHelper.ToBytes(DateTime.Now);
            var dateTimeValue = ByteConvertHelper.FormBytes<DateTime>(dateTimeData);

            var student = new StudentInfo { Id = 20, Name = "David" };
            var studentData = ByteConvertHelper.ToBytes(student);
            var studentValue = ByteConvertHelper.FormBytes<StudentInfo>(studentData);


            //var studentData = ByteConvertHelper.ObjectToBytes(student);
            //var student2 = ByteConvertHelper.BytesToObject<StudentInfo>(studentData);
        }
    }

    public struct TestInfo
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string Name;
        public int Id;
    }

    //[Serializable]
    public struct StudentInfo
    {
        public string Name;
        public int Id;
    }
}
