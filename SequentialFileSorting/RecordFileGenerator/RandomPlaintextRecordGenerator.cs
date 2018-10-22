using System;
using System.Linq;
using FileIO.RecordIO;
using FileIO.RecordIO.Interfaces;
using RecordFileGenerator.Interfaces;

namespace RecordFileGenerator
{
    public class RandomPlaintextRecordGenerator : IRandomRecordGenerator
    {
        public IPseudoRNG Randomizer;

        public RandomPlaintextRecordGenerator(IPseudoRNG randomizer = null, int? seed = null)
        {
            Randomizer = randomizer ?? new PseudoRNG(seed ?? DateTime.Now.Millisecond);
        }
        
        public IRecord GenerateRandomRecord()
        {
            return new Record(GenerateRandomRecordValuesDoubleArray());
        }
        
        public string[] GenerateRandomRecordValuesStringArray()
        {
            return GenerateRandomRecordValuesDoubleArray().Select(value => value.ToString()).ToArray();
        }

        public string GenerateRandomRecordValuesString(string separator)
        {
            return string.Join(separator, GenerateRandomRecordValuesDoubleArray());
        }

        public double[] GenerateRandomRecordValuesDoubleArray()
        {
            var numberOfValuesInRecord = Randomizer.GetInt(0, 15);
            var values = new double[numberOfValuesInRecord];
            for (var i = 0; i < numberOfValuesInRecord; i++)
            {
                values[i] = Randomizer.GetDouble();
            }

            return values;
        }
    }
}