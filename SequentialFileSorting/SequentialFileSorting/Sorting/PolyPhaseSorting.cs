using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;
using SequentialFileSorting.Interfaces;
using SequentialFileSorting.SortingManagment;

namespace SequentialFileSorting.Sorting
{
    public class PolyPhaseSorting : ObjectManager
    {
        private IFrontendCallback frontendCallback;
        
        private bool successfullyDistributed = false;
        
        private Task distributionTask;
        private Task stepTask;
        private Task mergeTask;
        
        public int Steps => Merger.Steps;
        public long ReadAccesses => readAccessStatistics.Sum(statistic => statistic.NumberOfAccesses);
        public long WriteAccesses => writeAccessStatistics.Sum(statistic => statistic.NumberOfAccesses);
        
        public PolyPhaseSorting(SortingParameters sortingParameters, FileParameters fileParameters)
        {
            CreateSortingObjects(sortingParameters, fileParameters);
            distributionTask = new Task(distribute);
            stepTask = new Task(step);
            mergeTask = new Task(merge);
        }

        public void Distribute()
        {
            if (!successfullyDistributed)
            {
                distributionTask.Start();
            }
        }

        private void distribute()
        {
            Distribution.Distribute();
            successfullyDistributed = true;
        }

        public void Step()
        {
            if (!successfullyDistributed) throw new Exception("Series not distributed yet!");
            stepTask.Start();
        }

        private void step()
        {
            Merger.Step();
        }

        public void Sort()
        {
            if(!successfullyDistributed) throw new Exception("Series not distributed yet!");
            mergeTask.Start();
        }

        private void merge()
        {
            Merger.Merge();
        }

        public void RestoreOriginalFileName()
        {
            PostSorting.SwapFileNames(fileBases[Merger.DestinationBufferIndex].FilePath, FileParameters.SourceFileName);
        }

        public string GetCurrentDestinationFilePath()
        {
            return fileBases[Merger.DestinationBufferIndex].FilePath;
        }
    }
}