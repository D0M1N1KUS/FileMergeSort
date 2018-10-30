using System;

namespace FileIO.RecordIO.Interfaces
{
    public interface IRecord : IComparable<IRecord>
    {
        double Value { get; }
        int Length { get; }
        IRecord Min { get; }
        bool IsDummy { get; }
        string[] ValueComponentsArray { get; }
        string ValueComponentsString(string separator);
        bool Equals(object obj);
        int CompareTo(IRecord other);
    }
}