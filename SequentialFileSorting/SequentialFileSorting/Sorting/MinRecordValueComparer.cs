using System.Collections;
using System.Collections.Generic;
using FileIO.RecordIO;
using FileIO.RecordIO.Interfaces;
using SequentialFileIO;

namespace SequentialFileSorting.Sorting
{
    public class MinRecordValueComparer : IRecordValueComparer
    {
        private List<IRecord> comparisonList = new List<IRecord>();
        private int indexOfSmallest = -1;
        private bool collectionChanged = false;
        
        public IRecord SmallestRecord { get; private set; } = Record.Max;


        public int GetIndexOfSmallest(params IRecord[] recordsList)
        {
            SmallestRecord = Record.Max;
            var indexOfSmallest = -1;
            for (var i = 0; i < recordsList.Length; i++)
            {
                if ((Record)recordsList[i] < SmallestRecord)
                {
                    SmallestRecord = recordsList[i];
                    indexOfSmallest = i;
                }
            }

            return indexOfSmallest;
        }

        public void AddRecordToComparison(IRecord record)
        {
            comparisonList.Add(record);
            collectionChanged = true;
        }

        public int GetIndexOfSmallest()
        {
            if (!collectionChanged) return indexOfSmallest;
            
            indexOfSmallest = -1;
            SmallestRecord = Record.Max;
            for (var i = 0; i < comparisonList.Count; i++)
            {
                if ((Record)comparisonList[i] < SmallestRecord)
                {
                    SmallestRecord = comparisonList[i];
                    indexOfSmallest = i;
                }
            }

            return indexOfSmallest;
        }

        public void Reset()
        {
            comparisonList = new List<IRecord>();
            collectionChanged = false;
        }
    }
}