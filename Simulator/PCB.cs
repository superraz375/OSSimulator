using SimpleMvvmToolkit;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulator
{
    public class PCB : ModelBase<PCB>
    {
        /// <summary>
        /// The process ID
        /// </summary>
        private int processId;
        /// <summary>
        /// The process ID
        /// </summary>
        public int ProcessId
        {
            get { return processId; }
            set
            {
                processId = value;
                NotifyPropertyChanged(m => m.ProcessId);
            }
        }

        /// <summary>
        /// The status of the current process
        /// </summary>
        private ProcessStatus status;
        /// <summary>
        /// The status of the current process
        /// </summary>
        public ProcessStatus Status
        {
            get { return status; }
            set
            {
                status = value;
                NotifyPropertyChanged(m => m.Status);
            }
        }

        /// <summary>
        /// List of all the CPU registers in the PCB
        /// </summary>
        private ObservableCollection<int> registers;
        /// <summary>
        /// List of all the CPU registers in the PCB
        /// </summary>
        public ObservableCollection<int> Registers
        {
            get { return registers; }
            set
            {
                registers = value;
                NotifyPropertyChanged(m => m.Registers);
            }
        }

        /// <summary>
        /// Current program counter for the process
        /// </summary>
        private int programCounter;
        /// <summary>
        /// Current program counter for the process
        /// </summary>
        public int ProgramCounter
        {
            get { return programCounter; }
            set
            {
                programCounter = value;
                NotifyPropertyChanged(m => m.ProgramCounter);
            }
        }

        private int totalMemoryAllocated;
        public int TotalMemoryAllocated
        {
            get { return totalMemoryAllocated; }
            set
            {
                totalMemoryAllocated = value;
                NotifyPropertyChanged(m => m.TotalMemoryAllocated);
            }
        }

        private int timeNeededForMemory;
        public int TimeNeededForMemory
        {
            get { return timeNeededForMemory; }
            set
            {
                timeNeededForMemory = value;
                NotifyPropertyChanged(m => m.TimeNeededForMemory);
            }
        }


        /// <summary>
        /// Milliseconds of CPU time used
        /// </summary>
        public int CPUTimeUsed
        {
            get { return EndTime - StartTime; }
        }

        /// <summary>
        /// Start time of process execution
        /// </summary>
        private int startTime;
        /// <summary>
        /// Start time of process execution
        /// </summary>
        public int StartTime
        {
            get { return startTime; }
            set
            {
                startTime = value;
                NotifyPropertyChanged(m => m.StartTime);
                NotifyPropertyChanged(m => m.CPUTimeUsed);
            }
        }

        /// <summary>
        /// End time of process executionm
        /// </summary>
        private int endTime;
        /// <summary>
        /// End time of process execution
        /// </summary>
        public int EndTime
        {
            get { return endTime; }
            set
            {
                endTime = value;
                NotifyPropertyChanged(m => m.EndTime);
                NotifyPropertyChanged(m => m.CPUTimeUsed);

            }
        }

        /// <summary>
        /// Wait time of process execution
        /// </summary>
        private int waitTime;
        /// <summary>
        /// Wait time of process execution
        /// </summary>
        public int WaitTime
        {
            get { return waitTime; }
            set
            {
                waitTime = value;
                NotifyPropertyChanged(m => m.WaitTime);

            }
        }

        /// <summary>
        /// Time the memory was allocated
        /// </summary>
        private int timeStarted;
        /// <summary>
        /// Time the memory was allocated
        /// </summary>
        public int TimeStarted
        {
            get { return timeStarted; }
            set
            {
                timeStarted = value;
                NotifyPropertyChanged(m => m.TimeStarted);

            }
        }

        private bool hasMemoryAllocated;
        public bool HasMemoryAllocated
        {
            get { return hasMemoryAllocated; }
            set
            {
                hasMemoryAllocated = value;
                NotifyPropertyChanged(m => m.HasMemoryAllocated);
            }
        }

        private bool isMemoryAllocationFinished;
        public bool IsMemoryAllocationFinished
        {
            get { return isMemoryAllocationFinished; }
            set
            {
                isMemoryAllocationFinished = value;
                NotifyPropertyChanged(m => m.IsMemoryAllocationFinished);
            }
        }

        /// <summary>
        /// List of Files used by the process
        /// </summary>
        private ObservableCollection<string> openFiles;
        /// <summary>
        /// List of Files used by the process
        /// </summary>
        public ObservableCollection<string> OpenFiles
        {
            get { return openFiles; }
            set
            {
                openFiles = value;
                NotifyPropertyChanged(m => m.OpenFiles);
            }
        }

        /// <summary>
        /// List of I/O devices used by the process
        /// </summary>
        private ObservableCollection<string> openDevices;
        /// <summary>
        /// List of I/O devices used by the process
        /// </summary>
        public ObservableCollection<string> OpenDevices
        {
            get { return openDevices; }
            set
            {
                openDevices = value;
                NotifyPropertyChanged(m => m.OpenDevices);
            }
        }


        /// <summary>
        /// Generates a display string to print information about the queue.
        /// </summary>
        public string DisplayString
        {

            get
            {
                var registerString = "";
                for (int i = 0; i < Registers.Count; i++)
                {
                    registerString += string.Format("R{0}: {1}\r\n", i + 1, Registers[i]);
                }


                return string.Format(@"ProcessID: {0}
Status: {1}
Program Counter: {2}
Open Files: {3}
Open Devices: {4}
Memory Allocated: {5}

Registers:
{6}", ProcessId, Status.ToString(), ProgramCounter, string.Join(", ", OpenFiles), string.Join(", ", OpenDevices), TotalMemoryAllocated, registerString);

            }
        }


        // Create a random number generator
        private static Random rand = new Random();

        /// <summary>
        /// Constructor for a PCB class.
        /// </summary>
        /// <param name="_processId"></param>
        public PCB(int _processId)
        {
            ProcessId = _processId;
            Registers = new ObservableCollection<int>();
            OpenFiles = new ObservableCollection<string>();
            OpenDevices = new ObservableCollection<string>();

            // Fill 16 registers with random values
            for (int i = 0; i < 16; i++)
            {
                Registers.Add(rand.Next());
            }

            // Assign a random Word-Level value for the number (Multiple of 4)
            ProgramCounter = (rand.Next(10000) / 4) * 4;
        }

    }
}
