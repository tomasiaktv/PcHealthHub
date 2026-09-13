using PcHealthHub.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;

namespace PcHealthHub.Service
{
    public static class FileService
    {
        public static ObservableCollection<FileItem> GetFiles(string path)
        {
            ObservableCollection<FileItem> items = [];

            DirectoryInfo directory = new(path);

            if (!directory.Exists)
                return items;

            foreach (FileInfo file in directory.GetFiles())
            {
                items.Add(new FileItem(file));
            }
            return items;
        }
    }
}
