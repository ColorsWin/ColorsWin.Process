using ColorsWin.Process.Helpers;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace ProcessDemo
{
    public class ConvertTest
    {
        public static void Output()
        {
            string typeName = "11".PadLeft(255, 'T');
            var typeData = System.Text.Encoding.Default.GetBytes(typeName);

            //var info = new TestInfo { Id = 20, Name = "David" };  
            //var data = ByteConvertHelper.StructToBytes(info);
            //var info2 = ByteConvertHelper.BytesToStruct<TestInfo>(data);

            //ByteConverManager.Test();

            var intData = ByteConvertHelper.ToBytes(int.MaxValue);

            var intValue = ByteConvertHelper.FormBytes<int>(intData);

            //error
            //var intValue2 = ByteConvertHelper.FormBytes<long>(intData);


            var stringData = ByteConvertHelper.ToBytes("Hello Word");
            var stringValue = ByteConvertHelper.FormBytes<string>(stringData);
            var stringValue2 = ByteConvertHelper.FormBytesByName<string>(typeof(string).FullName, stringData);


            var decimalData = ByteConvertHelper.ToBytes(5.0M);
            var decimalValue = ByteConvertHelper.FormBytes<decimal>(decimalData);

            var dateTimeData = ByteConvertHelper.ToBytes(DateTime.Now);
            var dateTimeValue = ByteConvertHelper.FormBytes<DateTime>(dateTimeData);

            var student = new StudentInfo { Id = 20, Name = "David" };
            var studentData = ByteConvertHelper.ToBytes(student);
            var studentValue = ByteConvertHelper.FormBytes<StudentInfo>(studentData);

            var studentValue2 = ByteConvertHelper.FormBytesByName<StudentInfo>(typeof(StudentInfo).FullName, studentData);

            //var studentData = ByteConvertHelper.ObjectToBytes(student);
            //var student2 = ByteConvertHelper.BytesToObject<StudentInfo>(studentData);



            List<StudentInfo> studentInfos = new List<StudentInfo> { new StudentInfo { Id = 1, Name = "Tom" } };
            var ownerData = ObjectSerializeHelper.Serialize(studentInfos);

            var intValue2 = ByteConvertHelper.FormBytesByName<List<StudentInfo>>(typeof(List<StudentInfo>).FullName, ownerData);


            var ownerIntData = ObjectSerializeHelper.Serialize(new List<int> { 1, 23 });
            var intValue3 = ByteConvertHelper.FormBytesByName<List<int>>(typeof(List<int>).FullName, ownerIntData);
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
