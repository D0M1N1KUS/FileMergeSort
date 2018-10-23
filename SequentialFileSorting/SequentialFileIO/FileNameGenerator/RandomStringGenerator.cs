using System;
using System.Text;
using RecordFileGenerator;
using RecordFileGenerator.Interfaces;
using SequentialFileIO.Enums;

namespace SequentialFileIO
{
    public class RandomStringGenerator : IRandomStringGenerator
    {
        private const int DEFAULT_LENGTH = 10;
        
        public IPseudoRNG PseudoRng;

        private int preferredLength;
        private LetterProperties letterProperties;
        private bool useNumbersInFileName;
        
        public RandomStringGenerator(int preferredLength = 0, 
            LetterProperties letterProperties = LetterProperties.UpperAndLowerCase, 
            bool useNumbersInFileName = false)
        {
            this.preferredLength = preferredLength > 0 ? preferredLength : DEFAULT_LENGTH;
            this.letterProperties = letterProperties;
            this.useNumbersInFileName = useNumbersInFileName;
            
            PseudoRng = new PseudoRNG();
        }

        public string GetRandomString(int length = 0)
        {
            return GetRandomString(length, this.letterProperties, this.useNumbersInFileName);
        }

        public string GetRandomString(int preferredLength, LetterProperties letterProperties = LetterProperties.UpperAndLowerCase,
            bool useNumbersInFileName = false)
        {
            var usedPreferredLength = preferredLength > 0 ? preferredLength : this.preferredLength;
            
            var randomChars = new char[usedPreferredLength];

            for (var i = 0; i < usedPreferredLength; i++)
            {
                randomChars[i] = useNumbersInFileName && PseudoRng.GetBool() ? 
                    getRandomNumber() : getRandomLetter(letterProperties);
            }
            
            return new string(randomChars);
        }

        private char getRandomLetter(LetterProperties letterProperties)
        {
            var preferredLetters = letterProperties;
            var randomChar = Convert.ToChar(Convert.ToInt32((Math.Floor(26 * PseudoRng.GetDouble() + 65))));
            if (preferredLetters == LetterProperties.UpperAndLowerCase)
                randomChar += Convert.ToChar(PseudoRng.GetBool() ? 0 : 32);
            else if (preferredLetters == LetterProperties.LowercaseOnly)
                randomChar += Convert.ToChar(32);

            return randomChar;
        }

        private char getRandomNumber()
        {
            return (char)(PseudoRng.GetInt() % 10 + 48);
        }
    }
}