using FileIO.RecordIO;
using FileIO.RecordIO.Interfaces;
using NSubstitute;
using NUnit.Framework;
using RecordFileGenerator;
using RecordFileGenerator.Interfaces;

namespace FileIO_Tests
{
    [TestFixture]
    public class RecordGeneratorTests
    {
        [Test]
        public void generateRecordStruct()
        {
            var pseudoRNG = Substitute.For<IPseudoRNG>();
            pseudoRNG.GetInt(0, 0).ReturnsForAnyArgs(10, 5, 0);
            pseudoRNG.GetDouble().Returns(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 1, 2, 3, 4, 5);
            var plaintextRecordGenerator = new RandomPlaintextRecordGenerator(pseudoRNG);
            var expectedRecords = new IRecord[]
            {
                new Record(new double[] {1, 2, 3, 4, 5, 6, 7, 8, 9, 10}),
                new Record(new double[] {1, 2, 3, 4, 5}),
                new Record(new double[0])
            };
            
            var actualRecords = new IRecord[3];
            for (var i = 0; i < 3; i++)
            {
                actualRecords[i] = plaintextRecordGenerator.GenerateRandomRecord();
            }
            
            CollectionAssert.AreEqual(expectedRecords, actualRecords);
        }

        [Test]
        public void generateRecordString()
        {
            var pseudoRNG = Substitute.For<IPseudoRNG>();
            pseudoRNG.GetInt(0, 0).ReturnsForAnyArgs(10);
            pseudoRNG.GetDouble().Returns(1, 2, 3, 4, 5, 6, 7, 8, 9, 10);
            var plaintextRecordGenerator = new RandomPlaintextRecordGenerator(pseudoRNG);
            var expectedString = "1 2 3 4 5 6 7 8 9 10";

            var actualString = plaintextRecordGenerator.GenerateRandomRecordValuesString(" ");
            
            Assert.AreEqual(expectedString, actualString);
        }
    }
}