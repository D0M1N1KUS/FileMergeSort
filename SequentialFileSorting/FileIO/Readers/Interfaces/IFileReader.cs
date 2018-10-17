namespace FileIO.Interfaces
{
    public interface IFileReader
    {
        bool EndOfFile { get; }
        
        IBlockReader BlockReader { get; set; }
        ILineSeparator LineSeparator { get; set;}
        
        string GetNextLine();
    }
}