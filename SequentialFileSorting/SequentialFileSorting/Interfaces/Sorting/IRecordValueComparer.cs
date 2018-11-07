using FileIO.RecordIO.Interfaces;

namespace SequentialFileIO
{
    public interface IRecordValueComparer
    {
        void Compare();
        void Compare(params IRecord[] recordsList);
        int GetIndexOfSmallest(params IRecord[] recordsList);
        void AddRecordToComparison(IRecord record);
        int GetIndexOfSmallest();
        IRecord SmallestRecord { get; }
        int IndexOfSmallest { get; }
        void Reset();
    }
}