using FileIO.RecordIO;
using NSubstitute;
using NUnit.Framework;
using SequentialFileIO;

namespace SequentialFileIO_Test
{
    [TestFixture]
    public class DistributionBufferingTests
    {
        [Test]
        public void GetInputBuffer_OutputBufferIsAtIndex0_InputBufferWithIndex0ShouldNotBeReturned()
        {
            var expectedRecord = Record.Max;
            IInputBuffer[] inputBuffers;
            IOutputBuffer[] outputBuffers;
            getIOBuffers(out inputBuffers, out outputBuffers, 3);

            outputBuffers[0].LastAppendedRecord.Returns(expectedRecord);
            outputBuffers[1].LastAppendedRecord.Returns(expectedRecord);
            outputBuffers[2].LastAppendedRecord.Returns(Record.NullRecord);
            
            var distributionBuffering = new DistributionBufferingIO(2, ref inputBuffers[2], ref outputBuffers[2],
                ref inputBuffers, ref outputBuffers);
            
            Assert.AreEqual(expectedRecord, distributionBuffering.GetOutputBuffer(0).LastAppendedRecord);
            Assert.AreEqual(expectedRecord, distributionBuffering.GetOutputBuffer(1).LastAppendedRecord);
        }
        
        [Test]
        public void GetInputBuffer_OutputBufferIsAtLastIndex_InputBufferWithLastIndexShouldNotBeReturned()
        {
            var expectedRecord = Record.Max;
            IInputBuffer[] inputBuffers;
            IOutputBuffer[] outputBuffers;
            getIOBuffers(out inputBuffers, out outputBuffers, 3);

            outputBuffers[0].LastAppendedRecord.Returns(expectedRecord);
            outputBuffers[1].LastAppendedRecord.Returns(Record.NullRecord);
            outputBuffers[2].LastAppendedRecord.Returns(expectedRecord);
            
            var distributionBuffering = new DistributionBufferingIO(2, ref inputBuffers[2], ref outputBuffers[2],
                ref inputBuffers, ref outputBuffers);
            distributionBuffering.SwitchToNextOutputBuffer();
            distributionBuffering.SwitchToNextOutputBuffer();
            
            Assert.AreEqual(expectedRecord, distributionBuffering.GetOutputBuffer(0).LastAppendedRecord);
            Assert.AreEqual(expectedRecord, distributionBuffering.GetOutputBuffer(1).LastAppendedRecord);
        }

        [Test]
        public void appendSeriesToOutputBuffer_ThereAreThreeSeries_NoDummySeries()
        {
            IInputBuffer[] inputBuffers;
            IOutputBuffer[] outputBuffers;
            getIOBuffers(out inputBuffers, out outputBuffers, 3);
            var records = new[]
            {
                new Record(new double[] {3}),
                new Record(new double[] {2}),
                new Record(new double[] {1})
            };
            
            inputBuffers[2].HasNext().Returns(true, true, true, true, true, true, false);
            inputBuffers[2].GetNextRecord().Returns(records[0], records[1], records[2]);
            inputBuffers[2].HasDummy().Returns(false);
            
            var distributionBuffering = new DistributionBufferingIO(2, ref inputBuffers[2], ref outputBuffers[2],
                ref inputBuffers, ref outputBuffers);
            distributionBuffering.WriteNextSeriesToBuffer(0);
            distributionBuffering.WriteNextSeriesToBuffer(0);
            distributionBuffering.WriteNextSeriesToBuffer(0);

            inputBuffers[2].Received(3).GetNextRecord();
            outputBuffers[0].Received().AppendRecord(records[0]);
            outputBuffers[0].Received().AppendRecord(records[1]);
            outputBuffers[0].Received().AppendRecord(records[2]);
        }
        
        
        private void getIOBuffers(out IInputBuffer[] inputBuffers, out IOutputBuffer[] outputBuffers,
            int numberOfBuffers)
        {
            inputBuffers = new IInputBuffer[numberOfBuffers];
            outputBuffers = new IOutputBuffer[numberOfBuffers];
            for (var i = 0; i < numberOfBuffers; i++)
            {
                inputBuffers[i] = Substitute.For<IInputBuffer>();
                outputBuffers[i] = Substitute.For<IOutputBuffer>();
            }
        }
    }
}