namespace SequentialFileIO
{
    public interface IMerging
    {
        int DestinationBufferIndex { get; }
        bool FileIsSorted { get; }
        int Steps { get; }
        void Merge();
        void Step();
    }
}