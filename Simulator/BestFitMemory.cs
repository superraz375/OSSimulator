using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulator
{
    /// <summary>
    /// Best Fit Memory Allocation Algorithm
    /// 
    /// Fits memory into the smallest available block
    /// </summary>
    public class BestFitMemory : Memory
    {
        public BestFitMemory(int totalSize)
            : base(totalSize) { }

        public override int TryAllocate(int processId, int size)
        {
            // Get the allocation blocks
            var blocks = CalculateAllocationBlocks();

            // Get available allocation blocks sorted by SIZE ASCENDING (smallest blocks first) that can fit the specified memory size
            var availableBlocks = blocks.Where(block => block.Value >= size).OrderBy(block => block.Value);

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

            // Cannot allocate the memory
            return -1;
        }
    }
}
