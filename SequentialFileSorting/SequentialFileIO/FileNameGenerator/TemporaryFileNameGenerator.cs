using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using FileIO.Interfaces;

namespace SequentialFileIO
{
    public class TemporaryFileNameGenerator : IFileNameGenerator
    {
        private readonly string EMPTY = string.Empty;

        private readonly Regex IllegalPathCharactersRegex =
            new Regex("[" + Regex.Escape(string.Join(string.Empty, System.IO.Path.GetInvalidPathChars())) + "]");

        private readonly Regex IllegalFileNameCharactersRegex =
            new Regex("[" + Regex.Escape(string.Join(string.Empty, System.IO.Path.GetInvalidFileNameChars())) + "]");
        
        public IRandomStringGenerator RandomString
        {
            get { return randomString ?? (randomString = new RandomStringGenerator()); }
            set { randomString = value; }
        }

        public IFileExistsChecker FileExistsChecker;

        private IRandomStringGenerator randomString;
        
        private string baseDirectory;
        private string baseFileName;
        private string extension;
                
        public TemporaryFileNameGenerator(string baseFileName = null, string extension = null, string baseDirectory = null)
        {
            this.baseFileName = string.IsNullOrEmpty(baseFileName) ? 
                RandomString.GetRandomString() : 
                removeIllegalFileNameCharacters(baseFileName);
            this.extension = string.IsNullOrEmpty(extension) ? 
                EMPTY : 
                removeIllegalFileNameCharacters(extension);
            this.baseDirectory = string.IsNullOrEmpty(baseDirectory) ? 
                EMPTY : 
                removeIllegalPathCharacters(baseDirectory);
            FileExistsChecker = new FileExistsChecker();
        }

        public string GetNextAvailableName(long triesLimit = long.MaxValue)
        {
            var tries = triesLimit;
            var fileNumber = 1;
            
            while (tries-- > 0)
            {
                var fileName = string.Concat(baseDirectory, baseFileName,
                    fileNumber > 1 ? fileNumber.ToString() : EMPTY, ".", extension);
                if (!FileExistsChecker.FileExists(fileName))
                    return fileName;
                fileNumber++;
            }
            
            throw new Exception("TemporaryFileNameGenerator: Failed to create random file name.");
        }

        private string removeIllegalFileNameCharacters(string name)
        {
            return IllegalFileNameCharactersRegex.Replace(name, string.Empty).Replace(".", string.Empty);
        }

        private string removeIllegalPathCharacters(string path)
        {
            return IllegalPathCharactersRegex.Replace(path, string.Empty).Replace(".", string.Empty);
        }
    }
}