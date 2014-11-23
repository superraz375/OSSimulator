using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleMvvmToolkit;
using System.Collections.ObjectModel;

namespace Simulator
{
    /// <summary>
    /// Model that represents the OS Simulator
    /// </summary>
    public class SimulatorModel : ModelBase<SimulatorModel>
    {

        /// <summary>
        /// List of options for the ProcessStatus enum type
        /// </summary>
        private ObservableCollection<ProcessStatus> processTypes;
        public ObservableCollection<ProcessStatus> ProcessTypes
        {
            get { return processTypes; }
            set
            {
                processTypes = value;
                NotifyPropertyChanged(m => m.ProcessTypes);
            }
        }
        
        /// <summary>
        /// Selected ProcessStatus from the GUI
        /// </summary>
        private ProcessStatus selectedProcessType;
        public ProcessStatus SelectedProcessType
        {
            get { return selectedProcessType; }
            set
            {
                selectedProcessType = value;
                NotifyPropertyChanged(m => m.SelectedProcessType);
            }
        }

        private ObservableCollection<MemoryType> memoryTypes;
        /// <summary>
        /// List of available memory allocation types
        /// </summary>
        public ObservableCollection<MemoryType> MemoryTypes
        {
            get { return memoryTypes; }
            set
            {
                memoryTypes = value;
                NotifyPropertyChanged(m => m.MemoryTypes);
            }
        }

        private MemoryType selectedMemoryType;
        /// <summary>
        /// The selected memory allocation type
        /// </summary>
        public MemoryType SelectedMemoryType
        {
            get { return selectedMemoryType; }
            set
            {
                selectedMemoryType = value;
                NotifyPropertyChanged(m => m.SelectedMemoryType);
                ChangeMemory();
            }
        }

        private int memorySize;
        /// <summary>
        /// Total memory size
        /// </summary>
        public int MemorySize
        {
            get { return memorySize; }
            set
            {
                memorySize = value;
                NotifyPropertyChanged(m => m.MemorySize);
                ChangeMemory();
            }
        }

        /// <summary>
        /// The queue that holds the waiting queue processes
        /// </summary>
        private ObservableCollection<PCB> waitingQueue;
        public ObservableCollection<PCB> WaitingQueue
        {
            get { return waitingQueue; }
            set
            {
                waitingQueue = value;
                NotifyPropertyChanged(m => m.WaitingQueue);
            }
        }

        /// <summary>
        /// The queue that holds the ready queue processes
        /// </summary>
        private ObservableCollection<PCB> readyQueue;
        public ObservableCollection<PCB> ReadyQueue
        {
            get { return readyQueue; }
            set
            {
                readyQueue = value;
                NotifyPropertyChanged(m => m.ReadyQueue);
            }
        }

        /// <summary>
        /// The currently executing PCB
        /// </summary>
        private PCB executingPcb;
        public PCB ExecutingPcb
        {
            get { return executingPcb; }
            set
            {
                executingPcb = value;
                NotifyPropertyChanged(m => m.ExecutingPcb);
            }
        }


        private int simulationDelay;
        public int SimulationDelay
        {
            get { return simulationDelay; }
            set
            {
                simulationDelay = value;
                NotifyPropertyChanged(m => m.SimulationDelay);
            }
        }

        private bool isEnabled;
        public bool IsEnabled
        {
            get { return isEnabled; }
            set
            {
                isEnabled = value;
                NotifyPropertyChanged(m => m.IsEnabled);
            }
        }

        private Memory memory;
        public Memory Memory
        {
            get { return memory; }
            set
            {
                memory = value;
                NotifyPropertyChanged(m => m.Memory);
            }
        }

        private int clock;
        public int Clock
        {
            get { return clock; }
            set
            {
                clock = value;
                NotifyPropertyChanged(m => m.Clock);
            }
        }

        private bool enableMemoryScreenshots;
        public bool EnableMemoryScreenshots
        {
            get { return enableMemoryScreenshots; }
            set
            {
                enableMemoryScreenshots = value;
                NotifyPropertyChanged(m => m.EnableMemoryScreenshots);
            }
        }

        /// <summary>
        /// Updates the memory with the correct algorithm and size
        /// </summary>
        public void ChangeMemory()
        {
            switch (SelectedMemoryType)
            {
                case MemoryType.FIRST_FIT:
                    Memory = new FirstFitMemory(MemorySize);
                    break;
                case MemoryType.BEST_FIT:
                    Memory = new BestFitMemory(MemorySize);
                    break;
                case MemoryType.WORST_FIT:
                    Memory = new WorstFitMemory(MemorySize);
                    break;
                default:
                    break;
            }

            Console.WriteLine("Memory Allocation Algorithm: {0}", SelectedMemoryType.ToString());
            Console.WriteLine("Memory Size: {0}", MemorySize);
        }

        public SimulatorModel()
        {

            // Initialize Model data
            WaitingQueue = new ObservableCollection<PCB>();
            ReadyQueue = new ObservableCollection<PCB>();
            ProcessTypes = new ObservableCollection<ProcessStatus>() {
                ProcessStatus.WAITING,
                ProcessStatus.READY
            };
            SelectedProcessType = ProcessTypes[0];

            SimulationDelay = 300;
            IsEnabled = true;

            MemorySize = 16;
            MemoryTypes = new ObservableCollection<MemoryType>() {
                MemoryType.FIRST_FIT,
                MemoryType.BEST_FIT,
                MemoryType.WORST_FIT
            };
            SelectedMemoryType = MemoryTypes[0];

            

        }
    }
}
