using System.IO;

namespace ColorsWin.Process.Helpers
{
    public class FileWatcher
    {
        private FileSystemEventHandler fswHandler = null;

        public FileWatcher(FileSystemEventHandler watchHandler)
        {
            fswHandler = watchHandler;
        }

        private FileSystemWatcher fsw = new FileSystemWatcher();
        public void Start(string path, string filter)
        {
            fsw.Path = path;
            fsw.Filter = filter;
            //fsw.Created += new FileSystemEventHandler(OnFileChanged);
            fsw.Changed += new FileSystemEventHandler(OnFileChanged);
            //fsw.Renamed += new RenamedEventHandler(watcher.OnFileChanged);
            //fsw.Deleted += new FileSystemEventHandler(OnFileChanged);
            fsw.EnableRaisingEvents = true;
        }

        public void OnFileChanged(object sender, FileSystemEventArgs e)
        {
            fswHandler(this, e);
        }
    }
}