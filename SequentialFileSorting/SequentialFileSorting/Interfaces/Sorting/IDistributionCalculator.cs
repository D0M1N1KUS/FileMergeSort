using SequentialFileSorting.Sorting;

namespace SequentialFileSorting.Interfaces.Sorting
{
    public interface IDistributionCalculator
    {
        IOptimalDistribution GetOptimalDistribution(int numberOfSeries = -1, int numberOfTemporaryBuffers = -1);
    }
}