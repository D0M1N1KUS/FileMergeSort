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

        public LineBasedRecordReader(string filePath)
        {
            var fileReader = new FileReaderBuilder().SetFilePath(filePath).Build();
            
        }

        public LineBasedRecordReader(IBlockReader blockReader, ILineSeparator lineSeparator, 
            IValueComponentsSplitter splitter)
        {
            FileReader = new FileReader(blockReader, lineSeparator);
        }

        public LineBasedRecordReader(IFileReader fileReader, IValueComponentsSplitter valueComponentsSplitter)
        {
            FileReader = fileReader;
            ValueComponentsSplitter = valueComponentsSplitter;
        }
        
            
        public IRecord GetNextRecord()
        {
            var line = FileReader.GetNextLine();
            return new Record(ValueComponentsSplitter.GetValues(line));
        }

        public bool HasNext()
        {
            return !FileReader.EndOfFile;
        }
    }
}