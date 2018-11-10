using System;
using System.IO;
using FileIO.Interfaces;

namespace FileIO
{
    public class BlockReader : IBlockReader, IStatistics
    {
        public bool EndOfFile { get; private set; } = false;
        public long NumberOfAccesses { get; private set; } = 0;
        public IFileIOBase FileBase;
        
        protected long CurrentBlockNumber;
        
        public BlockReader(IFileIOBase fileBase = null, int beginningBlock = 0)
        {
            FileBase = fileBase;
            CurrentBlockNumber = beginningBlock;
        }


        public string GetNextBlock()
        {
            if (EndOfFile) return string.Empty;
            var lengthOfCurrentBlock = readNextBlock();
            return new string(FileBase.Block, 0, lengthOfCurrentBlock);
        }

        private int readNextBlock()
        {
            var lengthOfCurrentBlock = 0;
            using (var streamReader = new StreamReader(FileBase.FilePath))
            {
                for (var i = 0; i < CurrentBlockNumber; i++)
                {
                    if (isEndOfStream(streamReader)) 
                        break;
                    streamReader.ReadBlock(FileBase.Block, 0, FileBase.BlockSize);
                }

                lengthOfCurrentBlock = streamReader.ReadBlock(FileBase.Block, 0, FileBase.BlockSize);
            }

            if (!EndOfFile)
            {
                CurrentBlockNumber++;
                NumberOfAccesses++;
            }
            
            return lengthOfCurrentBlock;
        }

        private bool isEndOfStream(StreamReader streamReader)
        {
            if (!streamReader.EndOfStream) 
                return false;
            
            EndOfFile = true;
            FileBase.ClearBlock();
            return true;

        }
    }
}