using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprheua.Models
{
    public class ImageCategory : ViewModels.NotificationObject
    {
        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                this.RaisePropertyChanged("Name");
            }
        }

        private readonly string FolderPath;

        public ObservableCollection<string> Images;

        public ImageCategory(string folderPath)
        {
            FolderPath = folderPath;
        }

    }
}
