using System;
using System.Collections.Generic;
using FileIO;
using FileIO.Builders;
using FileIO.Interfaces;
using FileIO.RecordIO;
using FileIO.RecordIO.Interfaces;
using FileIO.Writers;
using SequentialFileIO.Enums;

namespace SequentialFileIO
{
    public class FileBufferIo : IFileBufferIO
    {
        private IOutputBuffer[] outputBuffers;
        private IInputBuffer[] inputBuffers;

        public int InputBufferIndex { get; set; }
        
        private int capacity;

        public FileBufferIo(int capacity, IInputBuffer sourceInputBuffer, IOutputBuffer sourceOutputBuffer,
            IInputBuffer[] temporaryInputBuffers, IOutputBuffer[] temporaryOutputBuffers)
        {
            this.capacity = capacity;
            outputBuffers = new IOutputBuffer[capacity];
            inputBuffers = new IInputBuffer[capacity];
            if (sourceInputBuffer != null && sourceInputBuffer != null)
            {
                outputBuffers[0] = sourceOutputBuffer;
                inputBuffers[0] = sourceInputBuffer;
                InputBufferIndex = 0;
            }

            for (var i = 1; i < capacity; i++)
            {
                outputBuffers[i] = temporaryOutputBuffers[i - 1];
                inputBuffers[i] = temporaryInputBuffers[i - 1];
            }
        }
        
        public IRecord GetNextFromCurrentInputBuffer()
        {
            return inputBuffers[InputBufferIndex].HasNext()
                ? inputBuffers[InputBufferIndex].GetNextRecord()
                : inputBuffers[InputBufferIndex].RemoveDummyRecord();
        }

        public void AppendToOutputBuffer(int index, IRecord record)
        {
            outputBuffers[index].AppendRecord(record);
        }

        public IOutputBuffer this[int i] => 
            i <= InputBufferIndex ? outputBuffers[i + 1] : outputBuffers[i];
    }
}