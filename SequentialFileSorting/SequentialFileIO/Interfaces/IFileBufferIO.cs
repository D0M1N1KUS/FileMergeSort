using System.Collections.Generic;
using FileIO.Interfaces;
using FileIO.RecordIO.Interfaces;
using SequentialFileIO.Enums;

namespace SequentialFileIO
{
    public interface IFileBufferIO
    {
        IRecord GetNextFromCurrentInputBuffer();
        void AppendToOutputBuffer(int index, IRecord record);
        IOutputBuffer this[int i] { get; }
        int InputBufferIndex { get; set; }
    }
}