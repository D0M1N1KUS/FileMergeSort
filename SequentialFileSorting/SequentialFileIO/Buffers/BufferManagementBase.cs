namespace SequentialFileIO
{
    public class BufferManagementBase
    {
        protected IOutputBuffer[] outputBuffers;
        protected IInputBuffer[] inputBuffers;
        
        protected int capacity;
        protected int selectedBuffer = 0;
    }
}