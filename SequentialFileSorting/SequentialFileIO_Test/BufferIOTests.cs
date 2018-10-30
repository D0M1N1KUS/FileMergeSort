using FileIO.RecordIO;
using NSubstitute;
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

            var soucreInputBuffer = Substitute.For<IInputBuffer>();
            soucreInputBuffer.GetNextRecord().Returns(goodRecord);
            var sourceOutputBuffer = Substitute.For<IOutputBuffer>();
            sourceOutputBuffer.LastAppendedRecord.Returns(goodRecord);

            var tempInputBuffers = new IInputBuffer[3];
            var tempOutputBuffers = new IOutputBuffer[3];
            for (var i = 0; i < 3; i++)
            {
                tempInputBuffers[i] = Substitute.For<IInputBuffer>();
                tempInputBuffers[i].GetNextRecord().Returns(badRecord);
                tempOutputBuffers[i] = Substitute.For<IOutputBuffer>();
                tempOutputBuffers[i].LastAppendedRecord.Returns(badRecord);
            }

            var fileBufferIO = new FileBufferIo(4, soucreInputBuffer, sourceOutputBuffer, tempInputBuffers,
                tempOutputBuffers);

            Assert.AreEqual(fileBufferIO.GetNextFromCurrentInputBuffer(), goodRecord);
        }
    }
}