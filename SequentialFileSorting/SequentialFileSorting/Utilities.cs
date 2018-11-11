using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sortowanie_rekordów
{
    public static class Utilities
    {
        public static long getNumberOfLinesInFile(string path)
        {
            var lineCount = 0;
            using (var reader = File.OpenText(path))
            {
                var line = reader.ReadLine();
                while (line != null && line != string.Empty && line.Length >= 10 && line[9] != '\0')
                {
                    lineCount++;
                    line = reader.ReadLine();
                }
            }
            return lineCount;
        }

        public static bool pathIsDirectory(string path)
        {
            FileAttributes Attributes = File.GetAttributes(path);
            if ((Attributes & FileAttributes.Directory) == FileAttributes.Directory)
            {
                
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}
