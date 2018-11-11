using System;
using System.IO;
using System.Linq;
using FileIO.Interfaces;
using FileIO.Writers.Interfaces;

namespace FileIO.Writers
{
    public class BlockWriter : IFileWriter, IStatistics
    {
        public IFileIOBase FileBase;
        public IBlockSplitter BlockSplitter;

        public long NumberOfAccesses { get; private set; } = 0;

        private bool firstLine = true;

        public BlockWriter(IFileIOBase fileBase = null, IBlockSplitter blockSplitter = null, bool overwriteFile = false)
        {
            FileBase = fileBase;
            BlockSplitter = blockSplitter;
        }
        
        public void Write(string text)
        {
            var blocks = BlockSplitter.GetBlocks(text, useExcessText: true);
            
            using (var streamWriter = new StreamWriter(FileBase.FilePath, append: true))
            {
                foreach (var block in blocks)
                {
                    streamWriter.Write(block);
                    NumberOfAccesses++;
                }
            }
        }

        public void WriteLine(string text)
        {
            Write(text + Environment.NewLine);
        }

        public void Flush()
        {
            var excessText = BlockSplitter.ExcessText;
            if (string.IsNullOrEmpty(excessText)) return;
            
            using (var streamWriter = new StreamWriter(FileBase.FilePath, append: true))
            {
                streamWriter.Write(excessText);
                NumberOfAccesses++;
            }
        }

        public void ClearFile()
        {
            FileBase.EraseFileContent();
            firstLine = true;
        }
    }
}