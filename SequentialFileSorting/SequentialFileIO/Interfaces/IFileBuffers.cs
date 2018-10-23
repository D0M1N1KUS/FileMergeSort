using System.Collections.Generic;
using FileIO.RecordIO.Interfaces;
using SequentialFileIO.Enums;

namespace SequentialFileIO
{
    public interface IFileBuffers
    {
        Dictionary<string, IRecordReader> InputBuffer { get; }
        Dictionary<string, IRecordAppender> OutputBuffer { get; }
        void AddBuffer(string filePath, bool createNewFile = true,
            FileOperationType operationType = FileOperationType.Input);
        void CreateNewBuffers(int numberOfBuffers);
    }
}