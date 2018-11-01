using FileIO.RecordIO.Interfaces;

namespace SequentialFileIO
{
    public interface IRecordValueComparer
    {
        int GetIndexOfSmallest(params IRecord[] recordsList);
        void AddRecordToComparison(IRecord record);
        int GetIndexOfSmallest();
        IRecord SmallestRecord { get; }
        void Reset();
    }
}