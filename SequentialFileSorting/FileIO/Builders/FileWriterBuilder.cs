using FileIO.Interfaces;
using FileIO.Writers;
using FileIO.Writers.Interfaces;

namespace FileIO.Builders
{
    public class FileWriterBuilder : FileIOBuilderBase
    {
        private const bool CREATE_NEW_FILE_DEFAULT = true;

        private IBlockSplitter blockSplitter;

        public FileWriterBuilder()
        {
            createOrOverwriteFile = CREATE_NEW_FILE_DEFAULT;
        }
        
        public FileWriterBuilder SetFilePath(string path)
        {
            filePath = path;
            return this;
        }
        
        public FileWriterBuilder SetBlockSize(int blockSize)
        {
            this.blockSize = blockSize;
            return this;
        }

        public FileWriterBuilder SetBlockSplitter(IBlockSplitter blockSplitter)
        {
            this.blockSplitter = blockSplitter;
            return this;
        }
        
        public FileWriterBuilder CreateNewFile(bool value = true)
        {
            createOrOverwriteFile = value;
            return this;
        }

        public FileWriterBuilder SetFileBase(IFileIOBase fileBase)
        {
            this.fileBase = fileBase;
            return this;
        }

        public IFileWriter Build()
        {
            if(fileBase == null)
                buildFileBase();
            
            if(blockSplitter == null)
                blockSplitter = new BlockSplitter(fileBase);

            return new BlockWriter(fileBase, blockSplitter, createOrOverwriteFile);

        }
    }
}