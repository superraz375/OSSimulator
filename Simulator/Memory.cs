using SimpleMvvmToolkit;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulator
{
    
    public abstract class Memory : ModelBase<Memory>
    {
        private int totalSize;
        /// <summary>
        /// Total size of the memory
        /// </summary>
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
        /// <summary>
        /// Array of memory allocations
        /// </summary>
        public ObservableCollection<int> MemoryArray
        {
            get { return memoryArray; }
            set
            {
                memoryArray = value;
                NotifyPropertyChanged(m => m.MemoryArray);
            }
        }

        /// <summary>
        /// Initializes the memory
        /// </summary>
        /// <param name="totalSize"></param>
        public Memory(int totalSize)
        {
            TotalSize = totalSize;
            MemoryArray = new ObservableCollection<int>(Enumerable.Repeat<int>(-1, TotalSize));
        }

        /// <summary>
        /// Each subclass will implement a unique TryAllocate method for its implementation
        /// </summary>
        /// <param name="processId"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public abstract int TryAllocate(int processId, int size);

        /// <summary>
        /// Clears the memory for the specified process ID
        /// </summary>
        /// <param name="processId"></param>
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
                // Increase count for empty block
                if (MemoryArray[i] == -1)
                {
                    length++;
                }
                else
                {
                    // Space is taken, restart count of empty block
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

            // Return the blocks
            return blocks;
        }
    }
}
