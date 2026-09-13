using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace PcHealthHub.Model
{
    public class ProcessItem : INotifyPropertyChanged
    {
        private string _name = string.Empty;
        private double _cpuUsage;
        private double _memoryUsage;

        public int ProcessId { get; set; }
        public TimeSpan PreviousProcessorTime { get; set; }
        public DateTime PreviousSampleTime { get; set; }

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }

        public double CpuUsage
        {
            get => _cpuUsage;
            set
            {
                _cpuUsage = value;
                OnPropertyChanged();
            }
        }

        public double MemoryUsage
        {
            get => _memoryUsage;
            set
            {
                _memoryUsage = value;
                OnPropertyChanged();
            }
        }

        public ProcessItem(Process proc)
        {
            ProcessId = proc.Id;
            Name = proc.ProcessName;

            MemoryUsage =
                Math.Round(proc.WorkingSet64 / (1024.0 * 1024.0), 1);

            PreviousProcessorTime =
                proc.TotalProcessorTime;

            PreviousSampleTime =
                DateTime.Now;

            CpuUsage = 0;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(
            [CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(
                this,
                new PropertyChangedEventArgs(propertyName));
        }
    }
}
