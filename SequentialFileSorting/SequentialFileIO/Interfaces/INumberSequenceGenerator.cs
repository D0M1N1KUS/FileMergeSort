namespace SequentialFileIO
{
    public interface INumberSequenceGenerator
    {
        int GetNext(bool reset = false);
        int[] GetNext(int n, bool reset = false);
        int[] GetRange(int min, int max);
        int[] GetRange(int max);
        int[] GetRangeOfN(int beginN, int n);
    }
}