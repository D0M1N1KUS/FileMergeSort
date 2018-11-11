using System;
using System.IO;
using NUnit.Framework;
using SequentialFileSorting.Sorting;

namespace SortingTests
{
    [TestFixture]
    public class PreSortingTests
    {
        [Test]
        public void getNumberOfSeriesInFile()
        {
            var expectedNumberOfSeries = 6;

            var actualNumberOfSeries = PreSorting.GetNumberOfSeries(
                @"D:\visual studio 2015\Projects\CollageProjects\Struktury Baz Danych\Projekt1 - Sortowanie plików sekwencyjnych 2018\SequentialFileSorting\SortingTests\SeriesTest.txt");
            
            Assert.AreEqual(expectedNumberOfSeries, actualNumberOfSeries);
        }
    }
}