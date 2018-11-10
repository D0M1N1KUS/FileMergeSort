namespace FileIO.Interfaces
{
    public interface IFileIOBase
    {
        string FilePath { get;}
        string FileName { get; }
        int BlockSize { get;}
        char[] Block { get; }
        
        void CheckIfFileExists(string filePath);
        bool ValidateBlockSize(int blockSize);
        void ClearBlock();
        void EraseFileContent();
    }
}