using System;
using FileIO.RecordIO;
using NUnit.Framework;

namespace FileIO_Tests
{
    [TestFixture]
    public class RecordTests
    {
        [Test]
        public void createRecordFromStringArray()
        {
            var valueComponents = new string[] {"0", "1,0", "0,1"};
            var expectedRecordValue = 1.0;

            var record = new Record(valueComponents);
            
            Assert.AreEqual(expectedRecordValue, record.Value);
        }

        [Test]
        public void createRecordFromDoubleArray()
        {
            var valueComponents = new [] {0, 1.0, 0.5};
            var expectedRecordValue = 1.0;

            var record = new Record(valueComponents);
            
            Assert.AreEqual(expectedRecordValue, record.Value);
        }

        [Test]
        public void createRecordFromEmptyStringArray_ValueShouldBeDoubleMin()
        {
            var valueComponents = new string[0];
            
            var record = new Record(valueComponents);

            Assert.AreEqual(double.MinValue, record.Value);
        }
        
        [Test]
        public void createRecordFromEmptyDoubleArray_ValueShouldBeDoubleMin()
        {
            var valueComponents = new double[0];

            var record = new Record(valueComponents);

            Assert.AreEqual(double.MinValue, record.Value);
        }
        
        [Test]
        public void createRecordFromTooLargeStringArray_shouldThrowException()
        {
            var valueComponents = new[] {"1,0", "1,0", "1,0", "1,0", "1,0", "1,0", "1,0", "1,0", "1,0", "1,0", "1,0", 
                "1,0", "1,0", "1,0", "1,0", "1,0",};
            
            Assert.Throws<Exception>(() => new Record(valueComponents));
        }

        [Test]
        public void createRecordFromTooLargeDoubleArray_shouldThrowException()
        {
            var valueComponents = new[] {1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 
                1.0,};
            
            Assert.Throws<Exception>(() => new Record(valueComponents));
        }

        [Test]
        public void compareRecords_ValuesOfRecordsAreDifferent_RecordOneShouldBeSmaller()
        {
            var recordOne = new Record(new double[] {1, 2, 3, 4, 5});
            var recordTwo = new Record(new double[] {1, 2, 3, 4, 6});
            
            Assert.IsTrue(recordOne < recordTwo);
        }

        [Test]
        public void
            compareRecords_ValuesOfRecordsAreTheSameNumberOfComponentValuesAreDifferent_RecordOneShouldBeSmaller()
        {
            var recordOne = new Record(new double[] {1, 5});
            var recordTwo = new Record(new double[] {1, 2, 3, 4, 5});
            
            Assert.IsTrue(recordOne < recordTwo);
        }

        [Test]
        public void compareRecords_NumberOfValuesAreDifferent_BothShouldBeEqual()
        {
            var recordOne = new Record(new double[] {1, 5});
            var recordTwo = new Record(new double[] {1, 2, 3, 4, 5});
            
            Assert.AreEqual(recordOne, recordTwo);
        }

        [Test]
        public void getValueComponentsAsArray()
        {
            var expectedValueComponents = new string[] {"1,1", "2,2", "4,4", "3,3", "5,5"};
            var record = new Record(expectedValueComponents);

            var actualValueComponents = record.ValueComponentsArray;
            
            CollectionAssert.AreEqual(expectedValueComponents, actualValueComponents);
        }

        [Test]
        public void getValueComponentsAsString()
        {
            var valueComponents = new string[] {"1,1", "2,2", "4,4", "3,3", "5,5"};
            var record = new Record(valueComponents);
            var expectedValueComponentsString = "1,1 2,2 4,4 3,3 5,5";

            var actualValueComponentsString = record.ValueComponentsString(" ");
            
            Assert.AreEqual(expectedValueComponentsString, actualValueComponentsString);
        }
    }
}