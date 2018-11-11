using NUnit.Framework;
using SequentialFileSorting.Sorting;

namespace SortingTests
{
    [TestFixture]
    public class DistributionCalculatorTests
    {
        [Test]
        public void calculateDistribution_NumberOfSeriesMatchesTheFibonacciSequence_NoDummyRecordsShouldBeRequired()
        {
            var distributionCalculator = new DistributionCalculator(8, 2);
            var expectedDistribution = new OptimalDistribution(new []{5, 3},new []{0, 0});

            var actualDistribution = distributionCalculator.GetOptimalDistribution();
            
            CollectionAssert.AreEqual(expectedDistribution.RecordDistribution, actualDistribution.RecordDistribution);
            CollectionAssert.AreEqual(expectedDistribution.DummyRecordDistribution, actualDistribution.DummyRecordDistribution);
        }

        [Test]
        public void calculateDistribution_NumberOfSeriesDoesntMatchFibonacciSequence_DummyRecordsShouldBeRequired()
        {
            var distributionCalculator = new DistributionCalculator(10, 2);
            var expectedDistribution = new OptimalDistribution(new []{6, 4},new []{2, 1});

            var actualDistribution = distributionCalculator.GetOptimalDistribution();
            
            CollectionAssert.AreEqual(expectedDistribution.RecordDistribution, actualDistribution.RecordDistribution);
            CollectionAssert.AreEqual(expectedDistribution.DummyRecordDistribution, actualDistribution.DummyRecordDistribution);
        }
    }
}