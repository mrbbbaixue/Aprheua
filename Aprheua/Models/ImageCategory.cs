using System;
using System.Collections.ObjectModel;
using Aprheua.ViewModels;

namespace Aprheua.Models
{
    public class ImageCategory : NotificationObject
    {
        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                RaisePropertyChanged("Name");
            }
        }

        public readonly string FolderPath;
        public ObservableCollection<ImageBlock> ImageBlocks { get; set; }
        public Commands.DelegateCommand RemoveCategoryClickEvent { get; set; }
        public Commands.DelegateCommand AddBlockClickEvent { get; set; }

        public ImageCategory(string folderPath, string name, Commands.DelegateCommand removeCategoryClickEvent)
        {
            ImageBlocks = new ObservableCollection<ImageBlock>();
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
            //ToDo : AddImageBlockFromScreenShot
            App.Log.Info($"AddBlock for Category : {Name} has been triggered!");
        }
        public void ScanImages()
        {
            App.Log.Info($"ScanImages in Category : {Name} has been triggered!");
            var files = System.IO.Directory
                                 .EnumerateFiles(FolderPath,"*.jpg");
            foreach(var image in files)
            {
                if (!CheckIfAlreadyExists(image))
                {
                    AddBlockFromFile(image);
                }
            }
        }
        private bool CheckIfAlreadyExists(string image)
        {
            if(ImageBlocks.Count > 0)
            {
                foreach (var imageBlock in ImageBlocks)
                {
                    if (System.IO.Path.GetFullPath(imageBlock.BlockPath).Equals(System.IO.Path.GetFullPath(image)))
                        return true;
                }
            }
            return false;
        }
        private void AddBlockFromFile(string path)
        {
            Commands.DelegateCommand removeRemoveBlockClickEvent = new Commands.DelegateCommand(new Action<object>((_) =>
            {
                foreach (var imageblock in ImageBlocks)
                {
                    if (imageblock.BlockPath == path)
                    {
                        System.IO.File.Delete(imageblock.BlockPath);
                        ImageBlocks.Remove(imageblock);
                        break;
                    }
                }
            }));
            var imageBlock = new Models.ImageBlock(path, removeRemoveBlockClickEvent);
            ImageBlocks.Add(imageBlock);
            App.Log.Info($"ImageBlock {path} has been added to {Name}");
        }
    }
}
