using System.Collections.Generic;
using FileIO.Interfaces;
using FileIO.RecordIO.Interfaces;

namespace SequentialFileIO
{
    public class FileBuffers : IFileBuffers
    {
        public IRecordReader[] InputBuffer { get; }
        public IRecordAppender[] OutputBuffer { get; }

        private List<SequentialFile> bufferFiles;
        
        public FileBuffers(IFileReader initialSourceFile, int numberOfBuffers)
        {
            //I was thinking about the sense of this class. I think i should make a distrubutor first
        }

    }
}