using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Management;
using System.Runtime.CompilerServices;

namespace PcHealthHub.Model
{
    public class ComputerInfo : INotifyPropertyChanged
    {
        public string MachineName { get; set; } = string.Empty;
        public string OsVersion { get; set; } = string.Empty;
        public string Processors { get; set; } = string.Empty;

        public string DriveName { get; set; } = string.Empty;
        public double TotalSpace { get; set; }
        public double FreeSpace { get; set; }
        public double PercentUsed { get; set; }

        public int Directories { get; set; }
        public int Files { get; set; }

        public ComputerInfo()
        {
            MachineName = Environment.MachineName;
            OsVersion = Environment.OSVersion.ToString();

            LoadDriveInfo(@"C:\");

            if (!OperatingSystem.IsWindows())
            {
                throw new PlatformNotSupportedException("OS system must be Windows");
            }
            try
            {
                string query = "SELECT NumberOfProcessors FROM Win32_ComputerSystem";

                using ManagementObjectSearcher searcher = new(query);
                foreach (ManagementBaseObject item in searcher.Get())
                {
                    Processors = Convert.ToInt32(item["NumberOfProcessors"]).ToString();
                }
            }
            catch (Exception)
            {
                throw new Exception("Error retrieving CPU information");
            }
        }

        private void LoadDriveInfo(string drivePath)
        {
            DriveInfo drive = new(drivePath);

            if (!drive.IsReady)
                return;

            DriveName = drive.Name;

            TotalSpace =
                drive.TotalSize / (1024.0 * 1024.0 * 1024.0);

            FreeSpace =
                Math.Round(drive.AvailableFreeSpace / (1024.0 * 1024.0 * 1024.0), 1);

            double usedSpace =
                drive.TotalSize - drive.AvailableFreeSpace;

            PercentUsed =
                usedSpace / drive.TotalSize * 100.0;

            DirectoryInfo directory =
                new(drive.RootDirectory.FullName);

            try
            {
                Directories = directory.GetDirectories().Length;
                Files = directory.GetFiles().Length;
            }
            catch
            {
                Directories = 0;
                Files = 0;
            }
        }

        //Helpers
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
