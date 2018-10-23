using System.Collections.Generic;

namespace SequentialFileIO
{
    public class FibonacciSequenceGenerator : INumberSequenceGenerator
    {
        private int next;
        private int current;

        public FibonacciSequenceGenerator()
        {
            resetGenerator();
        }
        
        public int GetNext(bool reset = false)
        {
            if (reset) resetGenerator();

            var retVal = current;
            skip(1, ref current, ref next);
            return retVal;
        }

        public int[] GetNext(int n, bool reset = false)
        {
            if(reset) resetGenerator();
            
            var range = new int[n];
            for (var i = 0; i < n; i++)
                range[i] = GetNext();
            
            return range;
        }

        private void resetGenerator()
        {
            next = 1;
            current = 1;
        }

        public int[] GetRange(int min, int max)
        {
            var numberList = new List<int>();
            var next = 1;
            var current = 1;

            while (current <= max)
            {
                if(current >= min)
                    numberList.Add(current);
                skip(1, ref current, ref next);
            }

            return numberList.ToArray();
        }

        public int[] GetRange(int max)
        {
            return GetRange(0, max);
        }

        public int[] GetRangeOfN(int beginN, int n)
        {
            var next = 1;
            var current = 1;

            skip(beginN == 0 ? 0 : beginN - 1, ref current, ref next);

            var range = new int[n];
            for (var i = 0; i < n; i++)
            {
                range[i] = current;
                skip(1, ref current, ref next);
            }

            return range;
        }

        private void skip(int n, ref int current, ref int next)
        {
            for (var i = 0; i < n; i++)
            {
                var last = current;
                current = next;
                next = current + last;
            }

        }
    }
}