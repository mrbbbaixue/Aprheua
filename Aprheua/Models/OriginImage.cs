using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using OpenCvSharp;

namespace Aprheua.Models
{
    public class OriginImage
    {
        public string Path { get; set; }
        public string ThumbImagePath => System.IO.Path.Combine(App.AprheuaThumbImagesFolder, $"thumb-{Name}.jpg");
        public string OverlayImagePath => System.IO.Path.Combine(App.AprheuaOverlayImagesFolder, $"overlay-{Name}.jpg");
        public string Name { get; set; }
        public int NumberOfBlocks { get; set; }
        public bool IsSelected { get; set; }

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
