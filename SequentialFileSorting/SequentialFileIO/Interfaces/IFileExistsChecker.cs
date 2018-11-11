namespace SequentialFileIO
{
    public interface IFileExistsChecker
    {
        bool FileExists(string path);
    }
}