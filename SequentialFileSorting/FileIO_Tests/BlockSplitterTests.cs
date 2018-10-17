using System.Collections.Generic;
using System.Linq;
using FileIO.Interfaces;
using FileIO.Writers;
using NSubstitute;
using NUnit.Framework;

namespace FileIO_Tests
{
    [TestFixture]
    public class BlockSplitterTests
    {
        private IFileIOBase getSubstitudeForFileBase(int blockSize)
        {
            var fileBase = Substitute.For<IFileIOBase>();
            fileBase.BlockSize.Returns(blockSize);
            return fileBase;
        }
        
        [Test]
        public void getBlocksFromString_ShouldReturnThreeEvenBlocksOfText_TestStringHasExactlyThreeBlocks()
        {
            var testString = "ThisTexttest";
            var expectedBocks = new string[] {"This", "Text", "test"};
            var blockSplitter = new BlockSplitter(getSubstitudeForFileBase(4));

            var actualBlocks = blockSplitter.GetBlocks(testString);
            
            CollectionAssert.AreEqual(expectedBocks, actualBlocks);
            Assert.AreEqual(string.Empty, blockSplitter.ExcessText);
        }
        
        [Test]
        public void
            getBlocksFromString_ShouldReturnTwoBlocksAndContainAnExcessString_TestStringIsShorterThanThreeBlocks()
        {
            var testString = "ThisTextSux";
            var expectedBlocks = new string[] {"This", "Text"};
            var blockSplitter = new BlockSplitter(getSubstitudeForFileBase(4));

            var actualBlocks = blockSplitter.GetBlocks(testString);
            
            CollectionAssert.AreEqual(expectedBlocks, actualBlocks);
            Assert.AreEqual("Sux", blockSplitter.ExcessText);
        }

        [Test]
        public void getBlocksFromString_ShouldReturnEmptyCollection_TestStringIsShorterThanOneBlock()
        {
            var testString = "Short text";
            var expectedBlocks = new string[0];
            var blockSplitter = new BlockSplitter(getSubstitudeForFileBase(256));

            var actualBlocks = blockSplitter.GetBlocks(testString);
            
            CollectionAssert.AreEqual(expectedBlocks, actualBlocks);
            Assert.AreEqual(testString, blockSplitter.ExcessText);
        }

        [Test]
        public void getBlocksFromString_ShouldReturnEmptyCollectionAndEmptyExcess_TestStringIsEmpty()
        {
            var expectedBlocks = new string[0];
            var blockSplitter = new BlockSplitter(getSubstitudeForFileBase(4));

            var actualBlocks = blockSplitter.GetBlocks(string.Empty);
            
            CollectionAssert.AreEqual(expectedBlocks, actualBlocks);
            Assert.AreEqual(string.Empty, blockSplitter.ExcessText);
        }

        [Test]
        public void getBlocksFromTwoStrings_ShouldReturnOneAndTwoBlocks_TestStringsContainTwoUnevenlySplittedBlocks()
        {
            var string1 = "ABC";
            var string2 = "DEFGH";
            var expectedBlocks = new string[] {"ABCD", "EFGH"};
            var blockSplitter = new BlockSplitter(getSubstitudeForFileBase(4));

            var actualBlocks = new List<string>();
            var blocks1 = blockSplitter.GetBlocks(string1, useExcessText: true).ToList();
            var blocks2 = blockSplitter.GetBlocks(string2, useExcessText: true).ToList();
            actualBlocks.AddRange(blocks1);
            actualBlocks.AddRange(blocks2);
            
            CollectionAssert.AreEqual(expectedBlocks, actualBlocks.ToArray());
            Assert.AreEqual(string.Empty, blockSplitter.ExcessText);
        }
    }
}