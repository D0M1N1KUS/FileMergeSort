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
        public void distributeRecordsInFile_TheNumberOfRecordsCanBeEvenlyDistributedNotRequiringDummyRecords()
        {
            var fileBufferIO = Substitute.For<IDistributionBufferingIO>();
            fileBufferIO.InputBufferHasNext().Returns(true, true, true, true, true, false, false, false, false);
            fileBufferIO.GetOutputBuffer(0).Series.Returns(1, 1, 2);
            fileBufferIO.GetOutputBuffer(1).Series.Returns(1, 2, 3);
            
            var distributor = new DynamicDistribution(2, fileBufferIO, new FibonacciSequenceGenerator());
            distributor.Distribute();
            
            fileBufferIO.Received(2).WriteNextSeriesToBuffer(0);
            fileBufferIO.Received(3).WriteNextSeriesToBuffer(1);
        }
    }
}