using System.Collections;
using System.Collections.Generic;
using SequentialFileIO;

namespace SequentialFileSorting
{
    public class DistributionIterator : IEnumerator
    {
        private int[] range;
        
        public DistributionIterator(INumberSequenceGenerator numberSequenceGenerator, int sequenceBegin, int n)
        {
            range = numberSequenceGenerator.GetRangeOfN(sequenceBegin, n);
        }

        public DistributionIterator(int sequenceBegin, int n)
        {
            range = new FibonacciSequenceGenerator().GetRangeOfN(sequenceBegin, n);
        }
        
        public bool MoveNext()
        {
            throw new System.NotImplementedException();
        }

        public void Reset()
        {
            throw new System.NotImplementedException();
        }

        public object Current { get; }
    }
}