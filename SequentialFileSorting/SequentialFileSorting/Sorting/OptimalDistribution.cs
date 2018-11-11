namespace SequentialFileSorting.Sorting
{
    public struct OptimalDistribution : IOptimalDistribution
    {
        public int[] RecordDistribution { get; private set; }
        public int[] DummyRecordDistribution { get; private set; }

        public OptimalDistribution(int[] recordDistribution, int[] dummyRecordDistribution)
        {
            RecordDistribution = recordDistribution;
            DummyRecordDistribution = dummyRecordDistribution;
        }
    }
}