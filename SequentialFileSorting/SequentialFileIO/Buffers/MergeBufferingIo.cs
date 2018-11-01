using System;
using System.Collections.Generic;
using System.Linq;
using FileIO.RecordIO.Interfaces;

namespace SequentialFileIO
{
    public class MergingIO : BufferManagementBase, IMergingIO
    {
        public MergingIO(int capacity, IInputBuffer[] inputBuffers, IOutputBuffer[] outputBuffers, int initialOutputBuffer)
        {
            this.capacity = capacity;
            this.inputBuffers = inputBuffers;
            this.outputBuffers = outputBuffers;
            selectedBuffer = initialOutputBuffer;
        }
        
        public bool AllHaveNext => HasNext().Aggregate(true, (current, boolean) => current && boolean);
        
        public IRecord[] GetNextRecords()
        {
            if (!AllHaveNext)
                throw new Exception("MergingIO: Not all input buffers have values to output!");
            
            var records = new IRecord[capacity];
            for (var i = 0; i < capacity; i++)
            {
                records[i] = GetNextRecordFrom(i);
            }

            return records;
        }

        public IRecord GetNextRecordFrom(int bufferNumber)
        {
            return inputBuffers[bufferNumber].HasNext()
                ? inputBuffers[bufferNumber].GetNextRecord()
                : inputBuffers[bufferNumber].RemoveDummyRecord();
        }

        public bool[] HasNext()
        {
            return inputBuffers.Select(buffer => buffer.HasNext()).ToArray();
        }

        public bool HasNext(int bufferNumber)
        {
            return inputBuffers[bufferNumber].HasNext();
        }

        public void AppendToDestinationBuffer(IRecord record)
        {
            outputBuffers[selectedBuffer].AppendRecord(record);
        }

        public void SetEmptyBufferAsDestinationBuffer()
        {
            var numberOfEmptyBuffers = 0;
            for (var i = 0; i < inputBuffers.Length; i++)
            {
                if (!inputBuffers[i].HasNext() && !inputBuffers[i].HasDummy())
                {
                    selectedBuffer = i;
                    numberOfEmptyBuffers++;
                }
            }

            if (numberOfEmptyBuffers > 1 || numberOfEmptyBuffers == 0)
                throw new Exception("MergingIO->SetEmptyBufferAsDestinationBuffer: Inconsistent buffer state. Can't select destination buffer out of " +
                    numberOfEmptyBuffers + " buffers!");
        }

        public void SetDestinationBuffer(int bufferIndex)
        {
            if(!inputBuffers[bufferIndex].HasNext() && !inputBuffers[bufferIndex].HasDummy())
                throw new Exception("MergingIO->SetDestinationBuffer(" +  bufferIndex + "): Buffer is  not empty!");
            selectedBuffer = bufferIndex;
        }
    }
}