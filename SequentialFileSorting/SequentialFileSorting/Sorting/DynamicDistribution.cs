using System;
using FileIO.RecordIO.Interfaces;
using RecordFileGenerator.Interfaces;
using SequentialFileIO;

namespace SequentialFileSorting.Sorting
{
    /// <summary>
    /// This class' task is to distribute sequences found in the source file without knowing how many sequences there are.
    /// After the distribution dummy sequences are added as needed
    /// </summary>
    public class DynamicDistribution : IDistributor
    {
        public INumberSequenceGenerator FibonacciSequenceGenerator;
        public IFileBufferIO BufferIo;

        private IRecordAppender[] currentOutputBuffers;

        public DynamicDistribution(IFileBufferIO bufferIo, INumberSequenceGenerator fibonacciSequenceGenerator = null)
        {
            if(bufferIo == null)
                throw new Exception("Distribution: buffers can't be null!");
            BufferIo = bufferIo;
            fibonacciSequenceGenerator = fibonacciSequenceGenerator ?? new FibonacciSequenceGenerator();
        }

        public void Distribute()
        {
            var iteration = 0;

        }

        private void AddToOutputBuffer(IRecord record)
        {
            foreach (var buffer in currentOutputBuffers)
            {
                //if(buffer.)
                //Ok, honestly, this is shit. I don't want to sweat my ass of while trying to find the number of
                //sequences already added to a buffer. Currently I'm getting file accessors in one collection and
                //buffer infos in an another one...
            }
        }
    }
}