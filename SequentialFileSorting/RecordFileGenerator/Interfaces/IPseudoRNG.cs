namespace RecordFileGenerator.Interfaces
{
    public interface IPseudoRNG
    {
        double GetDouble();
        int GetInt();
        int GetInt(int from, int to);
        bool GetBool(double chanceForTrue = 0.5);
    }
}