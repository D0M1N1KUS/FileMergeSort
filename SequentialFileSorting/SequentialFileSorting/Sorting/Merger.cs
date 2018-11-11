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
        private bool[] seriesEnded;

        public int DestinationBufferIndex => lastDestinationBufferIndex;
        private int lastDestinationBufferIndex = 0;

        private int steps = 0;

        public Merger(int numberOfInputBuffers, IMergeBufferingIO bufferIO, IRecordValueComparer comparer)
        {
            BufferIO = bufferIO;
            this.numberOfInputBuffers = numberOfInputBuffers;
            Comparer = comparer;
        }

        private object lockSteps;
        
        public bool FileIsSorted { get; private set; } = false;
        public int Steps {
            get
            {
                lock (lockSteps)
                {
                    return steps;
                }
            }
            private set
            {
                lock (lockSteps)
                {
                    steps = value;
                }
            }
        }
        public int SeriesMerging { get; private set; } = 0;
        
        private bool allCurrentSeriesHaveNotEnded => 
            !seriesEnded.Aggregate(true, (current, boolean) => current && boolean);
        private bool allCachedRecordsAreLegal => 
            currentRecords.Aggregate(true, (current, record) => current && !record.IsNull);

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
            if(!allCachedRecordsAreLegal) replaceCachedNullRecords();
            
            while (BufferIO.AllHaveNextOrDummy || allCachedRecordsAreLegal)
            {
                resetPreviousValues();
                mergeNextSeries();
                SeriesMerging++;
            } 
            steps++;
            BufferIO.SetAnyEmptyBufferAsDestinationBuffer();
            FileIsSorted = BufferIO.AllOutputBuffersAreEmpty;
        }

        private void replaceCachedNullRecords()
        {
            for (var i = 0; i < currentRecords.Length; i++)
            {
                if (currentRecords[i].IsNull)
                    currentRecords[i] = BufferIO.GetNextRecordFrom(i);
            }
        }
        
        //after the buffer destination buffer swap, the indexes get mixed up, which causes an infinite loop of
        //null records
        private void mergeNextSeries()
        {
            do
            {
                indexOfSmallest = Comparer.GetIndexOfSmallest(currentRecords, seriesEnded);
                BufferIO.AppendToDestinationBuffer(Comparer.SmallestRecord);
                getNextRecord();
            } while (allCurrentSeriesHaveNotEnded);

            lastDestinationBufferIndex = BufferIO.GetDestinationBufferIndex();
            BufferIO.FlushDestinationBuffer();
        }

        private void getNextRecord()
        {
            previousRecords[indexOfSmallest] = currentRecords[indexOfSmallest];
            currentRecords[indexOfSmallest] = BufferIO.GetNextRecordFrom(indexOfSmallest);
            if ((Record) currentRecords[indexOfSmallest] < previousRecords[indexOfSmallest] || 
                currentRecords[indexOfSmallest].IsNull)
                seriesEnded[indexOfSmallest] = true;
        }

        private void resetPreviousValues()
        {
            previousRecords = Enumerable.Repeat(Record.Min, currentRecords.Length).ToArray();
            seriesEnded = Enumerable.Repeat(false, BufferIO.NumberOfTemporaryBuffers).ToArray();

            seriesIsContinuous = true;
            lastAppended = Record.NullRecord;
        }
    }
}