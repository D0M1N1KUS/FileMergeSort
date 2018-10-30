using FileIO.RecordIO.Interfaces;

namespace SequentialFileIO
{
    public interface IDummyRecords
    {
        int DummyRecords { get; }
        void AddDummyRecord(int amount = 1);
        IRecord RemoveDummyRecord();
        bool HasDummy();
    }
}