using System;
using FileIO.Interfaces;
using FileIO.RecordIO;
using FileIO.RecordIO.Interfaces;

namespace SequentialFileIO
{
    public class InputBuffer : IInputBuffer
    {
        public IRecordReader Reader;
        public int DummyRecords { get; private set; }
        public IRecord LastRecord { get; private set; }
        
        private IRecord currentRecord;


        public InputBuffer(IRecordReader reader = null)
        {
            Reader = reader;
            currentRecord = Record.Min;
        }
        
        public IRecord GetNextRecord()
        {
            LastRecord = currentRecord;
            currentRecord = Reader.GetNextRecord();
            return currentRecord;
        }

        public IRecord RemoveDummyRecord()
        {
            if (HasDummy())
            {
                DummyRecords--;
                return Record.Dummy;
            }
            throw new Exception("No more dummy records left!");
        }

        public bool HasNext()
        {
            return Reader.HasNext();
        }

        public void Rewind()
        {
            Reader.Restart();
        }

        public bool HasDummy()
        {
            return DummyRecords > 0;
        }
        
        public void AddDummyRecord(int amount = 1)
        {
            DummyRecords += amount;
        }
    }
}