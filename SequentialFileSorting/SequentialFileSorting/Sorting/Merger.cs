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
            steps++;

            FileIsSorted = BufferIO.AllOutputBuffersAreEmpty;
        }
        
        //the merger does not work properly. when a series ends it keeps getting values from the wrong buffer,
        //as long as there are smaller values present
        //getting the next element is not supposed to work that way too. when the series has ended somewhere,
        //the already read value should not be used for comparison
        private void mergeNextSeries()
        {
            do
            {
                indexOfSmallest = Comparer.GetIndexOfSmallest(currentRecords);
                BufferIO.AppendToDestinationBuffer(Comparer.SmallestRecord);
                //previousRecords[indexOfSmallest] = currentRecords[indexOfSmallest];
                //currentRecords[indexOfSmallest] = BufferIO.GetNextRecordFrom(indexOfSmallest);
                getNext(indexOfSmallest);
            } while (allCurrentSeriesHaveNotEnded());

            lastDestinationBufferIndex = BufferIO.GetDestinationBufferIndex();
            BufferIO.SetAnyEmptyBufferAsDestinationBuffer();
        }

        private void getNext(int indexOfCurrentSmallest)
        {
            var iteration = 0;
            var i = indexOfCurrentSmallest;
            while (iteration < BufferIO.NumberOfTemporaryBuffers)
            {
                if (!((Record) currentRecords[i] < previousRecords[i]))
                {
                    previousRecords[i] = currentRecords[i];
                    currentRecords[i] = BufferIO.GetNextRecordFrom(i);
                    return;
                }
                i = i + 1 % BufferIO.NumberOfTemporaryBuffers;
                iteration++;
            }
        }
        
        private bool allCurrentSeriesHaveNotEnded()
        {
            var endOfAllSeries = true;
            for (var i = 0; i < currentRecords.Length; i++)
            {
                endOfAllSeries =
                    endOfAllSeries && (Record)currentRecords[i] < previousRecords[i] || 
                    currentRecords[i].IsDummy || currentRecords[i].IsNull;
            }

            return !endOfAllSeries;
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