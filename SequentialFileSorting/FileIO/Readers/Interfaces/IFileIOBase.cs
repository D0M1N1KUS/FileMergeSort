namespace FileIO.Interfaces
{
    public interface IFileIOBase
    {
        string FilePath { get; set; }
        int BlockSize { get; set; }
        char[] Block { get; set; }
        
        void CheckIfFileExists(string filePath);
        bool ValidateBlockSize(int blockSize);
        void ClearBlock();
    }
}