namespace FileIO.Interfaces
{
    public interface IFileReader
    {
        IBlockReader BlockReader { get; set; }
        ILineSeparator LineSeparator { get; set;}
        
        string GetNextLine();
    }
}