using System.Diagnostics.Eventing.Reader;
using SequentialFileIO.Enums;

namespace SequentialFileIO
{
    public interface ISequentialFile
    {
        string FilePath { get; }
        int Sequences { get; set; }
        SequentialFileType FileType { get; set; }
    }
}