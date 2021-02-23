using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using Aprheua.ViewModels;
using OpenCvSharp;

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
                this.RaisePropertyChanged("ThumbImagePath");
            }
        }

        private string _overlayImagePath;
        public string OverlayImagePath
        {
            get { return _overlayImagePath; }
            set
            {
                _overlayImagePath = value;
                this.RaisePropertyChanged("OverlayImagePath");
            }
        }

        private int _numberOfImageBlocks;
        public int NumberOfImageBlocks
        {
            get { return _numberOfImageBlocks; }
            set
            {
                _numberOfImageBlocks = value;
                this.RaisePropertyChanged("NumberOfBlocks");
            }
        }

        private bool _isSelected;
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value;
                this.RaisePropertyChanged("IsSelected");
            }
        }
        public Commands.DelegateCommand CheckBoxClickEvent { get; set; }
        public ObservableCollection<ImageCategory> ImageCategories { get; set; }

        public OriginImage(string path, Commands.DelegateCommand checkBoxClickEvent)
        {
            Init(path);
            CheckBoxClickEvent = checkBoxClickEvent;
        }
        public OriginImage(string path) => Init(path);
        public void Init(string path)
        {
            Path = System.IO.Path.GetFullPath(path);
            Name = System.IO.Path.GetFileName(path);
            ThumbImagePath = System.IO.Path.Combine(App.AprheuaThumbImagesFolder, $"thumb-{Utility.GetTimeStamp()}-{Name}.jpg");
            OverlayImagePath = System.IO.Path.Combine(App.AprheuaOverlayImagesFolder, $"overlay-{Utility.GetTimeStamp()}-{Name}.jpg");
            NumberOfImageBlocks = 0;
            IsSelected = false;
            ImageCategories = new ObservableCollection<ImageCategory> { };
            //ToDo : 使用Async异步执行
            var thumbImage = new ThumbImage(Path);
            thumbImage.GetReducedImage(0.15, ThumbImagePath);
        }
    }
}
