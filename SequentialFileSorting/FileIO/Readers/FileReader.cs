using System;
using System.Collections.Generic;
using FileIO.Interfaces;

namespace FileIO
{
    public class FileReader : IFileReader, IStatistics
    {
        public bool EndOfFile => BlockReader.EndOfFile;

        public long NumberOfAccesses { get; private set; } = 0;
        public IBlockReader BlockReader { get; set; }
        public ILineSeparator LineSeparator { get; set; }

        private string[] foundLines;
        private string readText = string.Empty;
        private Queue<string> foundLinesQueue = new Queue<string>();

        private bool linesQueueIsEmptyAndFileHasntEnded => foundLinesQueue.Count <= 0 && !BlockReader.EndOfFile;

        public FileReader(IBlockReader blockReader = null, ILineSeparator lineSeparator = null)
        {
            BlockReader = blockReader;
            LineSeparator = lineSeparator;
        }


        public string GetNextLine()
        {
            while (linesQueueIsEmptyAndFileHasntEnded)
            {
                readText += BlockReader.GetNextBlock();
                if (LineSeparator.SeparateLines(readText, out foundLines) > 0)
                {
                    enqueueLines();
                    readText = LineSeparator.SeparationExcess;
                }
            }

            NumberOfAccesses++;
            
            return foundLinesQueue.Count > 0
                ? foundLinesQueue.Dequeue()
                : readText.Replace(Environment.NewLine, string.Empty);
        }

        private void enqueueLines()
        {
            foreach (var line in foundLines)
            {
                foundLinesQueue.Enqueue(line);
            }
        }

    }
}