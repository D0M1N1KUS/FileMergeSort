namespace FileIO.RecordIO.Interfaces
{
    public interface IRecordReader
    {
        IRecord GetNextRecord();
        bool HasNext();
        void Restart();
    }
}