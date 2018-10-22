using Microsoft.SqlServer.Server;
using SequentialFileIO.Enums;

namespace SequentialFileIO
{
    public struct SequentialFile : ISequentialFile
    {
        public string FilePath { get; }
        public int Sequences { get; set; }
        public SequentialFileType FileType { get; set; }

        public SequentialFile(string filePath, SequentialFileType fileType, int sequences = 0)
        {
            this.FilePath = filePath;
            FileType = fileType;
            Sequences = sequences;

        }
    }
}