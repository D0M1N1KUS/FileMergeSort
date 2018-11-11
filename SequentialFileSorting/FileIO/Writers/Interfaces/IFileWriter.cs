namespace FileIO.Writers.Interfaces
{
    public interface IFileWriter
    {
        void Write(string text);
        void WriteLine(string text);
        void Flush();
        void ClearFile();
    }
}