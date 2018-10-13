using FileIO;
using FileIO.Interfaces;
using NUnit.Framework;
using NSubstitute;

namespace FileIO_Tests
{
    [TestFixture]
    public class FileReaderTests
    {
        [Test]
        public void getLineFromFile_fileLenghtIsTwoTimesLargerThanBlockSize()
        {
            var blockReader = Substitute.For<IBlockReader>();
            blockReader.GetNextBlock().Returns("abcdefgh", "ijklmn" + System.Environment.NewLine);
            var fileReader = new FileReader("someRandomPath", blockReader);

            string expectedLine = "abcdefghijklmn\r\n";
            string actualLine = fileReader.GetNextLine();
            
            Assert.AreEqual(expectedLine, actualLine);
        }
    }
}