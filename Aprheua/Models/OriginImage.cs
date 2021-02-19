using System;
using System.Collections.Generic;
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

        public readonly string Path;
        public string ThumbImagePath => System.IO.Path.Combine(App.AprheuaThumbImagesFolder, $"thumb-{Name}.jpg");
        public string OverlayImagePath => System.IO.Path.Combine(App.AprheuaOverlayImagesFolder, $"overlay-{Name}.jpg");
        public string Name { get; set; }

        private int _numberOfBlocks;
        public int NumberOfBlocks
        {
            get { return _numberOfBlocks; }
            set
            {
                _numberOfBlocks = value;
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
        public OriginImage(string path)
        {
            Path = System.IO.Path.GetFullPath(path);
            Name = System.IO.Path.GetFileName(path);
            NumberOfBlocks = 0;
            IsSelected = false;
            //ToDo : 使用Async异步执行
            var thumbImage = new ThumbImage(Path);
            thumbImage.GetReducedImage(0.15, ThumbImagePath);
        }
    }
}
