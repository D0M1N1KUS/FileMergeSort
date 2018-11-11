using FileIO.RecordIO.Interfaces;

namespace SequentialFileIO
{
    public interface IInputBuffer : IDummyRecords
    {
        IRecord LastRecord { get; }
        IRecord GetNextRecord();
        bool HasNext();
        void Rewind();
    }
}