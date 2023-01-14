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


            var student = new StudentInfo { Id = 20, Name = "David" };
            var studentData = ByteConvertHelper.ObjectToBytes(student);
            var student2 = ByteConvertHelper.BytesToObject<StudentInfo>(studentData);
        }
    }

    public struct TestInfo
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string Name;
        public int Id;
    }

    [Serializable]
    public struct StudentInfo
    {
        public string Name;
        public int Id;
    }
}
