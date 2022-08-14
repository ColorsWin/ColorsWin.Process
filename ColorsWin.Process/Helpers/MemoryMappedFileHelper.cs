namespace ColorsWin.Process.Helpers
{
    public class MemoryMappedFileHelper
    {
        /// <summary>
        /// 创建内存映射文件
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
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
