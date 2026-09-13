using PcHealthHub.Model;
using PcHealthHub.Service;
using System.Collections.ObjectModel;
using System.Windows.Threading;
using System.Windows;
using System.Linq;

namespace PcHealthHub
{
    public partial class MainWindow : Window
    {
        public ComputerInfo ComputerInfo { get; }

        public ObservableCollection<FileItem> Files { get; }

        public ObservableCollection<ProcessItem> Processes { get; } = [];

        private readonly DispatcherTimer _processTimer;

        public MainWindow()
        {
            InitializeComponent();

            ComputerInfo = new ComputerInfo();
            Files = FileService.GetFiles(@"C:\");

            DataContext = this;

            _processTimer = new DispatcherTimer();
            _processTimer.Interval = TimeSpan.FromSeconds(1);
            _processTimer.Tick += ProcessTimer_Tick;

            RefreshProcesses();

            _processTimer.Start();
        }

        private void ProcessTimer_Tick(object? sender, EventArgs e)
        {
            RefreshProcesses();
        }

        private void RefreshProcesses()
        {
            var latestProcesses = ProcessService.GetTopProcesses(10);

            foreach (ProcessItem latest in latestProcesses)
            {
                ProcessItem? existing =
                    Processes.FirstOrDefault(p => p.ProcessId == latest.ProcessId);

                if (existing != null)
                {
                    existing.MemoryUsage = latest.MemoryUsage;

                    ProcessService.UpdateCpuUsage(existing);
                }
                else
                {
                    Processes.Add(latest);
                }
            }

            var currentIds =
                latestProcesses
                    .Select(p => p.ProcessId)
                    .ToHashSet();

            for (int i = Processes.Count - 1; i >= 0; i--)
            {
                if (!currentIds.Contains(Processes[i].ProcessId))
                {
                    Processes.RemoveAt(i);
                }
            }
        }
    }
}