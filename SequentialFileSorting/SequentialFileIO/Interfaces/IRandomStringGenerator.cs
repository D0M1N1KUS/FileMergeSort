using SequentialFileIO.Enums;

namespace SequentialFileIO
{
    public interface IRandomStringGenerator
    {
        string GetRandomString(int length = 0);
        string GetRandomString(int preferredLength,
            LetterProperties letterProperties = LetterProperties.UpperAndLowerCase,
            bool useNumbersInFileName = false);
    }
}