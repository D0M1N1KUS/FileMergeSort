using System.Collections.Generic;
using FileIO.Interfaces;
using FileIO.RecordIO.Interfaces;
using SequentialFileIO.Enums;

namespace SequentialFileIO
{
    public interface IFileBufferIO
    {
        IRecord GetNextFromCurrentInputBuffer();
        bool InputBufferHasNext();
        void AppendToOutputBuffer(int bufferNumber, IRecord record);
        void AppendToOutputBuffer(IRecord record);
        void SwitchToNextOutputBuffer();
        IOutputBuffer this[int i] { get; }
        IOutputBuffer GetOutputBuffer(int bufferNumber);
        int InputBufferIndex { get; set; }
    }
}