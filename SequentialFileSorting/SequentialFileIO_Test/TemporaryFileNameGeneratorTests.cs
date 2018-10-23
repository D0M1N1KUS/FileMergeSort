using NSubstitute;
using NUnit.Framework;
using RecordFileGenerator.Interfaces;
using SequentialFileIO;
using SequentialFileIO.Enums;

namespace SequentialFileIO_Test
{
    [TestFixture]
    public class TemporaryFileNameGeneratorTests
    {
        [Test]
        public void getThreeFileNames()
        {
            var baseFileName = "test";
            var fileExtension = "tmp";
            var baseDirectory = "D:\\";
            var fileNameBegin = baseDirectory + baseFileName;
            var fileNameEnd = "." + fileExtension;
            var fileNameGenerator = new TemporaryFileNameGenerator(baseFileName, fileExtension, baseDirectory);
            var fileExistsChecker = Substitute.For<IFileExistsChecker>();
            fileExistsChecker.FileExists(string.Empty).ReturnsForAnyArgs(
                false, true, false, true, true, false);
            fileNameGenerator.FileExistsChecker = fileExistsChecker;
            var actualFileNames = new string[3];
            var expectedFileNames = new string[3]
            {
                fileNameBegin + fileNameEnd,
                fileNameBegin + "2" + fileNameEnd,
                fileNameBegin + "3" + fileNameEnd
            };

            for (var i = 0; i < 3; i++)
            {
                actualFileNames[i] = fileNameGenerator.GetNextAvailableName();
            }
            
            CollectionAssert.AreEqual(expectedFileNames, actualFileNames);
        }

        [Test]
        public void getFileName_ProvidedNameStringsContainIllegalCharacters_IllegalCharactersShouldBeRemoves()
        {
            var fileNameGenerator = new TemporaryFileNameGenerator("T:E?S|t", ".tmp", "D:\\");
            var expectedFilePath = "D:\\TESt.tmp";

            var actualFilePath = fileNameGenerator.GetNextAvailableName();
            
            Assert.AreEqual(expectedFilePath, actualFilePath);
        }

        [Test]
        public void getRandomUppercaseOnlyString()
        {
            var randomString = new RandomStringGenerator(10, LetterProperties.UppercaseOnly);
            randomString.PseudoRng = Substitute.For<IPseudoRNG>();
            randomString.PseudoRng.GetDouble().Returns(0);
            var expectedString = "AAAAAAAAAA";

            var actualString = randomString.GetRandomString();
            
            Assert.AreEqual(expectedString, actualString);
        }

        [Test]
        public void getRandomLowercaseOnlyString()
        {
            var randomString = new RandomStringGenerator(10, LetterProperties.LowercaseOnly);
            randomString.PseudoRng = Substitute.For<IPseudoRNG>();
            randomString.PseudoRng.GetDouble().Returns(0);
            var expectedString = "aaaaaaaaaa";

            var actualString = randomString.GetRandomString();
            
            Assert.AreEqual(expectedString, actualString);
        }

        [Test]
        public void getRandomStringWithUpperAndLowercaseLetters()
        {
            var randomString = new RandomStringGenerator(10);
            randomString.PseudoRng = Substitute.For<IPseudoRNG>();
            randomString.PseudoRng.GetDouble().Returns(0);
            randomString.PseudoRng.GetBool()
                .ReturnsForAnyArgs(false, true, false, true, false, true, false, true, false, true);
            var expectedString = "aAaAaAaAaA";

            var actualString = randomString.GetRandomString();
            
            Assert.AreEqual(expectedString, actualString);
        }

        [Test]
        public void getRandomStringWithUppercaseLettersAndNumbers()
        {
            var randomString = new RandomStringGenerator(10, LetterProperties.UppercaseOnly, true);
            randomString.PseudoRng = Substitute.For<IPseudoRNG>();
            randomString.PseudoRng.GetDouble().Returns(0);
            randomString.PseudoRng.GetBool()
                .ReturnsForAnyArgs(false, true, false, true, false, true, false, true, false, true);
            randomString.PseudoRng.GetInt().ReturnsForAnyArgs(1, 2, 3, 4, 5);
            var expectedString = "A1A2A3A4A5";

            var actualString = randomString.GetRandomString();
            
            Assert.AreEqual(expectedString, actualString);
        }
    }
}