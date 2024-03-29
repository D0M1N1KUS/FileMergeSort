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
        public int ExpectedNumberOfRecords { private get; set; } = -1;
        private int lastDestinationBufferIndex = 0;

        public int Steps { get; private set; } = 0;

        public Merger(int numberOfInputBuffers, IMergeBufferingIO bufferIO, IRecordValueComparer comparer)
        {
            BufferIO = bufferIO;
            this.numberOfInputBuffers = numberOfInputBuffers;
            Comparer = comparer;
            lastDestinationBufferIndex = numberOfInputBuffers;
        }

        private object lockSteps;
        
        public bool FileIsSorted { get; private set; } = false;
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
            setInitialValues();
            if (FileIsSorted) return;
            
            while (BufferIO.AllHaveNextOrDummy || allCachedRecordsAreLegal)
            {
                resetPreviousValues();
                mergeNextSeries();
                SeriesMerging++;
            } 
            Steps++;
            BufferIO.SetAnyEmptyBufferAsDestinationBuffer();
        }

        private void setInitialValues()
        {
            if(ExpectedNumberOfRecords == -1) 
                ExpectedNumberOfRecords = ExpectedNumberOfRecords = BufferIO.GetSumOfRecordsInInputBuffers();
            if (currentRecords == null) currentRecords = BufferIO.GetNextRecordsFromAllBuffers();
            if(!allCachedRecordsAreLegal) replaceCachedNullRecords();
        }

        private void replaceCachedNullRecords()
        {
            for (var i = 0; i < currentRecords.Length; i++)
            {
                if (currentRecords[i].IsNull)
                    currentRecords[i] = BufferIO.GetNextRecordFrom(i);
            }
        }
        
        private void mergeNextSeries()
        {
            var iteration = 0;
            do
            {
                indexOfSmallest = Comparer.GetIndexOfSmallest(currentRecords, seriesEnded);
                BufferIO.AppendToDestinationBuffer(Comparer.SmallestRecord);
                iteration += !Comparer.SmallestRecord.IsDummy && !Comparer.SmallestRecord.IsNull ? 1 : 0;
                getNextRecord();
            } while (allCurrentSeriesHaveNotEnded);

            lastDestinationBufferIndex = BufferIO.GetDestinationBufferIndex();
            BufferIO.FlushDestinationBuffer();
            FileIsSorted = iteration == ExpectedNumberOfRecords;
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