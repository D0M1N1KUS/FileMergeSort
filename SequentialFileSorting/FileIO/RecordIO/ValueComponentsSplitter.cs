using System.Collections.Generic;
using System.Linq;
using FileIO.RecordIO.Interfaces;

namespace FileIO.RecordIO
{
    public class ValueComponentsSplitter : IValueComponentsSplitter
    {
        public const string DEFAULT_SEPARATION_SYMBOL = " ";
        
        public string SeparationSymbol
        {
            set
            {
                sepatationSymbol = value.ToCharArray();
                separationString = value;
            }
            private get { return separationString ?? DEFAULT_SEPARATION_SYMBOL; }
        }

        private string separationString;
        private char[] sepatationSymbol;

        public ValueComponentsSplitter()
        {
            sepatationSymbol = SeparationSymbol.ToCharArray();
        }
        
        public string[] GetValues(string values)
        {
            var valuesArrayTemp = values.Split(sepatationSymbol);
            return valuesArrayTemp.Where(value => !string.IsNullOrEmpty(value)).ToArray();
        }

        public double[] GetDoubleValues(string values)
        {
            double o;
            return GetValues(values).Where(value => double.TryParse(value, out o))
                .Select(double.Parse)
                .ToArray();
        }
    }
}