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
            
            var actualIndex = comparer.GetIndexOfSmallest(new IRecord[]{ 
                new Record(new double[] {5}), new Record(new double[] {4}), new Record(new double[] {3}),
                new Record(new double[] {2}), expectedSmallestRecord, new Record(new double[] {1})},
                new bool[]{false, false, false, false, false, false});
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
            comparer.AddRecordToComparison(new Record(new double[] {5}), false);
            comparer.AddRecordToComparison(new Record(new double[] {4}), false);
            comparer.AddRecordToComparison(new Record(new double[] {3}), false);
            comparer.AddRecordToComparison(new Record(new double[] {2}), false);
            comparer.AddRecordToComparison(expectedSmallestRecord, false);
            comparer.AddRecordToComparison(new Record(new double[] {1}), false);

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
            comparer.AddRecordToComparison(new Record(new double[] {5}), false);
            comparer.AddRecordToComparison(new Record(new double[] {4}), false);
            comparer.AddRecordToComparison(new Record(new double[] {3}), false);
            comparer.AddRecordToComparison(new Record(new double[] {2}), false);

            var previousSmallestIndex = comparer.GetIndexOfSmallest();
            var previousSmallest = comparer.SmallestRecord;
            comparer.AddRecordToComparison(expectedSmallestRecord, false);
            comparer.AddRecordToComparison(new Record(new double[] {1}), false);
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
            comparer.AddRecordToComparison(Record.Dummy, false);
            comparer.AddRecordToComparison(Record.Dummy, false);
            comparer.AddRecordToComparison(Record.Dummy, false);
            comparer.AddRecordToComparison(Record.Dummy, false);
            comparer.AddRecordToComparison(expectedSmallestRecord, false);
            comparer.AddRecordToComparison(Record.Dummy, false);

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
            comparer.AddRecordToComparison(Record.NullRecord, false);
            comparer.AddRecordToComparison(Record.NullRecord, false);
            comparer.AddRecordToComparison(Record.NullRecord, false);
            comparer.AddRecordToComparison(Record.NullRecord, false);
            comparer.AddRecordToComparison(expectedSmallestRecord, false);
            comparer.AddRecordToComparison(Record.NullRecord, false);

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
            comparer.AddRecordToComparison(Record.NullRecord, false);
            comparer.AddRecordToComparison(Record.Dummy, false);
            comparer.AddRecordToComparison(Record.NullRecord, false);
            comparer.AddRecordToComparison(Record.Dummy, false);
            comparer.AddRecordToComparison(expectedSmallestRecord, false);
            comparer.AddRecordToComparison(Record.NullRecord, false);

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
            comparer.AddRecordToComparison(Record.NullRecord, false);
            comparer.AddRecordToComparison(Record.NullRecord, false);
            comparer.AddRecordToComparison(expectedSmallestRecord, false);
            comparer.AddRecordToComparison(Record.Dummy, false);
            comparer.AddRecordToComparison(Record.NullRecord, false);
            comparer.AddRecordToComparison(Record.Dummy, false);
            comparer.AddRecordToComparison(Record.NullRecord, false);

            var actualIndex = comparer.GetIndexOfSmallest();
            var actualSmallestRecord = comparer.SmallestRecord;
            
            Assert.AreEqual(expectedIndex, actualIndex);
            Assert.AreEqual(expectedSmallestRecord, actualSmallestRecord);
        }

        [Test]
        public void getSmallestRecord_ListContainsOnlyNullRecords_ComparerShouldThrowException()
        {
            var comparer = new MinRecordValueComparer();
            comparer.AddRecordToComparison(Record.NullRecord, false);
            comparer.AddRecordToComparison(Record.NullRecord, false);
            comparer.AddRecordToComparison(Record.NullRecord, false);

            Assert.Throws<Exception>(() => comparer.GetIndexOfSmallest());
        }

        [Test]
        public void getSmallestRecord_ListContainsValuesFromEndedSeries_ComparerShouldIgnoreValuesFromEndedSeries()
        {
            var comparer = new MinRecordValueComparer();
            comparer.AddRecordToComparison(new Record(new double[] {5}), false);
            comparer.AddRecordToComparison(new Record(new double[] {4}), false);
            comparer.AddRecordToComparison(new Record(new double[] {3}), false);
            comparer.AddRecordToComparison(new Record(new double[] {2}), true);
            var expectedIndex = 2;
            var expectedSmallestRecord = new Record(new double[]{3});
            
            Assert.AreEqual(expectedIndex, comparer.GetIndexOfSmallest());
            Assert.AreEqual(expectedSmallestRecord, comparer.SmallestRecord);
        }

        [Test]
        public void getSmallestRecord_ListOnlyContainsRecordsFromEndedSeries_ShouldThrowException()
        {
            var comparer = new MinRecordValueComparer();
            comparer.AddRecordToComparison(new Record(new double[] {5}), true);
            comparer.AddRecordToComparison(new Record(new double[] {4}), true);
            comparer.AddRecordToComparison(new Record(new double[] {3}), true);
            comparer.AddRecordToComparison(new Record(new double[] {2}), true);

            Assert.Throws<Exception>(() => comparer.GetIndexOfSmallest());
        }
    }
}