using System.IO;

namespace SequentialFileIO
{
    public class FileExistsChecker : IFileExistsChecker
    {
        public bool FileExists(string path)
        {
            return File.Exists(path);
        }
    }
}