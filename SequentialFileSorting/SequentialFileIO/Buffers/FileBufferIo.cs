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
        private int bufferIndex;
        
        public int NumberOfOutputBuffers { get; private set; }
        
        public Dictionary<int, IRecordReader> InputBuffer { get; private set; }
        public Dictionary<int, IRecordAppender> OutputBuffer { get; private set; }

        public IFileNameGenerator FileNameGenerator;
        public int BlockSize = DEFAULT_BLOCK_SIZE;
        
        private Dictionary<int, ISequentialFile> bufferFiles;

        
        public FileBuffers(string sourceFile = null, int numberOfOutputBuffers = 0, IFileNameGenerator fileNameGenerator = null)
        {
            FileNameGenerator = fileNameGenerator ?? new TemporaryFileNameGenerator("IOBuffer");
            initializeDictionaries();

            AddBuffer(sourceFile, false, FileOperationType.Output);
            for (var i = 0; i < numberOfOutputBuffers; i++)
            {
                AddBuffer(FileNameGenerator.GetNextAvailableName());
            }

            bufferIndex = 0;
            NumberOfOutputBuffers = numberOfOutputBuffers;
        }

        
        public FileBuffers(string sourceFile = null, int numberOfOutputBuffers = 0, string locationOfBufferFiles = null)
        {
            initializeDictionaries();   
        }
        
        private void initializeDictionaries()
        {
            bufferFiles = new Dictionary<int, ISequentialFile>();
            InputBuffer = new Dictionary<int, IRecordReader>();
            OutputBuffer = new Dictionary<int, IRecordAppender>();
        }
        
        
        public void AddBuffer(string filePath, bool createNewFile = true, 
            FileOperationType operationType = FileOperationType.Input)
        {
            createNewReader(filePath);
            createNewWriter(filePath, createNewFile);
            bufferFiles.Add(bufferIndex, new SequentialFile(filePath, bufferIndex, operationType));
            bufferIndex++;
            NumberOfOutputBuffers++;
        }

        private void createNewWriter(string filePath, bool createNewFile)
        {
            var fileWriter = new FileWriterBuilder()
                .SetFilePath(filePath)
                .CreateNewFile(createNewFile)
                .SetBlockSize(BlockSize)
                .Build();
            IRecordAppender writer = new RecordAppender(fileWriter);
            OutputBuffer.Add(bufferIndex, writer);
        }

        private void createNewReader(string filePath)
        {
            var fileReader = new FileReaderBuilder()
                .SetFilePath(filePath)
                .SetBlockSize(BlockSize)
                .Build();
            InputBuffer.Add(bufferIndex, new LineBasedRecordReader(fileReader, new ValueComponentsSplitter()));
        }

        public void CreateNewBuffers(int numberOfBuffers)
        {
            for (var i = 0; i < numberOfBuffers; i++)
            {
                AddBuffer(FileNameGenerator.GetNextAvailableName());
            }

            NumberOfOutputBuffers += numberOfBuffers;
        }

        public IRecordAppender GetOutputBuffer(int index)
        {
            if (!OutputBuffer.ContainsKey(index))
                throw new Exception("FileBuffers->CreateNewBuffers: Buffer with index [" + index +
                                    "] does not exist!");
            if(bufferFiles[index].FileOperationType != FileOperationType.Output)
                throw new Exception("FileBuffers->CreateNewBuffers: Buffer with index [" + index + 
                                    "] is currently not an output buffer!");
            return OutputBuffer[index];
        }

        public IRecordReader GetInputBuffer()
        {
            foreach (var buffer in bufferFiles)
            {
                if (buffer.Value.FileOperationType == FileOperationType.Input && InputBuffer.ContainsKey(buffer.Key))
                    return InputBuffer[buffer.Key];
            }
            
            throw new Exception("FileBuffers->GetInputBuffer: Unable to find input buffer!");
        }

        public IRecordAppender[] GetOutputBuffers()
        {
            var outputBuffers = new IRecordAppender[NumberOfOutputBuffers];
            var i = 0;
            foreach (var buffer in OutputBuffer)
            {
                if (bufferFiles[buffer.Key].FileOperationType == FileOperationType.Output)
                    outputBuffers[i++] = buffer.Value;
            }

            return outputBuffers;
        }
    }
}