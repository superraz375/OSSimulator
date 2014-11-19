using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulator
{
    public class WorstFitMemory : Memory
    {
        public WorstFitMemory(int totalSize)
            : base(totalSize)
        {

        }
        public override bool TryAllocate(int processId, int size)
        {
            var blocks = CalculateAllocationBlocks();
            var availableBlocks = blocks.Where(block => block.Value >= size).OrderByDescending(block => block.Value);

            if (availableBlocks.Count() > 0)
            {
                var selectedBlock = availableBlocks.First();
                for (int i = 0; i < size; i++)
                {
                    MemoryArray[i + selectedBlock.Key] = processId;
                }
                return true;
            }

            return false;
        }
    }
}