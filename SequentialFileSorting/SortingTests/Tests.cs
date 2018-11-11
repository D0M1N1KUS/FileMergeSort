using System;
using System.IO;
using NUnit.Framework;
using SequentialFileSorting.Sorting;
using SequentialFileSorting.SortingManagment;

namespace SortingTests
{
    [TestFixture]
    public class Tests
    {
        [Test]
        public void sortRecordsInFile()
        {
            var testFilePath = "D:\\FileToSort.txt";
            var unsortedFileLinesArray = new string[] {"6", "5", "4", "16", "17", "3", "2", "1"};
            var sortedFileLinesArray = new string[] {"1", "2", "3", "4", "5", "6", "16", "17"};
            var sortedFileContent = string.Join(Environment.NewLine, sortedFileLinesArray) + Environment.NewLine;
            File.WriteAllText(testFilePath, string.Join(Environment.NewLine, unsortedFileLinesArray) + Environment.NewLine);
            var sortingParameters = new SortingParameters() { NumberOfTemporaryFiles = 2 };
            var fileParameters = new FileParameters() { BlockSize = 4, SourceFileName = testFilePath, 
                TemporaryBufferFileDirectory = "D:\\"};
            var sorter = new PolyPhaseSorting(sortingParameters, fileParameters);
            
            sorter.Distribution.Distribute();
            sorter.Merger.Merge();
            sorter.RestoreOriginalFileName();
            var actualFileContent = File.ReadAllText(testFilePath);
            
            Assert.AreEqual(sortedFileContent, actualFileContent);
            
            File.Delete(testFilePath);
        }

        [Test]
        public void sortRecordsInFile_FileContainsOnlyOneRecord()
        {
            var testFilePath = "D:\\FileToSort.txt";
            var unsortedFileLinesArray = new string[] {"1"};
            var sortedFileLinesArray = new string[] {"1"};
            var sortedFileContent = string.Join(Environment.NewLine, sortedFileLinesArray) + Environment.NewLine;
            File.WriteAllText(testFilePath, string.Join(Environment.NewLine, unsortedFileLinesArray) + Environment.NewLine);
            var sortingParameters = new SortingParameters() { NumberOfTemporaryFiles = 2 };
            var fileParameters = new FileParameters() { BlockSize = 4, SourceFileName = testFilePath, 
                TemporaryBufferFileDirectory = "D:\\"};
            var sorter = new PolyPhaseSorting(sortingParameters, fileParameters);
            
            sorter.Distribution.Distribute();
            sorter.Merger.Merge();
            sorter.RestoreOriginalFileName();
            var actualFileContent = File.ReadAllText(testFilePath);
            
            Assert.AreEqual(sortedFileContent, actualFileContent);
            
            File.Delete(testFilePath);
        }

        [Test]
        public void sortAlreadySortedfile()
        {
            var testFilePath = "D:\\FileToSort.txt";
            var unsortedFileLinesArray = new string[] {"1", "2", "3", "4", "5"};
            var sortedFileContent = string.Join(Environment.NewLine, unsortedFileLinesArray) + Environment.NewLine;
            File.WriteAllText(testFilePath, string.Join(Environment.NewLine, unsortedFileLinesArray) + Environment.NewLine);
            var sortingParameters = new SortingParameters() { NumberOfTemporaryFiles = 2 };
            var fileParameters = new FileParameters() { BlockSize = 4, SourceFileName = testFilePath, 
                TemporaryBufferFileDirectory = "D:\\"};
            var sorter = new PolyPhaseSorting(sortingParameters, fileParameters);
            
            sorter.Distribution.Distribute();
            sorter.Merger.Merge();
            sorter.RestoreOriginalFileName();
            var actualFileContent = File.ReadAllText(testFilePath);
            
            Assert.AreEqual(sortedFileContent, actualFileContent);
            
            File.Delete(testFilePath);
        }
    }
}