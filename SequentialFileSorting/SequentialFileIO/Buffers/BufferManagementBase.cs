using FileIO.RecordIO;
using FileIO.RecordIO.Interfaces;

namespace SequentialFileIO
{
    public abstract class BufferManagementBase
    {
        protected IOutputBuffer[] outputBuffers;
        protected IInputBuffer[] inputBuffers;
        
        protected int capacity;
        protected int selectedBuffer = 0;

        public void FlushAllBuffers()
        {
            foreach (var buffer in outputBuffers)
            {
                buffer.FlushBuffer();
            }
        }

        protected void AddDummyRecord(int bufferNumber, int amount = 1)
        {
            getInputBuffer(bufferNumber).AddDummyRecord(amount);
            getOutputBuffer(bufferNumber).AddDummyRecord(amount);
        }

        protected IRecord RemoveDummyRecord(int bufferNumber)
        {
            getInputBuffer(bufferNumber).RemoveDummyRecord();
            getOutputBuffer(bufferNumber).RemoveDummyRecord();
            return Record.Dummy;
        }

        protected IOutputBuffer getOutputBuffer(int bufferNumber)
        {
            return bufferNumber >= selectedBuffer ? outputBuffers[bufferNumber + 1] : outputBuffers[bufferNumber];
        }
        
        protected IInputBuffer getInputBuffer(int bufferNumber)
        {
            return bufferNumber >= selectedBuffer ? inputBuffers[bufferNumber + 1] : inputBuffers[bufferNumber];
        }
    }
}