namespace SequentialFileIO
{
    public interface IFileNameGenerator
    {
        string GetNextAvailableName(long triesLimit = long.MaxValue);
    }
}