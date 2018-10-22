namespace RecordFileGenerator.Interfaces
{
    public interface IPseugeRNG
    {
        double GetDouble();
        int GetInt();
        int GetInt(int from, int to);
    }
}