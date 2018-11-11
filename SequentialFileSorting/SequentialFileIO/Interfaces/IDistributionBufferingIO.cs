using System.Collections.Generic;
using FileIO.Interfaces;
using FileIO.RecordIO.Interfaces;
using SequentialFileIO.Enums;

namespace SequentialFileIO
{
    public interface IDistributionBufferingIO
    {
        IRecord GetNextFromCurrentInputBuffer();
        bool InputBufferHasNext();
        void WriteNextSeriesToBuffer(int bufferNumber);
        void AppendToOutputBuffer(int bufferNumber, IRecord record);
        void AppendToOutputBuffer(IRecord record);
        void SwitchToNextOutputBuffer();
        void AddDummyRecord(int bufferNumber);
        void FlushOutputBuffers();
        IOutputBuffer GetOutputBuffer(int bufferNumber);
        int Series { get; }
        int Records { get; }
    }
}