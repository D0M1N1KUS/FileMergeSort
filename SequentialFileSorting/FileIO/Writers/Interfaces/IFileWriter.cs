namespace FileIO.Writers.Interfaces
{
    public interface IFileWriter
    {
        void Write(string text);
        void Flush();
    }
}