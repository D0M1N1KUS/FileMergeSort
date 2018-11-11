namespace SequentialFileIO
{
    public interface IMerging
    {
        int DestinationBufferIndex { get; }
        int ExpectedNumberOfRecords { set; }
        bool FileIsSorted { get; }
        int Steps { get; }
        void Merge();
        void Step();
    }
}