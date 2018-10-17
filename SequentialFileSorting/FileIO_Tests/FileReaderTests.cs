using System;
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
            var lineSeparator = Substitute.For<ILineSeparator>();
            string[] whyDoesDiscardingNotWork;
            lineSeparator.SeparateLines(null, out whyDoesDiscardingNotWork).ReturnsForAnyArgs(
                x => { 
                    x[1] = new string[0];
                    return 0;
                },
                y => { 
                    y[1] = new string[] {"abcdefghijklmn"};
                    return 1;
                });
            
            string expectedLine = "abcdefghijklmn";
            var fileReader = new FileReader();
            fileReader.BlockReader = blockReader;
            fileReader.LineSeparator = lineSeparator;
            
            string actualLine = fileReader.GetNextLine();
            
            Assert.AreEqual(expectedLine, actualLine);
        }

        [Test]
        public void getLineFromFile_fileContainsTwoLines_GetNextBlockShouldBeCalledOnlyOnce()
        {
            var expectedFirstLine = "Line 1";
            var expectedSecondLine = "Line 2";
            var blockReader = Substitute.For<IBlockReader>();
            blockReader.GetNextBlock().Returns(expectedFirstLine + Environment.NewLine +
                                               expectedSecondLine + Environment.NewLine, 
                                               Environment.NewLine);
            var lineSeparator = Substitute.For<ILineSeparator>();
            string[] outFiller;
            lineSeparator.SeparateLines(null, out outFiller).ReturnsForAnyArgs(
                x =>
                {
                    x[1] = new string[] {expectedFirstLine, expectedSecondLine};
                    return 2;
                });
            
            var fileReader = new FileReader();
            fileReader.BlockReader = blockReader;
            fileReader.LineSeparator = lineSeparator;

            var actualFirstLine = fileReader.GetNextLine();
            var actualSocondLine = fileReader.GetNextLine();

            blockReader.Received(1).GetNextBlock();
            Assert.AreEqual(expectedFirstLine, actualFirstLine);
            Assert.AreEqual(expectedSecondLine, actualSocondLine);

        }

        [Test]
        public void readWholeFile_fileIsNotBeingMocked_ShouldContainThreeFullLinesAndOneIncomplete()
        {
            var expectedLines = new string[]
            {
                "Line 1",
                "Line 2",
                "Line 3",
                "Maybe incomplete line"
            };
            
            var fileReader = new FileReader(
                new BlockReader(
                    new FileBasics(@"D:\visual studio 2015\Projects\CollageProjects\Struktury Baz Danych\Projekt1 - Sortowanie plików sekwencyjnych 2018\SequentialFileSorting\FileIO_Tests\TEST.txt", 1)), 
                new NewlineSeparator());
            var actualLines = new string[4];

            for (var i = 0; i < 4; i++)
            {
                actualLines[i] = fileReader.GetNextLine();
            }
            
            CollectionAssert.AreEqual(expectedLines, actualLines);
        }
    }
}