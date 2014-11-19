using SimpleMvvmToolkit;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulator
{
    /*
     * Each process will include info about amount of memory needed and how long it will need it for
     * OS will keep track of memory
     * 3 Algorithms: First-Fit, Best-Fit, Worst-Fit (will need simulated clock)
     * Report possible memory fragmentation
     *      Terminate at first reporting of fragmentation
     *      
     */
    public abstract class Memory : ModelBase<Memory>
    {
        private int totalSize;
        public int TotalSize
        {
            get { return totalSize; }
            set
            {
                totalSize = value;
                NotifyPropertyChanged(m => m.TotalSize);
            }
        }

        private ObservableCollection<int> memoryArray;
        public ObservableCollection<int> MemoryArray
        {
            get { return memoryArray; }
            set
            {
                memoryArray = value;
                NotifyPropertyChanged(m => m.MemoryArray);
            }
        }

        public Memory(int totalSize)
        {
            TotalSize = totalSize;
            MemoryArray = new ObservableCollection<int>(Enumerable.Repeat<int>(-1, TotalSize));
        }

        public abstract bool TryAllocate(int processId, int size);

        public void ClearMemory(int processId)
        {
            for (int i = 0; i < MemoryArray.Count; i++)
            {
                if (MemoryArray[i] == processId)
                {
                    MemoryArray[i] = -1;
                }
            }
        }

        /// <summary>
        /// Returns list of available allocation block
        /// Dictionary key is the memory index
        /// Dictionary value is the allocation block's length
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, int> CalculateAllocationBlocks()
        {
            Dictionary<int, int> blocks = new Dictionary<int, int>();

            int start = 0;
            int length = 0;

            for (int i = 0; i < MemoryArray.Count; i++)
            {
                if (MemoryArray[i] == -1)
                {
                    length++;
                }
                else
                {
                    if (length > 0)
                    {
                        blocks[start] = length;
                    }

                    length = 0;
                    start = i + 1;
                }

            }

            if (length > 0)
            {
                blocks[start] = length;
            }

            return blocks;
        }
    }
}
