namespace FileIO.Interfaces
{
    public interface ILineSeparator
    {
        string SeparationExcess { get; } 
        int SeparateLines(string block, out string[] separatedLines);
    }
}