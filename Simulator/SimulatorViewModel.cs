using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleMvvmToolkit;
using System.Threading;
using System.Collections.ObjectModel;
using System.Windows.Threading;
using System.Windows;
using System.IO;

namespace Simulator
{

    /// <summary>
    /// View Model class that acts as the Controller between the Model and the View
    /// </summary>
    public class SimulatorViewModel : ViewModelDetailBase<SimulatorViewModel, SimulatorModel>
    {

        /// <summary>
        /// Constructor
        /// </summary>
        public SimulatorViewModel()
        {
            Model = new SimulatorModel();
        }

        /// <summary>
        /// Command to bind to the "Simulate Processes" button
        /// </summary>
        private DelegateCommand simulateProcessesCommand;
        public DelegateCommand SimulateProcessesCommand
        {
            get { return simulateProcessesCommand ?? (simulateProcessesCommand = new DelegateCommand(SimulateProcesses)); }
            private set { simulateProcessesCommand = value; }
        }

        /// <summary>
        /// Command to bind to the "Add New Process" button
        /// </summary>
        private DelegateCommand addNewProcessCommand;
        public DelegateCommand AddNewProcessCommand
        {
            get { return addNewProcessCommand ?? (addNewProcessCommand = new DelegateCommand(AddNewProcess)); }
            private set { addNewProcessCommand = value; }
        }

        /// <summary>
        /// Command to bind to the "Execute Next Process" button
        /// </summary>
        private DelegateCommand executeNextProcessCommand;
        public DelegateCommand ExecuteNextProcessCommand
        {
            get { return executeNextProcessCommand ?? (executeNextProcessCommand = new DelegateCommand(MoveToNextProcess)); }
            private set { executeNextProcessCommand = value; }
        }

        /// <summary>
        /// Command to bind to the "Set to Ready" button
        /// </summary>
        private DelegateCommand setToReadyCommand;
        public DelegateCommand SetToReadyCommand
        {
            get { return setToReadyCommand ?? (setToReadyCommand = new DelegateCommand(SetToReady)); }
            private set { setToReadyCommand = value; }
        }

        private DelegateCommand openFileCommand;
        public DelegateCommand OpenFileCommand
        {
            get { return openFileCommand ?? (openFileCommand = new DelegateCommand(OpenFile)); }
            private set { openFileCommand = value; }
        }

        /// <summary>
        /// Random number generator
        /// </summary>
        private static Random rand = new Random();

        /// <summary>
        /// Moves the first process in the waiting queue to the ready queue
        /// </summary>
        public void SetToReady()
        {
            // Check that the waiting queue is not empty
            if (Model.WaitingQueue.Count > 0)
            {

                // Remove from the waiting queue and move to the ready queue
                var process = Model.WaitingQueue.First();
                SetToReady(process);
            }
        }

        public async Task SetToReady(PCB process, int delay = 0)
        {
            await Task.Delay(delay);

            RemoveProcess(process.ProcessId);
            AddToReady(process, 0);
        }

        /// <summary>
        /// Add a new process to the queue depending on the queue the user selected.
        /// The process is created with a random process ID
        /// </summary>
        public void AddNewProcess()
        {
            // Generate a process with a random id
            var process = new PCB(rand.Next(1000) + 20);
            // Assign a random memory size
            process.TotalMemoryAllocated = rand.Next(Model.MemorySize / 3) + 1;
            process.TimeNeededForMemory = rand.Next(3) * 1000;

            // Assign the status according to the GUI
            process.Status = Model.SelectedProcessType;

            // Add the process to the corresponding queue.
            AddProcess(process);

        }

        public async void ClearAllocatedMemory(int delay, int processId)
        {
            await Task.Delay(delay);

            App.Current.Dispatcher.Invoke((Action)delegate
            {
                MoveToNextProcess(processId);
                Model.Memory.ClearMemory(processId);
            });

        }

        /// <summary>
        /// Simulates adding 10 processes to the waiting queue
        /// Waits for them to become ready
        /// Executes each process from the Ready queue
        /// </summary>
        public async void SimulateProcesses()
        {
            Model.IsEnabled = false;

            // Simulate adding to the waiting queue
            for (int i = 0; i < 10; i++)
            {
                var process = new PCB(i);
                // Assign a random memory size
                process.TotalMemoryAllocated = rand.Next(Model.MemorySize / 3) + 1;
                process.TimeNeededForMemory = rand.Next(3) * 1000;

                await AddToWaiting(process, Model.SimulationDelay);

            }

            // Simulate moving to the ready queue
            for (int i = 0; i < 10; i++)
            {
                var process = RemoveProcess(i);
                await AddToReady(process, Model.SimulationDelay);
            }


            // Simulate executing each process
            for (int i = 0; i <= 10; i++)
            {
                await ExecuteNextProcess(Model.SimulationDelay);
            }

            Model.IsEnabled = true;
        }

        /// <summary>
        /// Adds a process to the waiting queue
        /// </summary>
        /// <param name="process"></param>
        /// <param name="delay"></param>
        /// <returns></returns>
        public async Task AddToWaiting(PCB process, int delay = 1000)
        {
            await Task.Delay(delay);

            // Adds the process to the waiting queue
            process.Status = ProcessStatus.WAITING;
            AddProcess(process);
        }

        /// <summary>
        /// Adds a process to the ready queue
        /// </summary>
        /// <param name="process"></param>
        /// <param name="delay"></param>
        /// <returns></returns>
        public async Task AddToReady(PCB process, int delay = 1000)
        {
            await Task.Delay(delay);

            // Adds the process to the ready queue
            process.Status = ProcessStatus.READY;
            AddProcess(process);
        }

        /// <summary>
        /// Executes the next process in the ready queue
        /// </summary>
        /// <param name="delay"></param>
        /// <returns></returns>
        public async Task ExecuteNextProcess(int delay = 1000)
        {
            //Executes the next process
            await Task.Delay(delay);
            MoveToNextProcess();
        }


        /// <summary>
        /// adding a PCB to a given position in the queue, the default position is the end (tail) of the queue.
        /// </summary>
        /// <param name="process"></param>
        public void AddProcess(PCB process, int position = -1)
        {
            // set the desired queue according to the process status
            var queue = process.Status == ProcessStatus.READY ? Model.ReadyQueue : Model.WaitingQueue;

            if (position == -1)
            {
                // Insert at the tail of the queue
                queue.Insert(queue.Count, process);
                Console.WriteLine("Adding process({0}) to {1} queue at tail position", process.ProcessId, process.Status.ToString());
            }
            else if (position < queue.Count)
            {
                // Insert at a specified position
                queue.Insert(position, process);
                Console.WriteLine("Adding process({0}) to {1} queue at position {2}", process.ProcessId, process.Status.ToString(), position);
            }

            if (process.Status == ProcessStatus.READY)
            {
                // Allocate the memory
                var allocationResult = Model.Memory.TryAllocate(process.ProcessId, process.TotalMemoryAllocated);

                if (!allocationResult)
                {
                    // Fragmentation Error
                    MessageBox.Show("Fragmentation error using: " + Model.SelectedMemoryType.ToString());
                }
                Console.WriteLine(allocationResult);

                Task.Run(() => ClearAllocatedMemory(process.TimeNeededForMemory, process.ProcessId));

            }
        }

        /// <summary>
        /// removing a PCB with a given PID. the default is the beginning (head) of the queue.
        /// </summary>
        /// <param name="pid"></param>
        /// <returns>The removed process</returns>
        public PCB RemoveProcess(int pid)
        {

            // Check the ready queue for the process
            var readyProcess = Model.ReadyQueue.FirstOrDefault(p => p.ProcessId == pid);
            if (readyProcess != null)
            {
                Console.WriteLine("Removing process({0}) from READY queue", pid);

                Model.ReadyQueue.Remove(readyProcess);
                return readyProcess;
            }

            // Check the waiting queue for the process
            var waitingProcess = Model.WaitingQueue.FirstOrDefault(p => p.ProcessId == pid);
            if (waitingProcess != null)
            {
                Console.WriteLine("Removing process({0}) from WAITING queue", pid);

                Model.WaitingQueue.Remove(waitingProcess);
                return waitingProcess;
            }

            // Check the current CPU execution process for the specified process
            if (Model.ExecutingPcb != null && Model.ExecutingPcb.ProcessId == pid)
            {

                RemoveProcessFromExecution(Model.ExecutingPcb);

                var old = Model.ExecutingPcb;
                Model.ExecutingPcb = null;
                MoveToNextProcess();
                return old;
            }

            return null;
        }

        /// <summary>
        /// Prints the contents of the ready queue
        /// </summary>
        public void PrintReadyQueue()
        {
            PrintQueue(Model.ReadyQueue);
        }

        /// <summary>
        /// Prints the contents of the waiting queue
        /// </summary>
        public void PrintWaitingQueue()
        {
            PrintQueue(Model.WaitingQueue);
        }

        /// <summary>
        /// Prints the contents of the specified queue
        /// </summary>
        /// <param name="queue"></param>
        public void PrintQueue(ObservableCollection<PCB> queue)
        {
            Console.WriteLine("Queue Contents");
            for (int i = 0; i < queue.Count; i++)
            {
                // Write the contents of each PCB in the queue
                Console.WriteLine("Index: {0}", i);
                Console.WriteLine(queue[i].DisplayString);
            }
        }


        public void RemoveProcessFromExecution(PCB process)
        {
            process.EndTime = Environment.TickCount;


            App.Current.Dispatcher.Invoke((Action)delegate
            {
                Model.Memory.ClearMemory(process.ProcessId);
            });

            Console.WriteLine("Removing process({0}) from Execution", process.ProcessId);
            Console.WriteLine("CPU time used by process({0}): {1}", process.ProcessId, process.CPUTimeUsed);

        }

        /// <summary>
        /// Moves the next ready process to start executing.
        /// </summary>
        private void MoveToNextProcess()
        {
            var pid = Model.ReadyQueue.Count > 0 ? Model.ReadyQueue.First().ProcessId : -1;
            MoveToNextProcess(pid);
        }

        public void MoveToNextProcess(int pid)
        {
            if (Model.ExecutingPcb != null)
            {
                RemoveProcessFromExecution(Model.ExecutingPcb);
            }

            var process = Model.ReadyQueue.FirstOrDefault(p => p.ProcessId == pid);

            // If there is another process in the ready queue
            if (process != null)
            {
                

                // Get the next process from the ready queue
                Model.ExecutingPcb = process;
                Model.ReadyQueue.Remove(process);

                Model.ExecutingPcb.StartTime = Environment.TickCount;

                Console.WriteLine("Removing process({0}) from Ready Queue", Model.ExecutingPcb.ProcessId);

                Console.WriteLine("Adding process({0}) to Execution", Model.ExecutingPcb.ProcessId);

            }
            else
            {
                // Move the process out of execution
                Model.ExecutingPcb = null;
            }
        }

        public void OpenFile()
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.FileName = "Document"; // Default file name 
            dlg.DefaultExt = ".txt"; // Default file extension 
            dlg.Filter = "Text documents (.txt)|*.txt"; // Filter files by extension 

            // Show open file dialog box 
            var result = dlg.ShowDialog();

            // Process open file dialog box results 
            if (result == true)
            {
                // Open document 
                string filename = dlg.FileName;

                LoadProcessesFromFile(filename);
            }
        }

        public async void LoadProcessesFromFile(string filePath)
        {
            using (StreamReader reader = new StreamReader(File.OpenRead(filePath)))
            {

                string memorySizeStr = reader.ReadLine();
                int memorySize = Model.MemorySize;
                if (int.TryParse(memorySizeStr, out memorySize))
                {
                    Model.MemorySize = memorySize;
                    Model.ChangeMemory();
                }

                string line = null;

                while ((line = reader.ReadLine()) != null)
                {
                    if (line.Length > 0)
                    {
                        //PID   WAIT_TIME   EXEC_TIME   MEM_NEEDED
                        var splitted = line.Split('\t');

                        if (splitted.Length > 3)
                        {


                            var segments = splitted.Select(s =>
                            {
                                return s.Length > 0 ? int.Parse(s) : 0;
                            }).ToList();


                            int pid = segments[0];
                            int waitTime = segments[1];
                            int execTime = segments[2];
                            int memNeeded = segments[3];

                            var process = new PCB(pid);
                            process.TotalMemoryAllocated = memNeeded;
                            process.TimeNeededForMemory = execTime * 1000;
                            AddToWaiting(process, 0);
                            SetToReady(process, waitTime * 1000);

                            //break;
                        }
                    }
                }

            }
        }
    }

}