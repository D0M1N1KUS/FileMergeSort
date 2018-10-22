using FileIO.RecordIO.Interfaces;

namespace SequentialFileIO
{
    public interface IFileBuffers
    {
        IRecordReader[] InputBuffer { get; }
        IRecordAppender[] OutputBuffer { get; }
    }
}