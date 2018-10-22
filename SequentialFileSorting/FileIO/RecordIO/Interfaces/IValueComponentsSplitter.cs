namespace FileIO.RecordIO.Interfaces
{
    public interface IValueComponentsSplitter
    {
        string SeparationSymbol { set; }
        string[] GetValues(string values);
        double[] GetDoubleValues(string values);
    }
}