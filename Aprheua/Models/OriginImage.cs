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
        public string ThumbImagePath => System.IO.Path.Combine(App.AprheuaTempThumbImages,$"thumb-{Name}.jpg");
        public string Name { get; set; }
        public int NumberOfBlocks { get; set; }
        public void GenerateThumbImage()
        {
            var thumbImage = new ThumbImage(Path);
            thumbImage.GetReducedImage(0.15,ThumbImagePath);
        }
    }
}
