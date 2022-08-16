namespace ColorsWin.Process.Helpers
{
    public class MemoryMappedFileHelper
    {      
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
