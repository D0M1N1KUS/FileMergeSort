using System;
using System.Runtime.InteropServices;
using FileIO.RecordIO;
using NSubstitute;
using NUnit.Framework;
using SequentialFileIO;

namespace SequentialFileIO_Test
{
    [TestFixture]
    public class MergeBufferingIOTests
    {
        [Test]
        [Ignore("NSubstitute can't return stuff from indexing access")]
        public void DONT_DO_THIS()
        {
            var inputBuffers = Substitute.For<IInputBuffer[]>();
            inputBuffers[0].HasNext().Returns(true, true, true, true, true, false);
            inputBuffers[1].HasNext().Returns(true, true, true, false);

            inputBuffers[0].HasNext();
            inputBuffers[1].HasNext();
            var second = new[] {inputBuffers[0].HasNext(), inputBuffers[1].HasNext()};
            inputBuffers[0].HasNext(); inputBuffers[0].HasNext(); inputBuffers[0].HasNext();
            inputBuffers[1].HasNext();
            var last = new[] {inputBuffers[0].HasNext(), inputBuffers[1].HasNext()};

            var expectedSecond = new[] {true, true};
            var expectedLast = new[] {false, false};
            
            CollectionAssert.AreEqual(expectedSecond, second);    // Throws TypeLoadException
            CollectionAssert.AreEqual(expectedLast, last);    //This too :/
        }

        [Test]
        public void SetAnyEmptyBufferAsDestinationBuffer_ShouldReturnDesiredBuffer_SelectedBufferHasIndex0()
        {
            var numberOfBuffers = 3;
            IInputBuffer[] inputBuffers;
            IOutputBuffer[] outputBuffers;
            getIOBuffers(out inputBuffers, out outputBuffers, 3);
            inputBuffers[0].HasNext().Returns(true);
            inputBuffers[1].HasNext().Returns(true);
            inputBuffers[2].HasNext().Returns(false);
            inputBuffers[0].HasDummy().Returns(false);
            inputBuffers[1].HasDummy().Returns(false);
            inputBuffers[2].HasDummy().Returns(false);
            var mergeBuffering = new MergeBufferingIO(ref inputBuffers, ref outputBuffers, 0);
            
            var expectedSelectedBuffer = 2;
            mergeBuffering.SetAnyEmptyBufferAsDestinationBuffer();
            var actualSelectedBuffer = mergeBuffering.GetDestinationBufferIndex();
            
            Assert.AreEqual(expectedSelectedBuffer, actualSelectedBuffer);
        }
        
        [Test]
        public void SetAnyEmptyBufferAsDestinationBuffer_ShouldReturnDesiredBuffer_MultipleBuffersAreEmpty()
        {
            var numberOfBuffers = 3;
            IInputBuffer[] inputBuffers;
            IOutputBuffer[] outputBuffers;
            getIOBuffers(out inputBuffers, out outputBuffers, 4);
            inputBuffers[0].HasNext().Returns(true);
            inputBuffers[1].HasNext().Returns(true);
            inputBuffers[2].HasNext().Returns(false);
            inputBuffers[3].HasNext().Returns(false);
            inputBuffers[0].HasDummy().Returns(false);
            inputBuffers[1].HasDummy().Returns(false);
            inputBuffers[2].HasDummy().Returns(false);
            inputBuffers[3].HasDummy().Returns(false);
            var mergeBuffering = new MergeBufferingIO(ref inputBuffers, ref outputBuffers, 1);
            
            var expectedSelectedBuffer = 3;
            mergeBuffering.SetAnyEmptyBufferAsDestinationBuffer();
            var actualSelectedBuffer = mergeBuffering.GetDestinationBufferIndex();
            
            Assert.AreEqual(expectedSelectedBuffer, actualSelectedBuffer);
        }

        [Test]
        public void GetInputBuffer_DestinationBufferIsAtIndex0_InputBufferWithIndex0ShouldNotBeReturned()
        {
            var expectedRecord = Record.Max;
            IInputBuffer[] inputBuffers;
            IOutputBuffer[] outputBuffers;
            getIOBuffers(out inputBuffers, out outputBuffers, 3);
            inputBuffers[0].HasNext().Returns(false);
            inputBuffers[1].HasNext().Returns(true);
            inputBuffers[2].HasNext().Returns(true);
            inputBuffers[0].GetNextRecord().Returns(Record.NullRecord);
            inputBuffers[1].GetNextRecord().Returns(expectedRecord);
            inputBuffers[2].GetNextRecord().Returns(expectedRecord);
            
            var mergeBuffering = new MergeBufferingIO(ref inputBuffers, ref outputBuffers, 0);
            
            Assert.AreEqual(expectedRecord, mergeBuffering.GetInputBuffer(0).GetNextRecord());
            Assert.AreEqual(expectedRecord, mergeBuffering.GetInputBuffer(1).GetNextRecord());
        }
        
        [Test]
        public void GetInputBuffer_DestinationBufferIsAtTheEndList_InputBufferWithLastIndexShouldNotBeReturned()
        {
            var expectedRecord = Record.Max;
            IInputBuffer[] inputBuffers;
            IOutputBuffer[] outputBuffers;
            getIOBuffers(out inputBuffers, out outputBuffers, 3);
            inputBuffers[0].HasNext().Returns(true);
            inputBuffers[1].HasNext().Returns(true);
            inputBuffers[2].HasNext().Returns(false);
            inputBuffers[0].GetNextRecord().Returns(expectedRecord);
            inputBuffers[1].GetNextRecord().Returns(expectedRecord);
            inputBuffers[2].GetNextRecord().Returns(Record.NullRecord);
            
            var mergeBuffering = new MergeBufferingIO(ref inputBuffers, ref outputBuffers, 2);
            
            Assert.AreEqual(expectedRecord, mergeBuffering.GetInputBuffer(0).GetNextRecord());
            Assert.AreEqual(expectedRecord, mergeBuffering.GetInputBuffer(1).GetNextRecord());
        }
        
        [Test]
        public void SetAnyEmptyBufferAsDestinationBuffer_ShouldThrowException_NoBufferIsEmpty()
        {
            var numberOfBuffers = 3;
            IInputBuffer[] inputBuffers;
            IOutputBuffer[] outputBuffers;
            getIOBuffers(out inputBuffers, out outputBuffers, 2);
            inputBuffers[0].HasNext().Returns(true);
            inputBuffers[1].HasNext().Returns(true);
            inputBuffers[0].HasDummy().Returns(true);
            inputBuffers[1].HasDummy().Returns(true);
            var mergeBuffering = new MergeBufferingIO(ref inputBuffers, ref outputBuffers, 0);

            Assert.Throws<Exception>(() => mergeBuffering.SetAnyEmptyBufferAsDestinationBuffer());
        }
        
        [Test]
        public void GetInputBuffer_DestinationBufferIsMidList_InputBufferWithMidIndexShouldNotBeReturned()
        {
            var expectedRecord = Record.Max;
            IInputBuffer[] inputBuffers;
            IOutputBuffer[] outputBuffers;
            getIOBuffers(out inputBuffers, out outputBuffers, 4);
            inputBuffers[0].HasNext().Returns(true);
            inputBuffers[1].HasNext().Returns(true);
            inputBuffers[2].HasNext().Returns(false);
            inputBuffers[3].HasNext().Returns(true);
            inputBuffers[0].GetNextRecord().Returns(expectedRecord);
            inputBuffers[1].GetNextRecord().Returns(expectedRecord);
            inputBuffers[2].GetNextRecord().Returns(Record.NullRecord);
            inputBuffers[3].GetNextRecord().Returns(expectedRecord);
            
            var mergeBuffering = new MergeBufferingIO(ref inputBuffers, ref outputBuffers, 2);
            
            Assert.AreEqual(expectedRecord, mergeBuffering.GetInputBuffer(0).GetNextRecord());
            Assert.AreEqual(expectedRecord, mergeBuffering.GetInputBuffer(1).GetNextRecord());
            Assert.AreEqual(expectedRecord, mergeBuffering.GetInputBuffer(2).GetNextRecord());
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