using System;
using FileIO.RecordIO;
using FileIO.RecordIO.Interfaces;
using RecordFileGenerator.Interfaces;
using SequentialFileIO;

namespace SequentialFileSorting.Sorting
{
    public class DynamicDistribution : IDistribution
    {
        public INumberSequenceGenerator FibonacciSequenceGenerator;
        public IDistributionBufferingIO BufferIO;
        public int Series => BufferIO.Series;
        public int Records => BufferIO.Records;
        
        private int numberOfOutputBuffers;
        private int[] optimalDistribution;

        public DynamicDistribution(int numberOfOutputBuffers, IDistributionBufferingIO bufferIO, 
            INumberSequenceGenerator fibonacciSequenceGenerator = null)
        {
            if(bufferIO == null)
                throw new Exception("Distribution: buffers can't be null!");
            BufferIO = bufferIO;
            this.FibonacciSequenceGenerator = fibonacciSequenceGenerator ?? new FibonacciSequenceGenerator();
            this.numberOfOutputBuffers = numberOfOutputBuffers;
        }

        public void Distribute()
        {
            var iteration = 0;
            optimalDistribution =
                FibonacciSequenceGenerator.GetRangeOfN(iteration + 1, numberOfOutputBuffers);
            
            while (BufferIO.InputBufferHasNext() || !optimallyDistributed())
            {
                createNewOptimalDistributionIfNecessary(iteration + 1);
                for (var i = 0; i < optimalDistribution.Length; i++) 
                {
                    if(optimalDistribution[i] <= 0) continue;
                    BufferIO.WriteNextSeriesToBuffer(i);
                    optimalDistribution[i]--;
                }

                iteration++;
            }
            
            BufferIO.FlushOutputBuffers();
        }

        private void createNewOptimalDistributionIfNecessary(int iteration)
        {
            if (optimallyDistributed() && BufferIO.InputBufferHasNext())
            {
                optimalDistribution = FibonacciSequenceGenerator.GetRangeOfN(iteration, numberOfOutputBuffers);
                
                for (var i = 0; i < optimalDistribution.Length; i++)
                {
                    optimalDistribution[i] -= BufferIO.GetOutputBuffer(i).Series;
                }
            }
        }

        private bool optimallyDistributed()
        {
            var isOptimallyDistributed = true;
            foreach (var value in optimalDistribution)
            {
                if (value != 0) isOptimallyDistributed = false;
            }

            return isOptimallyDistributed;
        }
    }
}