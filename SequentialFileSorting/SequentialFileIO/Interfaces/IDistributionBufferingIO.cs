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
        void AppendToOutputBuffer(int bufferNumber, IRecord record);
        void AppendToOutputBuffer(IRecord record);
        void SwitchToNextOutputBuffer();
        IOutputBuffer GetOutputBuffer(int bufferNumber);
        int InputBufferIndex { get; set; }
    }
}