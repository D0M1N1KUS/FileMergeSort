using System;
using System.Collections.Generic;
using System.Linq;
using FileIO.RecordIO;
using FileIO.RecordIO.Interfaces;

namespace SequentialFileIO
{
    public class MergeBufferingIO : BufferManagementBase, IMergeBufferingIO
    {
        public MergeBufferingIO(ref IInputBuffer[] inputBuffers, ref IOutputBuffer[] outputBuffers, int initialOutputBuffer)
        {
            capacity = inputBuffers.Length;
            this.inputBuffers = inputBuffers;
            this.outputBuffers = outputBuffers;
            selectedBuffer = initialOutputBuffer;
        }
        
        public bool AllHaveNext => hasNext().Aggregate(true, (current, boolean) => current && boolean);
        public bool AllHaveNextOrDummy => hasNextOrDummy().Aggregate(true, (current, boolean) => current && boolean);
        public bool AllOutputBuffersAreEmpty =>
            hasNextOrDummy().Aggregate(false, (current, boolean) => current && boolean);


        public IRecord[] GetNextRecordsFromAllBuffers()
        {
            var records = new List<IRecord>();
            for (var i = 0; i < capacity; i++)
            {
                records.Add(hasNextOrDummy(i) ? GetNextRecordFrom(i) : Record.NullRecord);
            }

            return records.ToArray();
        }

        public IRecord GetNextRecordFrom(int bufferNumber)
        {
            if (inputBuffers[bufferNumber].HasNext())
                return GetInputBuffer(bufferNumber).GetNextRecord();
            if (inputBuffers[bufferNumber].HasDummy())
                return GetInputBuffer(bufferNumber).RemoveDummyRecord();
            return Record.NullRecord;
        }

        private bool[] hasNext()
        {
            return inputBuffers.Select(buffer => buffer.HasNext()).ToArray();
        }

        private bool[] hasNextOrDummy()
        {
            var hasNextOrDummy = new bool[capacity];
            for (var i = 0; i < capacity; i++)
            {
                hasNextOrDummy[i] = this.hasNextOrDummy(i);
            }

            return hasNextOrDummy;
        }

        private bool hasNextOrDummy(int bufferNumber)
        {
            var buffer = GetInputBuffer(bufferNumber);
            return buffer.HasNext() || buffer.HasDummy();
        }

        public void AppendToDestinationBuffer(IRecord record)
        {
            outputBuffers[selectedBuffer].AppendRecord(record);
        }

        public void SetAnyEmptyBufferAsDestinationBuffer()
        {
            var numberOfEmptyBuffers = 0;
            for (var i = 0; i < capacity; i++)
            {
                if (!hasNextOrDummy(i) && i != selectedBuffer)
                {
                    selectedBuffer = i;
                    numberOfEmptyBuffers++;
                }
            }

            if (numberOfEmptyBuffers == 0)
                throw new Exception("MergingIO->SetEmptyBufferAsDestinationBuffer: No empty buffers found!");
            
            outputBuffers[selectedBuffer].ClearBuffer();
        }

        public void SetDestinationBuffer(int bufferIndex)
        {
            if(hasNextOrDummy(bufferIndex))
                throw new Exception("MergingIO->SetDestinationBuffer(" +  bufferIndex + "): Buffer is  not empty!");
            selectedBuffer = bufferIndex;
        }

        public int GetDestinationBufferIndex()
        {
            return selectedBuffer;
        }

        public IInputBuffer GetInputBuffer(int bufferNumber)
        {
            return bufferNumber >= selectedBuffer ? inputBuffers[bufferNumber - 1] : inputBuffers[bufferNumber];
        }
    }
}