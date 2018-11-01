using FileIO.RecordIO.Interfaces;

namespace SequentialFileIO
{
    public interface IOutputBuffer : IDummyRecords
    {
        int Series { get; }
        IRecord LastAppendedRecord { get; }
        void AppendRecord(IRecord record);
        void AppendRecord(string[] recordComponents);
        void AppendRecord(double[] recordComponents);
    }
}