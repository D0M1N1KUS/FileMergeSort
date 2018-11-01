using System;
using FileIO.RecordIO;
using FileIO.RecordIO.Interfaces;
using RecordFileGenerator.Interfaces;
using SequentialFileIO;

namespace SequentialFileSorting.Sorting
{
    /// <summary>
    /// This class' task is to distribute sequences found in the source file without knowing how many sequences there are.
    /// After the distribution dummy sequences are added as needed
    /// </summary>
    public class DynamicDistribution : IDistribution
    {
        public INumberSequenceGenerator FibonacciSequenceGenerator;
        public IDistributionIO BufferIO;
        
        private IRecord lastRecord = Record.Min;
        private IRecord currentRecord = Record.Min;
        private int numberOfOutputBuffers;
        private int[] optimalDistribution;

        private bool seriesDidntEnd => currentRecord.Value >= lastRecord.Value;

        public DynamicDistribution(int numberOfOutputBuffers, IDistributionIO bufferIo, INumberSequenceGenerator fibonacciSequenceGenerator = null)
        {
            if(bufferIo == null)
                throw new Exception("Distribution: buffers can't be null!");
            BufferIO = bufferIo;
            this.FibonacciSequenceGenerator = fibonacciSequenceGenerator ?? new FibonacciSequenceGenerator();
            this.numberOfOutputBuffers = numberOfOutputBuffers;
        }

        public void Distribute()
        {
            var iteration = 0;
            optimalDistribution =
                FibonacciSequenceGenerator.GetRangeOfN(iteration + 1, numberOfOutputBuffers);
            currentRecord = BufferIO.GetNextFromCurrentInputBuffer();
            
            while (BufferIO.InputBufferHasNext() || !optimallyDistributed())
            {
                createNewOptimalDistributionIfNecessary(iteration + 1);
                for (var i = 0; i < optimalDistribution.Length; i++) 
                {
                    if(optimalDistribution[i] <= 0) continue;
                    writeToBuffer(i);
                    optimalDistribution[i]--;
                }

                iteration++;
            }
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

        private void writeToBuffer(int i)
        {
            if (BufferIO.InputBufferHasNext())
                writeNextSeriesToBuffer(i);
            else
                BufferIO.GetOutputBuffer(i).AddDummyRecord();
        }

        private void writeNextSeriesToBuffer(int bufferNumber)
        {
            do
            {
                BufferIO.AppendToOutputBuffer(bufferNumber, currentRecord);
                lastRecord = currentRecord;
                currentRecord = BufferIO.GetNextFromCurrentInputBuffer();
            } while (seriesDidntEnd && BufferIO.InputBufferHasNext());
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