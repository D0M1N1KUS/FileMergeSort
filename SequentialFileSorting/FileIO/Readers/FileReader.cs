using FileIO.Interfaces;

namespace FileIO
{
    public class FileReader : IFileReader
    {
        private IBlockReader blockReader;

        public FileReader(string pathToFile, IBlockReader reader = null)
        {
            blockReader = reader ?? new BlockReader(pathToFile);
        }
        
        public string GetNextLine()
        {
            return string.Empty;
        }
    }
}