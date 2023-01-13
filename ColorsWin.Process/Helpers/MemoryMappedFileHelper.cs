namespace ColorsWin.Process.Helpers
{
    public class MemoryMappedFileHelper
    {
        public static bool CreateMemoryMappedFile(string name)
        {
            CreateMemoryMappedFileObj(name);
            return true;
        }

        internal static MemoryMappedFileObj CreateMemoryMappedFileObj(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return null;
            }
            return new MemoryMappedByStream(name);
            //return new MemoryMappedByAccessor(name);
        }
    }
}
