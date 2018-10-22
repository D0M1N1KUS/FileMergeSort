using System;

namespace FileIO.RecordIO.Interfaces
{
    public interface IRecord : IComparable<IRecord>
    {
        double Value { get; }
        int Length { get; }
        string[] ValueComponentsArray { get; }
        string ValueComponentsString(string separator);
    }
}