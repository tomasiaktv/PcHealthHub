using PcHealthHub.Model;
using PcHealthHub.Service;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace PcHealthHub
{
    public partial class MainWindow : Window
    {
        public ComputerInfo ComputerInfo { get; }

        public ObservableCollection<DriveItem> Drives { get; } = [];

        public ObservableCollection<FileItem> Files { get; } = [];

        public ObservableCollection<ProcessItem> Processes { get; } = [];

        private readonly DispatcherTimer _processTimer;

        public MainWindow()
        {
            InitializeComponent();

            ComputerInfo = new ComputerInfo();
            Drives = DriveService.GetDrives();

            _processTimer = new DispatcherTimer();
            _processTimer.Interval = TimeSpan.FromSeconds(1);
            _processTimer.Tick += ProcessTimer_Tick;

            RefreshProcesses();

            _processTimer.Start();
            DataContext = this;
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

        private void LoadFiles(DriveItem selectedDrive)
        {
            Files.Clear();

            var files = FileService.GetFiles(selectedDrive.Name);

            foreach (var file in files)
            {
                Files.Add(file);
            }
        }

        private void DriveComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ComboBox comboBox &&
                comboBox.SelectedItem is DriveItem selectedDrive)
            {
                LoadFiles(selectedDrive);
            }
        }
    }
}