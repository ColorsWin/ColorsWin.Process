﻿using System;
using System.IO.MemoryMappedFiles;
using System.Security.AccessControl;
using System.Security.Principal;

namespace ColorsWin.Process
{
    public abstract class MemoryMappedFileObj
    {
        private long fileSize = 10 * 1024 * 1024;
        private string memoryMappedFileName = null;
        protected MemoryMappedFile file = null;

        public MemoryMappedFileObj(string name) : this(name, ProcessMessageConfig.MemoryCapacity)
        {

        }

        public MemoryMappedFileObj(string name, long size)
        {
            fileSize = size;
            if (fileSize <= 1024)
            {
                throw new ArgumentException($"fileSize={fileSize} to samll");
            }
            memoryMappedFileName = name;
            Init();
        }

        private void Init()
        {
            file = MemoryMappedFile.CreateOrOpen(memoryMappedFileName, fileSize);
            SetAccessControl(file);
        }


        private void SetAccessControl(MemoryMappedFile file)
        {
#if NET40
            var sid = new SecurityIdentifier(WellKnownSidType.WorldSid, null);
            var rule = new AccessRule<MemoryMappedFileRights>(sid, MemoryMappedFileRights.FullControl, AccessControlType.Allow);

            //var rule = new AccessRule<MemoryMappedFileRights>("Everyone", MemoryMappedFileRights.FullControl, AccessControlType.Allow);
            var security = new MemoryMappedFileSecurity();
            security.AddAccessRule(rule);
            file.SetAccessControl(security);
#endif
        } 

        public virtual void WriteMessage(string message)
        {
            var data = ProcessMessageConfig.Encoding.GetBytes(message);
            IsString = true;
            WriteData(data);
            IsString = false;
        }

        public virtual string ReadMessage()
        {
            var data = ReadData();
            return ProcessMessageConfig.Encoding.GetString(data);
        }

        internal bool IsString { get; set; } = false;

        public abstract void WriteData(byte[] data);

        public abstract byte[] ReadData();
    }
}


