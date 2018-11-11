using System.IO;
using FileIO.RecordIO;

namespace SequentialFileSorting.Sorting
{
    public static class PreSorting
    {
        public static int GetNumberOfSeries(string filePath)
        {
            var previousRecord = Record.Min;
            var valueComponentsSplitter = new ValueComponentsSplitter();
            
            var seriesCount = 1;
            using (var reader = File.OpenText(filePath))
            {
                var line = reader.ReadLine();
                while(line != null)
                {
                    var currentRecord = new Record(valueComponentsSplitter.GetValues(line));
                    if ((Record) previousRecord > currentRecord)
                        seriesCount++;
                    previousRecord = currentRecord;
                    
                    line = reader.ReadLine();
                }
            }

            return seriesCount;
        }
    }
}