using ColorsWin.Process.Helpers;
using System;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Security.AccessControl;
using System.Security.Principal;

namespace ColorsWin.Process
{
    public abstract class MemoryMappedFileObj
    {
        protected long fileSize = 10 * 1024 * 1024;
        private string memoryMappedFileName = null;
        protected MemoryMappedFile file = null;
        protected bool isRead = false;

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
            bool doesNotExist = false;
            bool unauthorized = false;

            try
            {
                file = MemoryMappedFile.OpenExisting(memoryMappedFileName);
            }
            catch (FileNotFoundException ex)
            {
                //LogHelper.Debug(memoryMappedFileName + " Named event does not exist.");
                doesNotExist = true;
            }
            catch (UnauthorizedAccessException ex)
            {
                LogHelper.Debug(memoryMappedFileName + "Unauthorized access:  " + ex.Message);
                unauthorized = true;
            }

            if (doesNotExist)
            {
                bool isAdmmin = ProcessHelper.IsRunAsAdmin();

                try
                {
                    file = MemoryMappedFile.CreateNew(memoryMappedFileName, fileSize);
                }
                catch (UnauthorizedAccessException ex)
                {
                    string errorMessage = "Unauthorized access";
                    if (!isAdmmin)
                    {
                        errorMessage = "If it starts with Global\\ , please run the process with administrator privileges first";
                    }
                    throw new UnauthorizedAccessException(errorMessage, ex);
                }

                if (isAdmmin)
                {
                    SetAccessControl(file);
                }
            }
            else if (unauthorized)
            {
                try
                {
                    file = MemoryMappedFile.OpenExisting(memoryMappedFileName, MemoryMappedFileRights.ReadPermissions | MemoryMappedFileRights.ChangePermissions);
                }
                catch (UnauthorizedAccessException ex)
                {
                    LogHelper.Debug("Unable to change permissions: {0}" + ex.Message);
                    throw new UnauthorizedAccessException("Unable to change permissions ", ex);
                }
            }
        }

        /// <summary>
        /// Non-administrator users can also read and write
        /// </summary>
        /// <param name="file"></param>
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


