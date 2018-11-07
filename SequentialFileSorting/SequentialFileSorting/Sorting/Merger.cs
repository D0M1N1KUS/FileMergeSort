using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using FileIO.RecordIO;
using FileIO.RecordIO.Interfaces;
using SequentialFileIO;

namespace SequentialFileSorting.Sorting
{
    public class Merger : IMerging
    {
        public IMergeBufferingIO BufferIO;
        public IRecordValueComparer Comparer;

        private int numberOfInputBuffers;

        private IRecord lastAppended = Record.NullRecord;
        private bool seriesIsContinuous = true;
        private int indexOfSmallest = -1;
        private IRecord[] currentRecords;
        private IRecord[] previousRecords;

        public Merger(int numberOfInputBuffers, IMergeBufferingIO bufferIO, IRecordValueComparer comparer)
        {
            BufferIO = bufferIO;
            this.numberOfInputBuffers = numberOfInputBuffers;
            Comparer = comparer;
        }

        public bool FileIsSorted { get; private set; } = false;
        public int Steps { get; private set; } = 0;
        public int SeriesMerging { get; private set; } = 0;

        public void Merge()
        {
            while(!FileIsSorted)
            {
                Step();
            }
        }
        
        public void Step()
        {
            if (FileIsSorted) return;
            if (currentRecords == null) currentRecords = BufferIO.GetNextRecordsFromAllBuffers();
            
            while (BufferIO.AllHaveNextOrDummy)
            {
                resetPreviousValues();
                mergeNextSeries();
                SeriesMerging++;
            } 
            Steps++;

            FileIsSorted = BufferIO.AllOutputBuffersAreEmpty;
        }

        private void mergeNextSeries()
        {
            do
            {
                indexOfSmallest = Comparer.GetIndexOfSmallest(currentRecords);
                BufferIO.AppendToDestinationBuffer(Comparer.SmallestRecord);
                previousRecords[indexOfSmallest] = currentRecords[indexOfSmallest];
                currentRecords[indexOfSmallest] = BufferIO.GetNextRecordFrom(indexOfSmallest);
            } while (allCurrentSeriesHaveNotEnded());
            
            BufferIO.SetAnyEmptyBufferAsDestinationBuffer();
        }
        
        private bool allCurrentSeriesHaveNotEnded()
        {
            var allPreciousValuesAreLarger = true;
            for (var i = 0; i < currentRecords.Length; i++)
            {
                allPreciousValuesAreLarger =
                    allPreciousValuesAreLarger && currentRecords[i].Value < previousRecords[i].Value || 
                    currentRecords[i].IsDummy || currentRecords[i].IsNull;
            }

            return allPreciousValuesAreLarger;
        }

        
        private void resetPreviousValues()
        {
            previousRecords = new IRecord[currentRecords.Length];
            for (var i = 0; i < previousRecords.Length; i++)
            {
                previousRecords[i] = Record.Min;
            }

            seriesIsContinuous = true;
            lastAppended = Record.NullRecord;
        }
    }
}