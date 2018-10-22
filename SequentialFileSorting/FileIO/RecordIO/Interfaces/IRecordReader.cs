namespace FileIO.RecordIO.Interfaces
{
    public interface IRecordReader
    {
        IRecord GetNextRecord();
    }
}