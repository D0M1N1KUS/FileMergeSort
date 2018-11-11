namespace SequentialFileSorting.Sorting
{
    public interface IOptimalDistribution
    {
        int[] RecordDistribution { get; }
        int[] DummyRecordDistribution { get; }
    }
}