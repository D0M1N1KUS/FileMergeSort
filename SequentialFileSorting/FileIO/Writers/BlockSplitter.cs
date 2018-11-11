using System;
using FileIO.Interfaces;
using FileIO.Writers.Interfaces;

namespace FileIO.Writers
{
    public class BlockSplitter : IBlockSplitter
    {
        public string ExcessText
        {
            get
            {
                var text = excessText;
                excessText = string.Empty;
                return text;
            }
        }

        

        public IFileIOBase FileBase;

        private string currentText = string.Empty;
        private string excessText = string.Empty;

        public BlockSplitter(IFileIOBase fileBase = null)
        {
            FileBase = fileBase;
        }
        
        public string[] GetBlocks(string text, bool useExcessText = false)
        {
            currentText = useExcessText ? string.Concat(excessText, text) : text;
            var numberOfTextBlocks = (int)Math.Floor((float) currentText.Length / FileBase.BlockSize);
            var blocks = new string[numberOfTextBlocks];
            
            for (var i = 0; i < numberOfTextBlocks; i++)
            {
                blocks[i] = currentText.Substring(i * FileBase.BlockSize, FileBase.BlockSize);
            }

            excessText = currentText.Substring(numberOfTextBlocks * FileBase.BlockSize);
            
            return blocks;
        }
    }
}