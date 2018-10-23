using System;
using System.Text;
using FileIO.Interfaces;

namespace FileIO.Builders
{
    public class FileReaderBuilder : FileIOBuilderBase
    {
        private const bool CREATE_NEW_FILE_DEFAULT = false;
        
        private IBlockReader blockReader;
        private ILineSeparator lineSeparator;
        private int beginningBlock;

        public FileReaderBuilder()
        {
            blockSize = DEFAULT_BLOCK_SIZE;
            beginningBlock = BEGINING_BLOCK_DEFAULT;
            createOrOverwriteFile = CREATE_NEW_FILE_DEFAULT;
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

        public FileReaderBuilder StartAtBlock(int beginningBlock)
        {
            this.beginningBlock = beginningBlock;
            return this;
        }
        
        public FileReaderBuilder SetFileBase(IFileIOBase fileBase)
        {
            this.fileBase = fileBase;
            return this;
        }
        
        public IFileReader Build()
        {
            if(blockReader == null)
                buildBlockReader();

            if (lineSeparator == null)
                lineSeparator = new NewlineSeparator();
                
            if(errorMessageLength != 0)
                throw new Exception(errorMessage);
                
            return new FileReader(blockReader, lineSeparator);
        }

        private void buildBlockReader()
        {
            if(fileBase == null)
                buildFileBase();
            blockReader = new BlockReader(fileBase, beginningBlock);
            
        }

        

       
    }
}