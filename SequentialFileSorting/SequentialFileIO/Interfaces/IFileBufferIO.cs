using System.Collections.Generic;
using FileIO.Interfaces;
using FileIO.RecordIO.Interfaces;
using SequentialFileIO.Enums;

namespace SequentialFileIO
{
    public interface IFileBuffers
    {
        int NumberOfOutputBuffers { get; }
        void AddBuffer(string filePath, bool createNewFile = true,
            FileOperationType operationType = FileOperationType.Input);
        void CreateNewBuffers(int numberOfBuffers);
        IRecordAppender GetOutputBuffer(int index);
        IRecordReader GetInputBuffer();
        IRecordAppender[] GetOutputBuffers();
    }
}