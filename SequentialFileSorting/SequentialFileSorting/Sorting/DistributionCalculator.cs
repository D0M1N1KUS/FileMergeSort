using System;
using System.Linq;
using SequentialFileIO;
using SequentialFileSorting.Interfaces.Sorting;

namespace SequentialFileSorting.Sorting
{
    public class DistributionCalculator : IDistributionCalculator
    {
        private int numberOfSeries;
        private int numberOfTemporaryBuffers;
        private INumberSequenceGenerator fibonacciSequenceGenerator;
        
        public DistributionCalculator(int numberOfSeries, int numberOfTemporaryBuffers)
        {
            this.numberOfSeries = numberOfSeries;
            this.numberOfTemporaryBuffers = numberOfTemporaryBuffers;
            fibonacciSequenceGenerator = new FibonacciSequenceGenerator();
        }
        
        public IOptimalDistribution GetOptimalDistribution(int numberOfSeries = -1, int numberOfTemporaryBuffers = -1)
        {
            var series = numberOfSeries <= 0 ? this.numberOfSeries : numberOfSeries;
            var buffers = numberOfTemporaryBuffers <= 0 ? this.numberOfTemporaryBuffers : numberOfTemporaryBuffers;

            var distribution = getDistribution(series, buffers);
            var dummyDistribution = getDummyDistribution(distribution.Sum() - series, buffers);

            for (var i = 0; i < buffers; i++)
            {
                distribution[i] -= dummyDistribution[i];
            }
            
            return new OptimalDistribution(distribution, dummyDistribution);
        }

        private int[] getDistribution(int series, int buffers)
        {
            var i = 1;
            int[] distribution;
            do
            {
                distribution = fibonacciSequenceGenerator.GetRangeOfN(i++, buffers);
            } while (distribution.Sum() < series);

            return reverse(distribution);
        }

        private int[] reverse(int[] array)
        {
            var reversedArray = new int[array.Length];
            for (var i = 0; i < array.Length; i++)
            {
                reversedArray[i] = array[(array.Length - 1) - i];
            }

            return reversedArray;
        }

        private int[] getDummyDistribution(int necessaryDummySeries, int buffers)
        {
            var dummyDistribution = Enumerable.Repeat(0, buffers).ToArray();
            var dummySeriesDistributedSoFar = 0;
            while (dummySeriesDistributedSoFar < necessaryDummySeries)
            {
                for (var j = 0; j < buffers && dummySeriesDistributedSoFar < necessaryDummySeries; j++)
                {
                    dummyDistribution[j]++;
                    dummySeriesDistributedSoFar++;
                }
            }

            return dummyDistribution;
        }
    }
}