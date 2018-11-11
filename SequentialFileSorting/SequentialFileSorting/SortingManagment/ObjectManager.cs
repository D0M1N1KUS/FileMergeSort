using System;
using System.Diagnostics;
using FileIO;
using FileIO.Builders;
using FileIO.Interfaces;
using FileIO.RecordIO;
using FileIO.RecordIO.Interfaces;
using FileIO.Writers;
using FileIO.Writers.Interfaces;
using SequentialFileIO;
using SequentialFileSorting.Sorting;

namespace SequentialFileSorting.SortingManagment
{
    public class ObjectManager
    {
        public IMerging Merger { get; protected set; }
        public IDistribution Distribution { get; protected set; }
        
        protected SortingParameters SortingParameters;
        protected FileParameters FileParameters;
        
        protected IDistributionBufferingIO DistributionBuffering;
        protected IMergeBufferingIO MergeBuffering;

        protected IOutputBuffer[] outputBuffers;
        protected IInputBuffer[] inputBuffers;
        protected IFileIOBase[] fileBases;
        
        protected IRecordReader[] recordReaders;
        protected IFileReader[] fileReaders;

        protected IRecordAppender[] recordAppenders;
        protected IFileWriter[] fileWriters;

        protected IStatistics[] readAccessStatistics;
        protected IStatistics[] writeAccessStatistics;

        protected int numberOfFiles;
        protected int indexOfSourceFile => numberOfFiles - 1;
        

        public void CreateSortingObjects(SortingParameters sortingParameters, FileParameters fileParameters)
        {
            SortingParameters = sortingParameters;
            FileParameters = fileParameters;
            numberOfFiles = SortingParameters.NumberOfTemporaryFiles + 1;
            initializeFileBasesOfTempFiles();
            setUpFileReaders();
            setUpFileWriters();
            setUpRecordReaders();
            setUpRecordAppenders();
            setUpInputBuffers();
            setUpOutputBuffers();
            buildDistributionBuffering();
            buildMergeBuffering();
            buildDistributionObject();
            buildMergeObject();
        }

        ~ObjectManager()
        {
            foreach (var file in fileBases)
            {
                if(file.FilePath != FileParameters.SourceFileName)
                    file.DeleteFile();
            }
        }


        protected void initializeFileBasesOfTempFiles()
        {
            fileBases = new IFileIOBase[numberOfFiles];
            var fileNameGenerator = new TemporaryFileNameGenerator("TmpBuffer", "buf", 
                FileParameters.TemporaryBufferFileDirectory);
            
            for (var i = 0; i < numberOfFiles; i++)
            {
                if (i == indexOfSourceFile) fileBases[i] = new FileIOBasics(FileParameters.SourceFileName, FileParameters.BlockSize);
                else fileBases[i] = 
                    new FileIOBasics(fileNameGenerator.GetNextAvailableName(), FileParameters.BlockSize, true);
            }
        }

        protected void setUpFileReaders()
        {
            fileReaders = new IFileReader[numberOfFiles];
            readAccessStatistics = new IStatistics[numberOfFiles];
            for (var i = 0; i < numberOfFiles; i++)
            {
                var reader = new FileReaderBuilder()
                    .SetFileBase(fileBases[i])
                    .Build();
                fileReaders[i] = reader;
                readAccessStatistics[i] = (FileReader)reader;
            }
        }

        protected void setUpFileWriters()
        {
            fileWriters = new IFileWriter[numberOfFiles];
            writeAccessStatistics = new IStatistics[numberOfFiles];
            for (var i = 0; i < numberOfFiles; i++)
            {
                var writer = new FileWriterBuilder()
                    .SetFileBase(fileBases[i])
                    .CreateNewFile(false)
                    .Build();
                fileWriters[i] = writer;
                writeAccessStatistics[i] = (BlockWriter)writer;
            }
        }

        protected void setUpRecordReaders()
        {
            recordReaders = new IRecordReader[numberOfFiles];
            for (var i = 0; i < numberOfFiles; i++)
            {
                recordReaders[i] = new LineBasedRecordReader(fileReaders[i], new ValueComponentsSplitter());

            }
        }

        protected void setUpRecordAppenders()
        {
            recordAppenders = new IRecordAppender[numberOfFiles];
            for (var i = 0; i < numberOfFiles; i++)
            {
                recordAppenders[i] = new RecordAppender(fileWriters[i]);
            }
        }

        protected void setUpInputBuffers()
        {
            inputBuffers = new IInputBuffer[numberOfFiles];
            for (var i = 0; i < numberOfFiles; i++)
            {
                inputBuffers[i] = new InputBuffer(recordReaders[i]);
            }
        }

        protected void setUpOutputBuffers()
        {
            outputBuffers = new IOutputBuffer[numberOfFiles];
            for (var i = 0; i < numberOfFiles; i++)
            {
                outputBuffers[i] = new OutputBuffer(recordAppenders[i], fileWriters[i]);
            }
        }

        protected void buildDistributionBuffering()
        {
            DistributionBuffering = new DistributionBufferingIO(ref inputBuffers,
                ref outputBuffers, indexOfSourceFile);
        }

        protected void buildMergeBuffering()
        {
            MergeBuffering = new MergeBufferingIO(ref inputBuffers, ref outputBuffers, indexOfSourceFile);
        }

        protected void buildDistributionObject()
        {
            Distribution = new Distribution(SortingParameters.NumberOfTemporaryFiles, DistributionBuffering,
                PreSorting.GetNumberOfSeries(FileParameters.SourceFileName));
        }

        protected void buildMergeObject()
        {
            Merger = new Merger(SortingParameters.NumberOfTemporaryFiles, MergeBuffering, new MinRecordValueComparer());
        }
    }
}