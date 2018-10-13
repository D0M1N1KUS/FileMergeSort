using System.IO;

namespace FileIO
{
    public  class FileBasics
    {
        protected readonly string FilePath;
        protected readonly int BlockSize;
        protected char[] Block;

        protected long CurrentBlockNumber;
        
        public FileBasics(string pathToFile, int blockSize)
        {
            CheckIfFileExists(pathToFile);
            FilePath = pathToFile;

            blockSize = ValidateBlockSize(blockSize) ? blockSize : 8;
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
        
        protected void clearBlock()
        {
            for (var i = 0; i < BlockSize; i++)
            {
                Block[i] = '\0';
            }
        }
    }
}