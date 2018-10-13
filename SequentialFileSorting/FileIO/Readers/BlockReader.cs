using System;
using System.IO;
using FileIO.Interfaces;

namespace FileIO
{
    public class BlockReader : FileBasics, IBlockReader
    {
        public bool EndOfFile => lastBlockLenght == 0;
        
        private int lastBlockLenght;
        
        public BlockReader(string pathToFile, int blockSize = 8) : base(pathToFile, blockSize)
        {
            lastBlockLenght = Int32.MaxValue;
        }


        public string GetNextBlock()
        {
            if (lastBlockLenght == 0) return string.Empty;
            var lengthOfCurrentBlock = readNextBlock();
            return new string(Block, 0, lengthOfCurrentBlock);
        }

        private int readNextBlock()
        {
            var lengthOfCurrentBlock = 0;
            using (var streamReader = new StreamReader(FilePath))
            {
                for (var i = 0; i < CurrentBlockNumber; i++)
                {
                    streamReader.ReadBlock(Block, 0, BlockSize);
                }

                lengthOfCurrentBlock = streamReader.ReadBlock(Block, 0, BlockSize);
            }

            return lengthOfCurrentBlock;
        }
    }
}