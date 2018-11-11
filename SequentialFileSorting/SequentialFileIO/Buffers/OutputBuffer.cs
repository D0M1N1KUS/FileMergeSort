using System;
using FileIO.Interfaces;
using FileIO.RecordIO;
using FileIO.RecordIO.Interfaces;
using FileIO.Writers.Interfaces;

namespace SequentialFileIO
{
    public class OutputBuffer : IOutputBuffer
    {
        public IRecordAppender Appender;
        public IFileWriter FileWriter;

        public int Series { get; private set; } = 0;
        public int DummyRecords { get; private set; } = 0;
        public IRecord LastAppendedRecord { get; private set; } = Record.Min;
        public int RecordsInBuffer { get; private set; } = 0;

        private bool firstRecordHasBeenAppended => Series == 0 && LastAppendedRecord.Equals(Record.Min);

        public OutputBuffer(IRecordAppender appender = null, IFileWriter writer = null)
        {
            Appender = appender;
            FileWriter = writer;
        }
        
        public void AppendRecord(IRecord record)
        {
            if(record.IsDummy) AddDummyRecord();
            else appendRecord(record);
        }

        public void AppendRecord(string[] recordComponents)
        {
            appendRecord(new Record(recordComponents));
        }

        public void AppendRecord(double[] recordComponents)
        {
            appendRecord(new Record(recordComponents));
        }

        public void ClearBuffer()
        {
            FlushBuffer();
            FileWriter?.ClearFile();
            RecordsInBuffer = 0;
        }

        public void FlushBuffer()
        {
            FileWriter?.Flush();
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
                return Record.Dummy;
            }
            throw new Exception("End of buffer reached!");
        }

        public bool HasDummy()
        {
            return DummyRecords > 0;
        }
        
        private void appendRecord(IRecord record)
        {
            checkForEndOfSeries(record);
            if (!record.IsDummy && !record.IsNull)
            {
                Appender?.AppendRecord(record);
                RecordsInBuffer++;
            }
            LastAppendedRecord = record;
        }

        private void checkForEndOfSeries(IRecord currentRecord)
        {
            if (currentRecord.Value < LastAppendedRecord.Value || firstRecordHasBeenAppended)
                Series++;
        }
    }
}