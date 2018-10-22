using System;
using System.Text;
using FileIO.Interfaces;

namespace FileIO.Builders
{
    public class FileReaderBuilder
    {
        private const int DEFAULT_BLOCK_SIZE = 4;
        private const bool CREATE_NEW_FILE_DEFAULT = false;
        private const int BEGINING_BLOCK_DEFAULT = 0;
        
        private IFileReader FileReader;

        private string filePath;
        private IBlockReader blockReader;
        private ILineSeparator lineSeparator;
        private IFileIOBase fileBase;

        private int blockSize;
        private bool createNewFile;
        private int beginningBlock;

        private StringBuilder errorMessageBuilder;

        public FileReaderBuilder()
        {
            errorMessageBuilder = new StringBuilder();
            blockSize = DEFAULT_BLOCK_SIZE;
            createNewFile = CREATE_NEW_FILE_DEFAULT;
            beginningBlock = BEGINING_BLOCK_DEFAULT;
        }

        public FileReaderBuilder SetFilePath(string filePath)
        {
            this.filePath = filePath;
            return this;
        }

        public FileReaderBuilder SetBlockReader(IBlockReader blockReader)
        {
            this.blockReader = blockReader;
            return this;
        }

        public FileReaderBuilder SetBlockSize(int blockSize)
        {
            this.blockSize = blockSize;
            return this;
        }

        public FileReaderBuilder CreateNewFile()
        {
            createNewFile = true;
            return this;
        }

        public FileReaderBuilder DontCreateNewFile()
        {
            createNewFile = false;
            return this;
        }

        public FileReaderBuilder StartAtBlock(int beginningBlock)
        {
            this.beginningBlock = beginningBlock;
            return this;
        }
        
        public IFileReader Build()
        {
            if(blockReader == null)
                buildBlockReader();

            if (lineSeparator == null)
                lineSeparator = new NewlineSeparator();
                
            if(errorMessageBuilder.Length != 0)
                throw new Exception(errorMessageBuilder.ToString());
                
            return new FileReader(blockReader, lineSeparator);
        }

        private void buildBlockReader()
        {
            if(fileBase == null)
                buildFileBase();
            blockReader = new BlockReader(fileBase, beginningBlock);
            
        }

        private void buildFileBase()
        {
            try
            {
                fileBase = new FileIOBasics(filePath, blockSize, createNewFile);
            }
            catch (Exception e)
            {
                appendErrorMessage(e);
            }
        }

        private void appendErrorMessage(Exception e)
        {
            errorMessageBuilder.Append(e.Message);
            errorMessageBuilder.Append(Environment.NewLine);
            errorMessageBuilder.Append(e.StackTrace);
            errorMessageBuilder.Append(Environment.NewLine);
            errorMessageBuilder.Append(Environment.NewLine);
        }
    }
}