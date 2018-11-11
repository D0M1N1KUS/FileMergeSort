using System;
using System.ComponentModel.Design;
using System.Runtime.CompilerServices;
using FileIO.Builders;
using FileIO.Interfaces;
using FileIO.RecordIO.Interfaces;

namespace FileIO.RecordIO
{
    public class LineBasedRecordReader : IRecordReader
    {
        public IFileReader FileReader;
        public IValueComponentsSplitter ValueComponentsSplitter;
        
        private string line = string.Empty;
        private IRecord nextRecord = Record.Dummy;
        private bool firstRead = true;

        public LineBasedRecordReader(IFileReader fileReader, IValueComponentsSplitter valueComponentsSplitter)
        {
            FileReader = fileReader;
            ValueComponentsSplitter = valueComponentsSplitter;
        }
        
            
        public IRecord GetNextRecord()
        {
            if (firstRead)
            {
                readNextRecord();
                firstRead = false;
            }
            var currentRecord = nextRecord;
            readNextRecord();
            return currentRecord;
        }

        private void readNextRecord()
        {
            line = string.Empty;
            while (string.IsNullOrEmpty(line) && HasNext())
            {
                line = FileReader.GetNextLine();
            }
            nextRecord = string.IsNullOrEmpty(line) 
                ? Record.NullRecord 
                : new Record(ValueComponentsSplitter.GetValues(line));
        }

        public bool HasNext()
        {
            return !FileReader.EndOfFile && !nextRecord.Equals(Record.NullRecord);
        }

        public void Restart()
        {
            FileReader.BlockReader.Rewind();
            firstRead = true;
        }
    }
}