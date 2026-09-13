using PcHealthHub.Utility;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;

namespace PcHealthHub.Model
{
    public class FileItem : INotifyPropertyChanged
    {
        private string _name = string.Empty;
        private string _size = string.Empty;
        private string _dateModified = string.Empty;

        public string Name
        {
            get { return _name; }
            set { _name = value; OnPropertyChanged(); }
        }
        public string Size
        {
            get { return _size; }
            set { _size = value; OnPropertyChanged(); }
        }
        public string DateModified
        {
            get { return  _dateModified; }
            set { _dateModified = value; OnPropertyChanged(); }
        }
        public FileItem(FileInfo file)
        {
            Name = file.Name;
            Size = FriendlyFileSize.FormatBytes(file.Length);
            DateModified = file.LastWriteTime.ToString();
        }

        //Helpers
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
