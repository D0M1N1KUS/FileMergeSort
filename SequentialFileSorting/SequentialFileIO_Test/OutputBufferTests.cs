using FileIO.Interfaces;
using FileIO.RecordIO;
using FileIO.RecordIO.Interfaces;
using NSubstitute;
using NUnit.Framework;
using SequentialFileIO;

namespace SequentialFileIO_Test
{
    [TestFixture]
    public class OutputBufferTests
    {
        [Test]
        public void addRecordsToOutputBuffer_RecordsAreAddedToFormThreeSeries_NumberOfCountedSeriesShouldBeThree()
        {
            var records = new IRecord[]
            {
                new Record(new double[] {3}),
                new Record(new double[] {2}),
                new Record(new double[] {2}),
                new Record(new double[] {1}),
                new Record(new double[] {1}),
                new Record(new double[] {1}),
                new Record(new double[] {1})
            };
            var outputBuffer = new OutputBuffer {Appender = Substitute.For<IRecordAppender>()};

            foreach (var record in records)
            {
                outputBuffer.AppendRecord(record);
            }
            
            Assert.AreEqual(3, outputBuffer.Series);
            Assert.AreEqual(0, outputBuffer.DummyRecords);
        }

        [Test]
        public void addOneRecord_BufferShouldContainOneSeries()
        {
            var outputBuffer = new OutputBuffer() {Appender = Substitute.For<IRecordAppender>()};
            
            outputBuffer.AppendRecord(new double[]{1});
            
            Assert.AreEqual(1, outputBuffer.Series);
        }

        [Test]
        public void addThreeDummyRecords_ExpectedNumberOfSeriesShouldBeThree()
        {
            var outputBuffer = new OutputBuffer() {Appender = Substitute.For<IRecordAppender>()};
            
            outputBuffer.AddDummyRecord(3);
            
            Assert.AreEqual(3, outputBuffer.DummyRecords);
            Assert.AreEqual(3, outputBuffer.Series);
        }

        [Test]
        public void addThreeRecordsAndThreeDummyRecords_DummyRecordsAreExpectedToGenerateTheirOwnTwoSeries()
        {
            var outputBuffer = new OutputBuffer() {Appender = Substitute.For<IRecordAppender>()};
            
            outputBuffer.AppendRecord(new double[]{3});
            outputBuffer.AppendRecord(new double[]{2});
            outputBuffer.AppendRecord(new double[]{1});
            outputBuffer.AddDummyRecord();
            outputBuffer.AddDummyRecord(2);
            
            Assert.AreEqual(6, outputBuffer.Series);
            Assert.AreEqual(3, outputBuffer.DummyRecords);
        }
    }
}