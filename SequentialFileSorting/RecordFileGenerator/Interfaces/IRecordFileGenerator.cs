namespace RecordFileGenerator.Interfaces
{
    public interface IRecordFileGenerator
    {
        IRandomRecordGenerator Generator { set; }
        double Progress { get; }
        
        void Generate(double size, GeneratorSizeType sizeType);
    }
}