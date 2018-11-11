using FileIO.RecordIO.Interfaces;

namespace SequentialFileIO
{
    public interface IRecordValueComparer
    {
        int GetIndexOfSmallest(IRecord[] recordsList, bool[] seriesEndedList);
        void AddRecordToComparison(IRecord record, bool seriesEnded);
        int GetIndexOfSmallest();
        IRecord SmallestRecord { get; }
        int IndexOfSmallest { get; }
        void Reset();
    }
}