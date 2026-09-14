using PcHealthHub.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;

namespace PcHealthHub.Service
{
    public static class DriveService
    {
        public static ObservableCollection<DriveItem> GetDrives()
        {
            ObservableCollection<DriveItem> drives = [];

            DriveInfo[] allDrives = DriveInfo.GetDrives();

            if (allDrives.Length == 0)
                return drives;

            foreach (DriveInfo di in allDrives)
            {
                if (!di.IsReady)
                    continue;

                drives.Add(new DriveItem(di));
            }
            return drives;
        }
    }
}
