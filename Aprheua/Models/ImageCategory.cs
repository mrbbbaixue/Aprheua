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

        public ImageCategory(string folderPath, string name)
        {
            Init(name);
            if (!System.IO.Directory.Exists(folderPath))
            {
                System.IO.Directory.CreateDirectory(folderPath);
            }
            FolderPath = folderPath;
        }

        public ImageCategory(string name) => Init(name);
        public void Init(string name)
        {
            Name = name;
            //ToDo : 扫描并添加
        }
    }
}
