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

        public PseudoRNG()
        {
            random = new Random((int)DateTime.Now.Ticks);
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

        public bool GetBool(double chanceForTrue = 0.5)
        {
            return GetDouble() >= 1 - chanceForTrue;
        }
    }
}