using System;
using NUnit.Framework;
using FileIO;

namespace FileIO_Tests
{
    [TestFixture]
    public class NewLineSeparatorTests
    {
        [Test]
        public void getLinesFromBlock_theReadBlockContainsExactlyThreeLinesWithNewLineAtTheEnd_ShouldReturnThreeLines()
        {
            var block = "Line 1" + Environment.NewLine +
                        "Line 2" + Environment.NewLine +
                        "Line 3" + Environment.NewLine;
            var expectedLines = new string[] {"Line 1", "Line 2", "Line 3"};
            string[] actualLines;
            
            var foundLines = new NewlineSeparator().SeparateLines(block, out actualLines);
            
            CollectionAssert.AreEqual(expectedLines, actualLines);
        }

        [Test]
        public void getLinesFromBlock_theReadBlockContainsTwoCompleteLinesAndOneUncompletedLine_ShouldReturnTwoLines()
        {
            var block = "CompleteLine 1" + Environment.NewLine +
                        "CompleteLine 2" + Environment.NewLine +
                        "Incomplete Line";
            var expectedLines = new string[] {"CompleteLine 1", "CompleteLine 2"};
            string[] actualLines;

            var foundLines = new NewlineSeparator().SeparateLines(block, out actualLines);
            
            CollectionAssert.AreEqual(expectedLines, actualLines);
        }

        [Test]
        public void getLinesFromBlock_ReadBlockContainsOnlyOneIncompletedLine_ShouldReturnEmptyArray()
        {
            var block = "Incomplete Line";
            string[] actualLines;

            new NewlineSeparator().SeparateLines(block, out actualLines);
            
            CollectionAssert.IsEmpty(actualLines);
        }

        [Test]
        public void getLinesFromBlock_ReadBlockContainsThreeEmptyLines_SouldReturnThreeEmptyLines()
        {
            var block = Environment.NewLine + Environment.NewLine + Environment.NewLine;
            var expectedLines = new string[] {string.Empty, string.Empty, string.Empty};
            string[] actualLines;

            new NewlineSeparator().SeparateLines(block, out actualLines);
            
            CollectionAssert.AreEqual(expectedLines, actualLines);
        }
    }
}