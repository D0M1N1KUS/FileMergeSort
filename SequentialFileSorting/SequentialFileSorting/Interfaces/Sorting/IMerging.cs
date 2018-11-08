namespace SequentialFileIO
{
    public interface IMerging
    {
        bool FileIsSorted { get; }
        int Steps { get; }
        void Merge();
        void Step();
    }
}