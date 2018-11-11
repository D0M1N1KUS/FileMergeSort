namespace FileIO.Interfaces
{
    public interface IBlockReader
    {
        bool EndOfFile { get; }
        string GetNextBlock();
        void Rewind();
    }
}