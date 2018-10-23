using System;
using System.Collections.Generic;
using FileIO;
using FileIO.Builders;
using FileIO.Interfaces;
using FileIO.RecordIO;
using FileIO.RecordIO.Interfaces;
using FileIO.Writers;
using SequentialFileIO.Enums;

namespace SequentialFileIO
{
    public class FileBuffers : IFileBuffers
    {
        private const int DEFAULT_BLOCK_SIZE = 8;
        
        public Dictionary<string, IRecordReader> InputBuffer { get; private set; }
        public Dictionary<string, IRecordAppender> OutputBuffer { get; private set; }

        public IFileNameGenerator FileNameGenerator;
        
        private Dictionary<string, SequentialFile> bufferFiles;

        public FileBuffers(string sourceFile = null, int numberOfBuffers = 0, IFileNameGenerator fileNameGenerator = null)
        {
            FileNameGenerator = fileNameGenerator ?? new TemporaryFileNameGenerator("Buffer");
            initializeDictionaries();

            AddBuffer(sourceFile);
            for (var i = 0; i < numberOfBuffers; i++)
            {
                AddBuffer(FileNameGenerator.GetNextAvailableName());
            }
        }

        public FileBuffers(string sourceFile = null, int numberOfBuffers = 0, string locationOfBufferFiles = null)
        {
            initializeDictionaries();   
        }
        
        private void initializeDictionaries()
        {
            bufferFiles = new Dictionary<string, SequentialFile>();
            InputBuffer = new Dictionary<string, IRecordReader>();
            OutputBuffer = new Dictionary<string, IRecordAppender>();
        }
        
        
        public void AddBuffer(string filePath, bool createNewFile = true, 
            FileOperationType operationType = FileOperationType.Input)
        {
            var fileReader = new FileReaderBuilder()
                .SetFilePath(filePath)
                .SetBlockSize(DEFAULT_BLOCK_SIZE)
                .Build();
            InputBuffer.Add(filePath, new LineBasedRecordReader(fileReader, new ValueComponentsSplitter()));
            
            var fileWriter = new FileWriterBuilder()
                .SetFilePath(filePath)
                .CreateNewFile(createNewFile)
                .SetBlockSize(DEFAULT_BLOCK_SIZE)
                .Build();
            IRecordAppender writer = new RecordAppender(fileWriter);
            
            bufferFiles.Add(filePath, new SequentialFile(filePath, operationType));
        }

        public void CreateNewBuffers(int numberOfBuffers)
        {
            for (var i = 0; i < numberOfBuffers; i++)
            {
                AddBuffer(FileNameGenerator.GetNextAvailableName());
            }
        }
    }
}