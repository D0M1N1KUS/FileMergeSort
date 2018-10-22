using System;
using System.Collections;
using System.Linq;
using System.Text;
using FileIO.RecordIO.Interfaces;
using FileIO.Writers.Interfaces;

namespace FileIO.RecordIO
{
    public class RecordAppender : IRecordAppender
    {
        public readonly string Separator;
        
        public IFileWriter FileWriter;

        public RecordAppender(IFileWriter fileWriter, string separator = null)
        {
            FileWriter = fileWriter;
            Separator = separator ?? Environment.NewLine;
        }


        public void AppendRecord(IRecord record)
        {
            FileWriter.WriteLine(record.ValueComponentsString(Separator));
        }

        public void AppendRecord(double[] recordComponents)
        {
            checkRecordComponents(recordComponents);
            FileWriter.WriteLine(string.Join(Separator, recordComponents));
        }

        public void AppendRecord(string[] recordComponents)
        {
            checkRecordComponents(recordComponents);
            FileWriter.WriteLine(string.Join(Separator, recordComponents));
        }

        private void checkRecordComponents(ICollection collection)
        {
            if (collection.Count == 0 || collection.Count > 15)
                throw new Exception("RecordAppender: Invalid record length: " + collection.Count);
        }
    }
}