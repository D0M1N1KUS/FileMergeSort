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
    public class DistributionBufferingIO : BufferManagementBase, IDistributionBufferingIO
    {
        private int capacity;
        private IRecord lastRecord = Record.Min;
        private IRecord currentRecord = Record.Min;
        
        private bool seriesDidntEnd => currentRecord.Value >= lastRecord.Value;

        
        public DistributionBufferingIO(int numberOfTemporaryBuffers, ref IInputBuffer sourceInputBuffer, ref IOutputBuffer sourceOutputBuffer,
            ref IInputBuffer[] temporaryInputBuffers, ref IOutputBuffer[] temporaryOutputBuffers)
        {
            this.capacity = numberOfTemporaryBuffers + 1;
            outputBuffers = new IOutputBuffer[capacity];
            inputBuffers = new IInputBuffer[capacity];
            if (sourceInputBuffer != null && sourceInputBuffer != null)
            {
                outputBuffers[0] = sourceOutputBuffer;
                inputBuffers[0] = sourceInputBuffer;
                selectedBuffer = 0;
            }

            for (var i = 1; i < capacity; i++)
            {
                outputBuffers[i] = temporaryOutputBuffers[i - 1];
                inputBuffers[i] = temporaryInputBuffers[i - 1];
            }
        }
        
        public IRecord GetNextFromCurrentInputBuffer()
        {
            return inputBuffers[selectedBuffer].HasNext()
                ? inputBuffers[selectedBuffer].GetNextRecord()
                : inputBuffers[selectedBuffer].HasDummy()
                    ? inputBuffers[selectedBuffer].RemoveDummyRecord()
                    : Record.NullRecord;
        }

        public bool InputBufferHasNext()
        {
            return inputBuffers[selectedBuffer].HasNext();
        }
        
        public void WriteNextSeriesToBuffer(int bufferNumber)
        {
            if (InputBufferHasNext())
                writeNextSeriesToBuffer(bufferNumber);
            else
                GetOutputBuffer(bufferNumber).AddDummyRecord();
        }

        private void writeNextSeriesToBuffer(int bufferNumber)
        {
            if (currentRecord.Equals(Record.Min)) currentRecord = GetNextFromCurrentInputBuffer();

            do
            {
                AppendToOutputBuffer(bufferNumber, currentRecord);
                lastRecord = currentRecord;
                currentRecord = GetNextFromCurrentInputBuffer();

            } while (seriesDidntEnd && InputBufferHasNext());
        }

        public void AppendToOutputBuffer(int bufferNumber, IRecord record)
        {
            GetOutputBuffer(bufferNumber).AppendRecord(record);
        }

        public void AppendToOutputBuffer(IRecord record)
        {
            GetOutputBuffer(selectedBuffer).AppendRecord(record);
        }

        public void SwitchToNextOutputBuffer()
        {
            selectedBuffer = (selectedBuffer + 1) % capacity;
        }
        
        public IOutputBuffer GetOutputBuffer(int bufferNumber)
        {
            return bufferNumber >= selectedBuffer ? outputBuffers[bufferNumber + 1] : outputBuffers[bufferNumber];
        }
    }
}