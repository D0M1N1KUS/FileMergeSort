using System;
using FileIO.RecordIO;
using FileIO.RecordIO.Interfaces;
using NUnit.Framework;
using SequentialFileSorting.Sorting;

namespace SequentialFileIO_Test
{
    [TestFixture]
    public class MinRecordValueComparerTests
    {
        [Test]
        public void getSmallestRecordAndIndex_UsingParamsMethod()
        {
            IRecord expectedSmallestRecord = Record.Min;
            var expectedIndex = 4;
            var comparer = new MinRecordValueComparer();
            
            var actualIndex = comparer.GetIndexOfSmallest(
                new Record(new double[] {5}), new Record(new double[] {4}), new Record(new double[] {3}),
                new Record(new double[] {2}), expectedSmallestRecord, new Record(new double[] {1}));
            var actualSmallestRecord = comparer.SmallestRecord;
            
            Assert.AreEqual(expectedIndex, actualIndex);
            Assert.AreEqual(expectedSmallestRecord, actualSmallestRecord);
        }

        [Test]
        public void getSmallestRecordAndIndex_UsingComparisonListMethod()
        {
            IRecord expectedSmallestRecord = Record.Min;
            var expectedIndex = 4;
            var comparer = new MinRecordValueComparer();
            comparer.AddRecordToComparison(new Record(new double[] {5}));
            comparer.AddRecordToComparison(new Record(new double[] {4}));
            comparer.AddRecordToComparison(new Record(new double[] {3}));
            comparer.AddRecordToComparison(new Record(new double[] {2}));
            comparer.AddRecordToComparison(expectedSmallestRecord);
            comparer.AddRecordToComparison(new Record(new double[] {1}));

            var actualIndex = comparer.GetIndexOfSmallest();
            var actualSmallestRecord = comparer.SmallestRecord;
            
            Assert.AreEqual(expectedIndex, actualIndex);
            Assert.AreEqual(expectedSmallestRecord, actualSmallestRecord);
        }

        [Test]
        public void getSmallestRecordAndIndex_ModifyingComparisonList()
        {
            IRecord expectedSmallestRecord = Record.Min;
            var expectedIndex = 4;
            var comparer = new MinRecordValueComparer();
            comparer.AddRecordToComparison(new Record(new double[] {5}));
            comparer.AddRecordToComparison(new Record(new double[] {4}));
            comparer.AddRecordToComparison(new Record(new double[] {3}));
            comparer.AddRecordToComparison(new Record(new double[] {2}));            

            var previousSmallestIndex = comparer.GetIndexOfSmallest();
            var previousSmallest = comparer.SmallestRecord;
            comparer.AddRecordToComparison(expectedSmallestRecord);
            comparer.AddRecordToComparison(new Record(new double[] {1}));
            var actualIndex = comparer.GetIndexOfSmallest();
            var actualSmallestRecord = comparer.SmallestRecord;
            
            Assert.AreNotEqual(previousSmallestIndex, actualIndex);
            Assert.AreNotEqual(previousSmallest, actualSmallestRecord);
            Assert.AreEqual(expectedIndex, actualIndex);
            Assert.AreEqual(expectedSmallestRecord, actualSmallestRecord);
        }

        [Test]
        public void getSmallestRecordAndIndex_ListContainsDummyRecord_DummyRecordsShouldBeIgnored()
        {
            IRecord expectedSmallestRecord = new Record(new double[]{10000});
            var expectedIndex = 4;
            var comparer = new MinRecordValueComparer();
            comparer.AddRecordToComparison(Record.Dummy);
            comparer.AddRecordToComparison(Record.Dummy);
            comparer.AddRecordToComparison(Record.Dummy);
            comparer.AddRecordToComparison(Record.Dummy);
            comparer.AddRecordToComparison(expectedSmallestRecord);
            comparer.AddRecordToComparison(Record.Dummy);

            var actualIndex = comparer.GetIndexOfSmallest();
            var actualSmallestRecord = comparer.SmallestRecord;
            
            Assert.AreEqual(expectedIndex, actualIndex);
            Assert.AreEqual(expectedSmallestRecord, actualSmallestRecord);
        }
        
        [Test]
        public void getSmallestRecordAndIndex_ListContainsNullRecords_NullRecordsShouldBeIgnored()
        {
            IRecord expectedSmallestRecord = new Record(new double[]{10000});
            var expectedIndex = 4;
            var comparer = new MinRecordValueComparer();
            comparer.AddRecordToComparison(Record.NullRecord);
            comparer.AddRecordToComparison(Record.NullRecord);
            comparer.AddRecordToComparison(Record.NullRecord);
            comparer.AddRecordToComparison(Record.NullRecord);
            comparer.AddRecordToComparison(expectedSmallestRecord);
            comparer.AddRecordToComparison(Record.NullRecord);

            var actualIndex = comparer.GetIndexOfSmallest();
            var actualSmallestRecord = comparer.SmallestRecord;
            
            Assert.AreEqual(expectedIndex, actualIndex);
            Assert.AreEqual(expectedSmallestRecord, actualSmallestRecord);
        }
        
        [Test]
        public void getSmallestRecordAndIndex_ListContainsNullAndDummyRecords_OnlyNormalRecordsShouldBeConsidered()
        {
            IRecord expectedSmallestRecord = new Record(new double[]{10000});
            var expectedIndex = 4;
            var comparer = new MinRecordValueComparer();
            comparer.AddRecordToComparison(Record.NullRecord);
            comparer.AddRecordToComparison(Record.Dummy);
            comparer.AddRecordToComparison(Record.NullRecord);
            comparer.AddRecordToComparison(Record.Dummy);
            comparer.AddRecordToComparison(expectedSmallestRecord);
            comparer.AddRecordToComparison(Record.NullRecord);

            var actualIndex = comparer.GetIndexOfSmallest();
            var actualSmallestRecord = comparer.SmallestRecord;
            
            Assert.AreEqual(expectedIndex, actualIndex);
            Assert.AreEqual(expectedSmallestRecord, actualSmallestRecord);
        }

        [Test]
        public void getSmallestRecordAndIndex_ListContainsOnlyNullAndDummyRecords_FirstDummyRecordShouldBeSmallest()
        {
            var expectedSmallestRecord = Record.Dummy;
            var expectedIndex = 2;
            var comparer = new MinRecordValueComparer();
            comparer.AddRecordToComparison(Record.NullRecord);
            comparer.AddRecordToComparison(Record.NullRecord);
            comparer.AddRecordToComparison(expectedSmallestRecord);
            comparer.AddRecordToComparison(Record.Dummy);
            comparer.AddRecordToComparison(Record.NullRecord);
            comparer.AddRecordToComparison(Record.Dummy);
            comparer.AddRecordToComparison(Record.NullRecord);

            var actualIndex = comparer.GetIndexOfSmallest();
            var actualSmallestRecord = comparer.SmallestRecord;
            
            Assert.AreEqual(expectedIndex, actualIndex);
            Assert.AreEqual(expectedSmallestRecord, actualSmallestRecord);
        }

        [Test]
        public void getSmallestRecord_ListContainsOnlyNullRecords_ComparerShouldThrowException()
        {
            var comparer = new MinRecordValueComparer();
            comparer.AddRecordToComparison(Record.NullRecord);
            comparer.AddRecordToComparison(Record.NullRecord);
            comparer.AddRecordToComparison(Record.NullRecord);

            Assert.Throws<Exception>(() => comparer.GetIndexOfSmallest());
        }
    }
}