using FileIO.RecordIO.Interfaces;
using NSubstitute;
using NUnit.Framework;
using SequentialFileIO;
using SequentialFileSorting.Sorting;

namespace SequentialFileIO_Test
{
    [TestFixture]
    public class DynamicDistributionTests
    {
        [Test]
        [Ignore("Can't modify returning values for index accessors")]
        public void distributeRecordsInFile_TheNumberOfRecordsCanBeEvenlyDistributedNotRequiringDummyRecords()
        {
            var fileBufferIO = Substitute.For<IFileBufferIO>();
            fileBufferIO.InputBufferHasNext().Returns(true, true, true, true, true, true, true, true, true, true, 
                true, true, true, true, true, false, false, false, false);
            var recordArray = new IRecord[]
            {
                getSubstituteRecordWithValue(5),
                getSubstituteRecordWithValue(4),
                getSubstituteRecordWithValue(3),
                getSubstituteRecordWithValue(2),
                getSubstituteRecordWithValue(1)
            };
            fileBufferIO.GetNextFromCurrentInputBuffer().Returns(
                recordArray[0], recordArray[1], recordArray[2], recordArray[3], recordArray[4]);
            var distributor = new DynamicDistribution(2, fileBufferIO, new FibonacciSequenceGenerator());
            distributor.Distribute();
            
            fileBufferIO.Received().AppendToOutputBuffer(0, recordArray[0]);
            fileBufferIO.Received().AppendToOutputBuffer(1, recordArray[1]);
            fileBufferIO.Received().AppendToOutputBuffer(0, recordArray[2]);
            fileBufferIO.Received().AppendToOutputBuffer(0, recordArray[3]);
            fileBufferIO.Received().AppendToOutputBuffer(1, recordArray[4]);
        }

        private IFileBufferIO getSampleFileBufferIO()
        {
            var fileBufferIO = Substitute.For<IFileBufferIO>();
            fileBufferIO.InputBufferHasNext().Returns(true, true, true, true, true, true, true, true, true, true, 
                true, true, true, true, true, false, false, false, false);
            fileBufferIO.GetOutputBuffer(0).Series.Returns(1, 1, 2);
            fileBufferIO.GetOutputBuffer(1).Series.Returns(1, 2, 3);
            
            return fileBufferIO;
        }

        private IRecord getSubstituteRecordWithValue(int value)
        {
            var record = Substitute.For<IRecord>();
            record.Value.Returns(value);
            return record;
        }
    }
}