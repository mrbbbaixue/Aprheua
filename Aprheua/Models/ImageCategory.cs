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
        public ObservableCollection<ImageBlock> ImageBlocks { get; set; }
        public Commands.DelegateCommand RemoveCategoryClickEvent { get; set; }
        public Commands.DelegateCommand AddBlockClickEvent { get; set; }

        public ImageCategory(string folderPath, string name, Commands.DelegateCommand removeCategoryClickEvent)
        {
            ImageBlocks = new ObservableCollection<ImageBlock> { };
            Name = name;
            if (!System.IO.Directory.Exists(folderPath))
            {
                System.IO.Directory.CreateDirectory(folderPath);
            }
            FolderPath = folderPath;
            RemoveCategoryClickEvent = removeCategoryClickEvent;
            AddBlockClickEvent = new Commands.DelegateCommand(new Action<object>(AddBlock));
        }
        public void AddBlock(object parameter)
        {
            //ToDo : AddImageBlock
            App.Log.Info($"AddBlock for category : {Name} has triggered!");
        }
    }
}
