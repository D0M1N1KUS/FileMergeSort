using System;
using RecordFileGenerator.Interfaces;

namespace RecordFileGenerator
{
    public class PseudoRNG : IPseudoRNG
    {
        private Random random;

        public PseudoRNG(int seed)
        {
            random = new Random(seed);
        }
        
        public double GetDouble()
        {
            return random.NextDouble();
        }

        public int GetInt()
        {
            return random.Next();
        }

        public int GetInt(int from, int to)
        {
            return random.Next(from, to);
        }
    }
}