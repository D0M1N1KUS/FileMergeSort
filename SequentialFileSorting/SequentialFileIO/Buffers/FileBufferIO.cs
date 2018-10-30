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
    public class FileBufferIO : IFileBufferIO
    {
        private IOutputBuffer[] outputBuffers;
        private IInputBuffer[] inputBuffers;

        public int InputBufferIndex { get; set; }
        
        private int capacity;
        private int selectedBuffer = 0;

        public FileBufferIO(int capacity, IInputBuffer sourceInputBuffer, IOutputBuffer sourceOutputBuffer,
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

        public bool InputBufferHasNext()
        {
            return inputBuffers[InputBufferIndex].HasNext();
        }

        public void AppendToOutputBuffer(int bufferNumber, IRecord record)
        {
            outputBuffers[bufferNumber].AppendRecord(record);
        }

        public void AppendToOutputBuffer(IRecord record)
        {
            this[selectedBuffer].AppendRecord(record);
        }

        public void SwitchToNextOutputBuffer()
        {
            selectedBuffer = (selectedBuffer + 1) % capacity - 1;
        }

        public IOutputBuffer this[int i] => 
            i >= InputBufferIndex ? outputBuffers[i + 1] : outputBuffers[i];
        
        public IOutputBuffer GetOutputBuffer(int bufferNumber)
        {
            return this[bufferNumber];
        }
    }
}