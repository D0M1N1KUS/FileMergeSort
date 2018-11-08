using FileIO.Builders;
using SequentialFileIO;

namespace SequentialFileSorting.Sorting
{
    //One class to rule them all. Initialize all the backend crap here (using a configuration class)
    public class RecordSortingBuffers : ISortingBuffers
    {
        public readonly FileReaderBuilder FileReaderBuilder;
        public readonly FileWriterBuilder FileWriterBuilder;

        public RecordSortingBuffers(string pathToRecordFile, int numberOfSortingBuffers)
        {
            FileReaderBuilder = new FileReaderBuilder();
            FileWriterBuilder = new FileWriterBuilder();
        }
        
        
    }
}