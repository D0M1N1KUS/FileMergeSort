using System;
using RecordFileGenerator.Interfaces;
using SequentialFileIO;

namespace SequentialFileSorting.Sorting
{
    public class Distribution : IDistributor
    {
        public INumberSequenceGenerator FibonacciSequenceGenerator;
        public IFileBuffers Buffers;

        public Distribution(IFileBuffers buffers, INumberSequenceGenerator fibonacciSequenceGenerator = null)
        {
            if(buffers == null)
                throw new Exception("Distribution: buffers can't be null!");
            Buffers = buffers;
            fibonacciSequenceGenerator = fibonacciSequenceGenerator ?? new FibonacciSequenceGenerator();
        }

        public void Distribute()
        {
            
        }
    }
}