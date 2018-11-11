using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FileIO.RecordIO;
using FileIO.RecordIO.Interfaces;
using SequentialFileIO;

namespace SequentialFileSorting.Sorting
{
    public class MinRecordValueComparer : IRecordValueComparer
    {
        private List<IRecord> comparisonList = new List<IRecord>();
        private List<bool> seriesEndedList = new List<bool>();
        public int IndexOfSmallest { get; private set; } = -1;
        private bool collectionChanged = false;
        
        public IRecord SmallestRecord { get; private set; } = Record.Max;

        public int GetIndexOfSmallest(IRecord[] recordsList, bool[] seriesEndedList)
        {
            SmallestRecord = Record.Max;
            IndexOfSmallest = -1;
            for (var i = 0; i < recordsList.Length; i++)
            {
                if (recordIsNotNull(recordsList[i]) && currentSmallestRecordIsBiggerOrDummy(recordsList[i]) && 
                    !seriesEndedList[i])
                {
                    SmallestRecord = currentRecordIsNotDummyOrSmallestHasNotBeenSet(recordsList[i])
                        ? recordsList[i]
                        : SmallestRecord;
                    IndexOfSmallest = currentRecordIsNotDummyOrIndexOfSmallestHasNotBeenSet(recordsList[i])
                        ? i
                        : IndexOfSmallest;
                }
            }
            
            if(IndexOfSmallest == -1)
                throw new Exception("MinRecordValueComparer->GetIndexOfSmallest([" + 
                                    string.Join("]\n[ ",recordsList.Select(v => v.Value.ToString())) + 
                                    "]): Invalid array provided!");

            return IndexOfSmallest;
        }

        private bool currentRecordIsNotDummyOrIndexOfSmallestHasNotBeenSet(IRecord r)
        {
            return !r.IsDummy || IndexOfSmallest == -1;
        }

        private bool currentRecordIsNotDummyOrSmallestHasNotBeenSet(IRecord r)
        {
            return !r.IsDummy || SmallestRecord.Equals(Record.Max) || SmallestRecord.Equals(Record.Dummy);
        }

        private bool currentSmallestRecordIsBiggerOrDummy(IRecord r)
        {
            return ((Record)r < SmallestRecord || SmallestRecord.Equals(Record.Dummy));
        }

        private static bool recordIsNotNull(IRecord r)
        {
            return !r.IsNull;
        }

        public void AddRecordToComparison(IRecord record, bool seriesEnded)
        {
            comparisonList.Add(record);
            seriesEndedList.Add(seriesEnded);
            collectionChanged = true;
        }

        public int GetIndexOfSmallest()
        {
            return !collectionChanged ? IndexOfSmallest : GetIndexOfSmallest(comparisonList.ToArray(), 
                seriesEndedList.ToArray());
        }

        public void Reset()
        {
            comparisonList = new List<IRecord>();
            collectionChanged = false;
        }
    }
}