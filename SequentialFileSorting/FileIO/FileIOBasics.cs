using System.IO;
using FileIO.Interfaces;

namespace FileIO
{
    public  class FileIOBasics : IFileIOBase
    {
        private const int DEFAULT_BLOCK_SIZE = 8;
        
        public string FilePath { get; protected set; }
        public string FileName { get; private set; }
        public int BlockSize  { get; protected set; }
        public char[] Block { get; protected set; }

        private bool createNewFile;

        public FileIOBasics(string pathToFile, int blockSize = 0, bool createNewFile = false)
        {
            this.createNewFile = createNewFile;
            CheckIfFileExists(pathToFile);
            FilePath = pathToFile;

            BlockSize = ValidateBlockSize(blockSize) ? blockSize : DEFAULT_BLOCK_SIZE;
            Block = new char[this.BlockSize];

            createNewFileIfNecessary();
            FileName = Path.GetFileName(pathToFile);
        }
        
        public void CheckIfFileExists(string filePath)
        {
            if(string.IsNullOrEmpty(filePath)) 
                throw new IOException("The provided path is null or empty!");
            if (!File.Exists(filePath) && !createNewFile)
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

        public void EraseFileContent()
        {
            File.Create(FilePath).Close();
        }

        private void createNewFileIfNecessary()
        {
            if (createNewFile)
                File.Create(FilePath).Close();
        }
    }
}