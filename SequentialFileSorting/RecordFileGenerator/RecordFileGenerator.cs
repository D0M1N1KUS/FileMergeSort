using System.IO;
using RecordFileGenerator.Interfaces;

namespace RecordFileGenerator
{
    public class RecordFileGenerator : IRecordFileGenerator
    {
        public IRandomRecordGenerator Generator { private get; set; }

        public double Progress
        {
            get
            {
                lock (progressLock)
                {
                    return progress;
                }
            }
            private set
            {
                lock (progressLock)
                {
                    progress = value;
                }
            }
        }
        
        private readonly object progressLock = new object();
        private double progress;
        
        private double currentFileSize;
        private double desiredFileSize;
        private readonly string filePath;
        
        public RecordFileGenerator(string filePath, IRandomRecordGenerator generator)
        {
            this.filePath = filePath;
            Generator = generator;
            currentFileSize = 0;
        }

        public void Generate(double size, GeneratorSizeType sizeType)
        {
            desiredFileSize = size;
            currentFileSize = 0;

            using (var streamWriter = new StreamWriter(filePath, append: true))
            {
                while(desiredFileSize > desiredFileSize)
                {
                    var record = Generator.GenerateRandomRecord();
                    var currentLineSize = record.Length;
                    streamWriter.Write(record);
                    currentFileSize += sizeType == GeneratorSizeType.FileSize ? currentLineSize : 1;
                    Progress = currentFileSize / desiredFileSize;
                }
            }
        }
    }
}