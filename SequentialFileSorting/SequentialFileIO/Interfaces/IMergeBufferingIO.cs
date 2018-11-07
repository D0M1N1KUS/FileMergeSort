using FileIO.RecordIO.Interfaces;

namespace SequentialFileIO
{
    public interface IMergeBufferingIO
    {
        IRecord[] GetNextRecordsFromAllBuffers();
        IRecord GetNextRecordFrom(int bufferNumber);
        bool AllHaveNext { get; }
        bool AllHaveNextOrDummy { get; }
        bool AllOutputBuffersAreEmpty { get; }
        void AppendToDestinationBuffer(IRecord record);
        void SetAnyEmptyBufferAsDestinationBuffer();
        void SetDestinationBuffer(int bufferIndex);
        int GetDestinationBufferIndex();
    }
}