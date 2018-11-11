using System.IO;
using FileIO;
using FileIO.Writers;
using NUnit.Framework;

namespace FileIO_Tests
{
    [TestFixture]
    public class BlockWriterTests
    {
        [Test]
        public void writeString_StringIsTooLongToFitInOneBlockAndNeedsToBeFlushedInTheEnd()
        {
            var sentence = "Sample text\r\n";
            var filePath = "D:\\TestFile.txt";
            var blockWriter = initializeBlockWriter(filePath, 4);
            var expectedNumberOfAccesses = 4;
            
            blockWriter.Write(sentence);
            blockWriter.Flush();
            
            Assert.AreEqual(sentence, File.ReadAllText(filePath));
            Assert.AreEqual(expectedNumberOfAccesses, blockWriter.NumberOfAccesses);

            File.Delete(filePath);
        }

        [Test]
        public void writeString_StringIsShorterThanBlock()
        {
            var sentence = "Short sample text\r\n";
            var filePath = "D:\\TestFile.txt";
            var blockWriter = initializeBlockWriter(filePath, 2137);
            var expectedNumberOfAccesses = 1;
            
            blockWriter.Write(sentence);
            blockWriter.Flush();
            
            Assert.AreEqual(sentence, File.ReadAllText(filePath));
            Assert.AreEqual(expectedNumberOfAccesses, blockWriter.NumberOfAccesses);

            File.Delete(filePath);
        }

        [Test]
        public void writeString_StringFitsInBlock()
        {
            var sentence = "1234";
            var filePath = "D:\\TestFile.txt";
            var blockWriter = initializeBlockWriter(filePath, 4);
            var expectedNumberOfAccesses = 1;
            
            blockWriter.Write(sentence);
            blockWriter.Flush();
            
            Assert.AreEqual(sentence, File.ReadAllText(filePath));
            Assert.AreEqual(expectedNumberOfAccesses, blockWriter.NumberOfAccesses);

            File.Delete(filePath);
        }

        private static BlockWriter initializeBlockWriter(string filePath, int blockSize)
        {
            if (File.Exists(filePath)) File.Delete(filePath);
            var fileBase = new FileIOBasics(filePath, blockSize, true);
            var blockWriter = new BlockWriter(fileBase, new BlockSplitter(fileBase), true);
            return blockWriter;
        }
    }
}