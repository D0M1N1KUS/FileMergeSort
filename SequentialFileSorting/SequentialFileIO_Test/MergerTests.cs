using System.CodeDom;
using System.Collections;
using System.Diagnostics;
using FileIO.RecordIO;
using FileIO.RecordIO.Interfaces;
using NSubstitute;
using NUnit.Framework;
using SequentialFileIO;
using SequentialFileSorting.Sorting;

namespace SequentialFileIO_Test
{
    /*
     * NOTE: These tests are kinda pointless, because the most important logic is inside of the MergingBuffer
     */
    [TestFixture]
    public class MergerTests
    {
        /* Imaginary distribution:
         * TempBuffer1 = 7, 8, 3, 4, d;
         * TempBuffer2 = 5, 6, 1, 2;
         * TempBuffer3 = ;
         *
         * Step1.1
         * TempBuffer1 = 3, 4, d;
         * TempBuffer2 = 1, 2;
         * TempBuffer3 = 5, 6, 7, 8;
         *
         * Step1.2
         * TempBuffer1 = d;
         * TempBuffer2 = ;
         * TempBuffer3 = 5, 6, 7, 8, 1, 2, 3, 4;
         *
         * Step2
         * TempBuffer1 = ;
         * TempBuffer2 = 5, 6, 7, 8;
         * TempBuffer3 = 1, 2, 3, 4;
         *
         * Step3
         * TempBuffer1 = 1, 2, 3, 4, 5, 6, 7, 8;
         * TempBuffer2 = ;
         * TempBuffer3 = ;
         */

        [Test]
        public void NSubstituteReturningTest_testingReturnValuesForSpecificFunctionArgsAndProperties()
        {
            var mergeBufferingSubstitute = Substitute.For<IMergeBufferingIO>();
            mergeBufferingSubstitute.AllHaveNext.Returns(false, true, false, true);
            
            Assert.IsFalse(mergeBufferingSubstitute.AllHaveNext);
            Assert.IsTrue(mergeBufferingSubstitute.AllHaveNext);
            Assert.IsFalse(mergeBufferingSubstitute.AllHaveNext);
            Assert.IsTrue(mergeBufferingSubstitute.AllHaveNext);
        }

        [Test]
        public void mergeImaginaryFile_MergerShouldDoExactlyNIterations()
        {
            var records = new IRecord[]
            {
                Record.Dummy,
                new Record(new double[] {1}), new Record(new double[] {2}), new Record(new double[] {3}),
                new Record(new double[] {4}), new Record(new double[] {5}), new Record(new double[] {6}),
                new Record(new double[] {7}), new Record(new double[] {8})
            };
            var comparerSubstitute = Substitute.For<IRecordValueComparer>();
            comparerSubstitute.GetIndexOfSmallest().Returns(1, 1, 0, 0,   1, 1, 0, 0,  0,
                1, 1, 1, 1,   0, 0, 0, 0);
            comparerSubstitute.SmallestRecord.Returns(
                records[5], records[6], records[7], records[8],
                records[1], records[2], records[3], records[4], records[0], 
                records[1], records[2], records[3], records[4], 
                records[5], records[6], records[7], records[8]
                );
            var mergeBufferingSubstitute = Substitute.For<IMergeBufferingIO>();
            mergeBufferingSubstitute.GetNextRecordsFromAllBuffers().Returns(
                new IRecord[] {records[7], records[5]}
                );
            mergeBufferingSubstitute.GetNextRecordFrom(0).Returns(
                records[8],
                records[3], records[4], records[0],
                records[5], records[6], records[7], records[8], Record.NullRecord
            );
            mergeBufferingSubstitute.GetNextRecordFrom(1).Returns(
                records[6],
                records[1], records[2], Record.NullRecord,
                records[1], records[2], records[3], records[4], Record.NullRecord
            );
            mergeBufferingSubstitute.AllOutputBuffersAreEmpty.Returns(false, true);
            mergeBufferingSubstitute.AllHaveNextOrDummy.Returns(true, true, false, true, false);

            var merger = new Merger(2, mergeBufferingSubstitute, comparerSubstitute);
            merger.Merge();

            Assert.AreEqual(2, merger.Steps);
        }
        
    }
}