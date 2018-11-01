using System;
using FileIO.Interfaces;
using FileIO.RecordIO;
using FileIO.RecordIO.Interfaces;

namespace SequentialFileIO
{
    public class OutputBuffer : IOutputBuffer
    {
        public IRecordAppender Appender;

        public int Series { get; private set; } = 0;
        public int DummyRecords { get; private set; } = 0;
        public IRecord LastAppendedRecord { get; private set; } = Record.Min;

        private bool firstRecordHasBeenAppended => Series == 0 && LastAppendedRecord.Equals(Record.Min);
        
        public void AppendRecord(IRecord record)
        {
            checkForEndOfSeries(record);
            Appender.AppendRecord(record);
            LastAppendedRecord = record;
        }

        public void AppendRecord(string[] recordComponents)
        {
            var record = new Record(recordComponents);
            checkForEndOfSeries(record);
            Appender.AppendRecord(record);
            LastAppendedRecord = record;
        }

        public void AppendRecord(double[] recordComponents)
        {
            var record = new Record(recordComponents);
            checkForEndOfSeries(record);
            Appender.AppendRecord(record);
            LastAppendedRecord = record;
        }

        public void AddDummyRecord(int amount = 1)
        {
            DummyRecords += amount;
            Series += amount;
        }

        public IRecord RemoveDummyRecord()
        {
            if (HasDummy())
            {
                DummyRecords--;
                Series--;
                return new Record(new double[0], isDummy: true);
            }
            throw new Exception("End of buffer reached!");
        }

        public bool HasDummy()
        {
            return DummyRecords > 0;
        }

        private void checkForEndOfSeries(IRecord currentRecord)
        {
            if (currentRecord.Value < LastAppendedRecord.Value || firstRecordHasBeenAppended)
                Series++;
        }
    }
}