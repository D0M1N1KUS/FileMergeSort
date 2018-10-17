namespace FileIO.Writers.Interfaces
{
    public interface IBlockSplitter
     {
         string ExcessText { get; }
         
         string[] GetBlocks(string text, bool useExcessText = false);
     }
 }