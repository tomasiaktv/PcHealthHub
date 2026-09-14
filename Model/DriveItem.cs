using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;

namespace PcHealthHub.Model
{
    public class DriveItem
    {
        public string Name { get; set; } = string.Empty;
        public double TotalSpace { get; set; }
        public double FreeSpace { get; set; }
        public double UsedSpace { get; set; }
        public double PercentUsed { get; set; }

        public int Directories { get; set; }
        public int Files { get; set; }

        public DriveItem(DriveInfo di)
        {
            long usedBytes = di.TotalSize - di.AvailableFreeSpace;

            Name = di.Name;
            TotalSpace = di.TotalSize / (1024.0 * 1024.0 * 1024.0);
            FreeSpace = Math.Round(di.AvailableFreeSpace / (1024.0 * 1024.0 * 1024.0), 1);
            UsedSpace = Math.Round(usedBytes / (1024.0 * 1024.0 * 1024.0), 1);
            PercentUsed = (double)usedBytes / di.TotalSize * 100.0;

            DirectoryInfo directory = new(di.RootDirectory.FullName);
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
    }
}
