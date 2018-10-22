using FileIO.RecordIO.Interfaces;

namespace RecordFileGenerator.Interfaces
{
    public interface IRandomRecordGenerator
    {
        IRecord GenerateRandomRecord();
        string[] GenerateRandomRecordValuesStringArray();
        string GenerateRandomRecordValuesString(string separator);
        double[] GenerateRandomRecordValuesDoubleArray();
    }
}