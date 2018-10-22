namespace FileIO.RecordIO.Interfaces
{
    public interface IRecordAppender
    {
        void AppendRecord(IRecord record);
        void AppendRecord(double[] recordComponents);
        void AppendRecord(string[] recordComponents);
    }
}