using System.IO;
using FileIO.Interfaces;

namespace FileIO
{
    public  class FileBasics : IFileIOBase
    {
        public string FilePath { get; set; }
        public int BlockSize  { get; set; }
        public char[] Block { get; set; }
        
        public FileBasics(string pathToFile, int blockSize)
        {
            CheckIfFileExists(pathToFile);
            FilePath = pathToFile;

            BlockSize = ValidateBlockSize(blockSize) ? blockSize : 8;
            Block = new char[this.BlockSize];
        }
        
        public void CheckIfFileExists(string filePath)
        {
            if(string.IsNullOrEmpty(filePath)) 
                throw new IOException("The provided path is null or empty!");
            if (!File.Exists(filePath))
                throw new IOException("The file " + filePath + " does not exist!");
        }

        public bool ValidateBlockSize(int blockSize)
        {
            return blockSize > 0;
        }
        
        public void ClearBlock()
        {
            for (var i = 0; i < BlockSize; i++)
            {
                Block[i] = '\0';
            }
        }
    }
}