using Microsoft.SqlServer.Server;
using SequentialFileIO.Enums;

namespace SequentialFileIO
{
    public struct SequentialFile : ISequentialFile
    {
        public string FilePath { get; }
        public int Sequences { get; set; }
        public FileOperationType FileOperationType { get; set; }

        public SequentialFile(string filePath, FileOperationType fileOperationType, int sequences = 0)
        {
            this.FilePath = filePath;
            FileOperationType = fileOperationType;
            Sequences = sequences;

        }
    }
}