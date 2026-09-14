using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Management;
using System.Runtime.CompilerServices;

namespace PcHealthHub.Model
{
    public class ComputerInfo
    {
        public string MachineName { get; set; } = string.Empty;
        public string OsVersion { get; set; } = string.Empty;
        public string Processors { get; set; } = string.Empty;

        public ComputerInfo()
        {
            MachineName = Environment.MachineName;
            OsVersion = Environment.OSVersion.ToString();

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
    }
}
