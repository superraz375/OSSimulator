using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulator
{
    public class FirstFitMemory : Memory
    {

        /// <summary>
        /// First Fit Memory Allocation Algorithm
        /// 
        /// Fits memory into the first available block
        /// </summary>
        public FirstFitMemory(int totalSize)
            : base(totalSize)
        {

        }

        public override int TryAllocate(int processId, int size)
        {
            // Get the allocation blocks
            var blocks = CalculateAllocationBlocks();

            // Get available allocation blocks that can fit the specified memory size
            var availableBlocks = blocks.Where(block => block.Value >= size);

            if (availableBlocks.Count() > 0)
            {

                // Allocate the memory
                var selectedBlock = availableBlocks.First();
                for (int i = 0; i < size; i++)
                {
                    MemoryArray[i + selectedBlock.Key] = processId;
                }
                return selectedBlock.Key;
            }
            else if (blocks.Sum(d => d.Value) >= size)
            {
                // Fragmentation
                return -2;
            }

            // Not enough memory, wait.
            return -1;
        }
    }
}
