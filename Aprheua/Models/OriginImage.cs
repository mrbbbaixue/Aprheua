/****************************************************************
                   Copyright © 2021 Aprheua

    File Name:     Models/OriginImage.cs

    Author:        Chenhao Wang (MrBBBaiXue@github.com)

    Version:       2.3.3.3

    Date:          2021-03-11

    Description:   待分类图像 OriginImage类

    Classes:       OriginImage : NotificationObject
                   // NotificationObject的继承，包含ImageCategories；
                      处理传入的图像数据，图像类型分类和图片块。同时与前台
                      进行间接的绑定。

****************************************************************/

using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Aprheua.ViewModels;

namespace Aprheua.Models
{
    public class OriginImage : NotificationObject
    {
        public string Path { get; set; }
        public string Name { get; set; }

        private string _thumbImagePath;
        public string ThumbImagePath
        {
            get { return _thumbImagePath; }
            set
            {
                _thumbImagePath = value;
                RaisePropertyChanged("ThumbImagePath");
            }
        }

        private string _overlayImagePath;
        public string OverlayImagePath
        {
            get { return _overlayImagePath; }
            set
            {
                _overlayImagePath = value;
                RaisePropertyChanged("OverlayImagePath");
            }
        }

        private int _numberOfImageBlocks;
        public int NumberOfImageBlocks
        {
            get { return _numberOfImageBlocks; }
            set
            {
                _numberOfImageBlocks = value;
                RaisePropertyChanged("NumberOfBlocks");
            }
        }

        private bool _isSelected;
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value;
                RaisePropertyChanged("IsSelected");
            }
        }
        public Commands.DelegateCommand CheckBoxClickEvent { get; set; }
        public Commands.DelegateCommand RemoveImageClickEvent { get; set; }
        public Commands.DelegateCommand OpenInExplorerClickEvent { get; set; }
        public void OpenInExplorerClick(object parameter)
            => System.Diagnostics.Process.Start("explorer", Path);
        public ObservableCollection<ImageCategory> ImageCategories { get; set; }

        public OriginImage(string path, Commands.DelegateCommand checkBoxClickEvent, Commands.DelegateCommand removeImageClickEvent)
        {
            Path = System.IO.Path.GetFullPath(path);
            Name = System.IO.Path.GetFileName(path);
            ThumbImagePath = System.IO.Path.Combine(App.AprheuaThumbImagesFolder, $"thumb-{Utility.GetTimeStamp()}-{Name}");
            OverlayImagePath = System.IO.Path.Combine(App.AprheuaOverlayImagesFolder, $"overlay-{Utility.GetTimeStamp()}-{Name}");
            NumberOfImageBlocks = 0;
            IsSelected = false;
            ImageCategories = new ObservableCollection<ImageCategory>();
            GetReducedImageAsync();
            CheckBoxClickEvent = checkBoxClickEvent;
            RemoveImageClickEvent = removeImageClickEvent;
            OpenInExplorerClickEvent = new Commands.DelegateCommand(new Action<object>(OpenInExplorerClick));
        }
        public int AddCategory(string folderPath, string name)
        {
            Commands.DelegateCommand removeCategoryClickEvent = new Commands.DelegateCommand(new Action<object>((_) =>
            {
                foreach(var imagecategory in ImageCategories)
                {
                    if (imagecategory.Name == name)
                    {
                        ImageCategories.Remove(imagecategory);
                        Utility.DeleteFolder(imagecategory.FolderPath);
                        break;
                    }
                }
            }));
            var category = new ImageCategory(folderPath, name, removeCategoryClickEvent);
            category.ScanImages();
            ImageCategories.Add(category);
            return ImageCategories.IndexOf(category);
        }
        private async void GetReducedImageAsync()
        {
            Task GetReducedImageTask()
            {
                return Task.Run(() =>
                {
                    var thumbImage = new ThumbImage(Path);
                    thumbImage.GetReducedImage(50, ThumbImagePath);
                    RaisePropertyChanged("ThumbImagePath");
                });
            }
            await GetReducedImageTask().ConfigureAwait(false);
        }
    }
}
