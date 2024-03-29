using System;
using FileIO.RecordIO;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NUnit.Framework;
using SequentialFileIO;

namespace SequentialFileIO_Test
{
    [TestFixture]
    public class BufferIOTests
    {
        [Test]
        public void getNextRecordValueFromSourceInputBuffer()
        {
            var goodRecord = new Record(new double[] {1, 1, 1,});
            var badRecord = new Record(new double[] {0, 0, 0});

            var sourceInputBuffer = Substitute.For<IInputBuffer>();
            sourceInputBuffer.GetNextRecord().Returns(goodRecord);
            sourceInputBuffer.HasNext().Returns(true);
            sourceInputBuffer.RemoveDummyRecord().Throws(new Exception("No dummy records should be removed here!"));
            var sourceOutputBuffer = Substitute.For<IOutputBuffer>();
            sourceOutputBuffer.LastAppendedRecord.Returns(goodRecord);

            var tempInputBuffers = new IInputBuffer[4];
            var tempOutputBuffers = new IOutputBuffer[4];
            for (var i = 0; i < 3; i++)
            {
                tempInputBuffers[i] = Substitute.For<IInputBuffer>();
                tempInputBuffers[i].GetNextRecord().Returns(badRecord);
                tempInputBuffers[i].HasNext().Returns(true);
                tempOutputBuffers[i] = Substitute.For<IOutputBuffer>();
                tempOutputBuffers[i].LastAppendedRecord.Returns(badRecord);
            }

            tempInputBuffers[3] = sourceInputBuffer;
            tempOutputBuffers[3] = sourceOutputBuffer;

            var fileBufferIO = new DistributionBufferingIO(ref tempInputBuffers, ref tempOutputBuffers, 3);

            var actualRecord = fileBufferIO.GetNextFromCurrentInputBuffer();
            Assert.AreEqual(false, actualRecord.IsDummy);
            Assert.AreEqual(goodRecord.Value, actualRecord.Value);
        }
    }
}