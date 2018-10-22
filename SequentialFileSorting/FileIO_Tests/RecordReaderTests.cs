using FileIO.Interfaces;
using FileIO.RecordIO;
using NSubstitute;
using NUnit.Framework;

namespace FileIO_Tests
{
    [TestFixture]
    public class RecordReaderTests
    {
        [Test]
        public void getRecordFromFile_LineContainsRandomSpaces()
        {
            var fileReader = Substitute.For<IFileReader>();
            fileReader.GetNextLine().Returns("1,0  2,0 3,0  4,0  0,5 ");
            
            var recordReader = new LineBasedRecordReader(fileReader, new ValueComponentsSplitter());
            var expectedRecord = new Record(new string[] {"1,0", "2,0", "3,0", "4,0", "0,5"});
            
            var actualRecord = recordReader.GetNextRecord();
            
            Assert.AreEqual(expectedRecord, actualRecord);
        }
        
        [Test]
        public void getRecordFromFile()
        {
            var fileReader = Substitute.For<IFileReader>();
            fileReader.GetNextLine().Returns("1,0 2,0 3,0 4,0 0,5");
            
            var recordReader = new LineBasedRecordReader(fileReader, new ValueComponentsSplitter());
            var expectedRecord = new Record(new string[] {"1,0", "2,0", "3,0", "4,0", "0,5"});
            
            var actualRecord = recordReader.GetNextRecord();
            
            Assert.AreEqual(expectedRecord, actualRecord);
        }
    }
}