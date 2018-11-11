using System;
using SequentialFileIO;

namespace SequentialFileSorting.Sorting
{
    public class Distribution : IDistribution
    {
        public IDistributionBufferingIO BufferIO;
        
        private int numberOfOutputBuffers;
        private IOptimalDistribution optimalDistribution;
        
        public Distribution(int numberOfOutputBuffers, IDistributionBufferingIO bufferIO, int numberOfSeries)
        {
            if(bufferIO == null)
                throw new Exception("Distribution: buffers can't be null!");
            BufferIO = bufferIO;
            this.numberOfOutputBuffers = numberOfOutputBuffers;
            optimalDistribution =
                new DistributionCalculator(numberOfSeries, numberOfOutputBuffers).GetOptimalDistribution();
        }
        
        public void Distribute()
        {
            for (var i = 0; i < numberOfOutputBuffers; i++)
            {
                for (var j = 0; j < optimalDistribution.RecordDistribution[i]; j++)
                    BufferIO.WriteNextSeriesToBuffer(i);
                for (var j = 0; j < optimalDistribution.DummyRecordDistribution[i]; j++)
                    BufferIO.AddDummyRecord(i);
            }
        }
    }
}