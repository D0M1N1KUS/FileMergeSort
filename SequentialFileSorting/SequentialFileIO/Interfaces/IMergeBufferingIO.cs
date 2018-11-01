using FileIO.RecordIO.Interfaces;

namespace SequentialFileIO
{
    public interface IMergingIO
    {
        IRecord[] GetNextRecords();
        IRecord GetNextRecordFrom(int bufferNumber);
        bool[] HasNext();
        bool HasNext(int bufferNumber);
        void AppendToDestinationBuffer(IRecord record);
        void SetEmptyBufferAsDestinationBuffer();
        void SetDestinationBuffer(int bufferIndex);
    }
}