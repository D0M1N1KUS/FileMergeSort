using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using FileIO.Interfaces;
using FileIO.RecordIO.Interfaces;
using FileIO.Writers.Interfaces;

namespace FileIO.RecordIO
{
    public struct Record : IRecord
    {
        public double Value
        {
            get
            {
                var largest = double.MinValue;
                foreach (var component in valueComponents)
                {
                    if (largest < component) largest = component;
                }

                return largest;
            }
        }
        public int Length => valueComponents.Length;
        IRecord IRecord.Min => Min;
        IRecord IRecord.Max => Max;
        IRecord IRecord.Dummy => Dummy;

        IRecord IRecord.NullRecord => NullRecord;

        public bool IsDummy { get; private set; }
        public bool IsNull { get; private set; }
        public string[] ValueComponentsArray => valueComponents.Select(value => value.ToString()).ToArray();

        public static IRecord Min => new Record(new double[0]);
        public static IRecord Max => new Record(new double[15]
        {
            double.MaxValue, double.MaxValue, double.MaxValue, double.MaxValue, double.MaxValue, double.MaxValue,
            double.MaxValue, double.MaxValue, double.MaxValue, double.MaxValue, double.MaxValue, double.MaxValue,
            double.MaxValue, double.MaxValue, double.MaxValue
        });
        public static IRecord Dummy => new Record(new double[0], true);
        public static IRecord NullRecord => new Record(new double[0], false, true);
        
        private readonly double[] valueComponents;

        public Record(double[] valueComponents)
        {
            if (valueComponents.Length > 15)
                throw new Exception("Record creation error: A record can contain a maximum of 15 values. Number of values passed: " +
                                    valueComponents.Length);
                
            this.valueComponents = valueComponents;
            IsDummy = false;
            IsNull = false;
        }
        
        private Record(double[] valueComponents, bool isDummy = false, bool isNull = false)
        {
            if (valueComponents.Length > 15)
                throw new Exception("Record creation error: A record can contain a maximum of 15 values. Number of values passed: " +
                                    valueComponents.Length);
                
            this.valueComponents = valueComponents;
            IsDummy = isDummy;
            IsNull = isNull;
        }

        public Record(string[] valueComponents, bool isDummy = false, bool isNull = false)
        {
            if (valueComponents.Length > 15)
                throw new Exception("A record can contain a maximum of 15 values. Number of values passed: " +
                                    valueComponents.Length);

            var parsedValueComponents = new double[valueComponents.Length];
            for(var i = 0; i < valueComponents.Length; i++)
            {
                double o;
                if(!double.TryParse(valueComponents[i], out o))
                    throw new Exception("Unable to parse value: \"" + valueComponents[i] + "\" to double.");
                parsedValueComponents[i] = double.Parse(valueComponents[i]);
            }

            this.valueComponents = parsedValueComponents;
            IsDummy = isDummy;
            IsNull = isNull;
        }
        
        public string ValueComponentsString(string separator)
        {
            return string.Join(separator, ValueComponentsArray);
        }

        public int CompareTo(IRecord other)
        {
            if (other == null)
                return 1;
            if (other.IsDummy || other.IsNull)
            {
                return 1;
            }

            if (IsDummy || IsNull)
            {
                return -1;
            }
            if (other.Equals(this))
            {
                if (other.Length == this.Length)
                    return 0;
                if (other.Length > this.Length)
                    return -1;
                return 1;
            }

            if (other.Value > this.Value)
                return -1;
            return 1;
        }

        public static bool operator <(Record r1, IRecord r2)
        {
            return r1.CompareTo(r2) < 0;
        }
        
        public static bool operator >(Record r1, IRecord r2)
        {
            return r1.CompareTo(r2) > 0;
        }

        public override bool Equals(object obj)
        {
            var record = obj as IRecord;
            if (record == null)
                return false;
            return record.Value.Equals(Value) &&
                   record.IsDummy == IsDummy &&
                   record.IsNull == IsNull;
        }

        public new string ToString()
        {
            return string.Join(" ", valueComponents);
        }
    }
}