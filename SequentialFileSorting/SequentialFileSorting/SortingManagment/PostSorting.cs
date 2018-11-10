using System.IO;
using SequentialFileIO;

namespace SequentialFileSorting.SortingManagment
{
    public static class PostSorting
    {
        public static void SwapFileNames(string pathToFile1, string pathToFile2)
        {
            if (pathToFile1 == pathToFile2) return;
            var temporaryFileNameGenerator = new TemporaryFileNameGenerator();
            var tempName = temporaryFileNameGenerator.GetNextAvailableName();
            File.Move(pathToFile1, tempName);
            File.Move(pathToFile2, pathToFile1);
            File.Move(tempName, pathToFile1);
        }
    }
}