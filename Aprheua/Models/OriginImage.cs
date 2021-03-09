using System;
using System.Collections.ObjectModel;
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
            //ToDo : 异步执行
            //ToDo : 生成之后需要Dispose
            var thumbImage = new ThumbImage(Path);
            thumbImage.GetReducedImage(50, ThumbImagePath);
            CheckBoxClickEvent = checkBoxClickEvent;
            RemoveImageClickEvent = removeImageClickEvent;
            OpenInExplorerClickEvent = new Commands.DelegateCommand(new Action<object>(OpenInExplorerClick));
        }
        public void AddCategory(string folderPath, string name)
        {
            Commands.DelegateCommand removeCategoryClickEvent = new Commands.DelegateCommand(new Action<object>((_) =>
            {
                foreach(var imagecategory in ImageCategories)
                {
                    if (imagecategory.Name == name)
                    {
                        Utility.DeleteFolder(imagecategory.FolderPath);
                        ImageCategories.Remove(imagecategory);
                        break;
                    }
                }
            }));
            var category = new ImageCategory(folderPath, name, removeCategoryClickEvent);
            category.ScanImages();
            ImageCategories.Add(category);
        }
    }
}
