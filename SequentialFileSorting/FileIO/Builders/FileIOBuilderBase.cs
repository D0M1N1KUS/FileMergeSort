using System;
using System.Text;
using FileIO.Interfaces;

namespace FileIO.Builders
{
    public class FileIOBuilderBase
    {
        protected const int DEFAULT_BLOCK_SIZE = 4;
        protected const int BEGINING_BLOCK_DEFAULT = 0;
        protected string filePath;
        
        protected IFileIOBase fileBase;
        protected int blockSize;
        protected bool createOrOverwriteFile;

        protected int errorMessageLength => errorMessageBuilder.Length;
        protected string errorMessage => errorMessageBuilder.ToString();
        
        private StringBuilder errorMessageBuilder;

        protected FileIOBuilderBase()
        {
            errorMessageBuilder = new StringBuilder();
        }
        
        protected void buildFileBase()
        {
            try
            {
                fileBase = new FileIOBasics(filePath, blockSize, createOrOverwriteFile);
            }
            catch (Exception e)
            {
                appendErrorMessage(e);
            }
        }
        
        protected void appendErrorMessage(Exception e)
        {
            errorMessageBuilder.Append(e.Message);
            errorMessageBuilder.Append(Environment.NewLine);
            errorMessageBuilder.Append(e.StackTrace);
            errorMessageBuilder.Append(Environment.NewLine);
            errorMessageBuilder.Append(Environment.NewLine);
        }
    }
}