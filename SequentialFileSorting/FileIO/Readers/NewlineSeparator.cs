using System;
using System.Collections.Generic;
using System.Linq;
using FileIO.Interfaces;

namespace FileIO
{
    public class NewlineSeparator : ILineSeparator
    {
        public string SeparationExcess => excess;
        private string excess = string.Empty;
        
        public int SeparateLines(string block, out string[] separatedLines)
        {
            excess = block;
            separatedLines = new string[0];
            
            if (string.IsNullOrEmpty(block))
                return 0;

            if (block.Contains(System.Environment.NewLine))
            {
                var linesList = block.Split(new[] {Environment.NewLine}, StringSplitOptions.None);
                separatedLines = linesList.Take(linesList.Length - 1).ToArray();
                excess = linesList.Last();
                return separatedLines.Length;
            }
            
            return 0;
        }
    }
}