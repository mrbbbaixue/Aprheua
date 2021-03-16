/****************************************************************
                   Copyright © 2021 Aprheua

    File Name:     Models/ImageCategory.cs

    Author:        Chenhao Wang (MrBBBaiXue@github.com)

    Version:       2.3.3.3

    Date:          2021-03-10

    Description:   图像分类 ImageCategory类

    Classes:       ImageCategory : NotificationObject
                   // NotificationObject的继承，包含ImageBlocks；
                      处理传入的图像类别，同时与前台进行间接的绑定。

****************************************************************/

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
            var outputPath = System.IO.Path.Combine(FolderPath, $"Snap-{ImageBlocks.Count}.jpg");
            App.CreateClipWindow(App.MainWindowViewModel.SelectedImage.Path, outputPath);
            App.Log.Info($"AddBlockFromScreenShot for category : {Name} has been triggered!");
            ScanImages();
        }
        public bool ScanImages()
        {
            var hasNewFiles = false;
            App.Log.Info($"ScanImages in Category : {Name} has been triggered!");
            var files = System.IO.Directory
                                 .EnumerateFiles(FolderPath,"*.jpg");
            foreach(var image in files)
            {
                if (!CheckIfAlreadyExists(image))
                {
                    hasNewFiles = true;
                    AddBlockFromFile(image);
                }
            }
            return hasNewFiles;
        }
        private bool CheckIfAlreadyExists(string image)
        {
            if(ImageBlocks.Count > 0)
            {
                foreach (var imageBlock in ImageBlocks)
                {
                    if (System.IO.Path.GetFullPath(imageBlock.BlockPath).Equals(System.IO.Path.GetFullPath(image)))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        private void AddBlockFromFile(string path)
        {
            Commands.DelegateCommand removeRemoveBlockClickEvent = new Commands.DelegateCommand(new Action<object>((_) =>
            {
                foreach (var block in ImageBlocks)
                {
                    if (block.BlockPath == path)
                    {
                        var deleteBlockPath = block.BlockPath;
                        block.BlockPath = "";
                        GC.Collect();
                        //System.IO.File.Delete(deleteBlockPath);
                        ImageBlocks.Remove(block);
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
