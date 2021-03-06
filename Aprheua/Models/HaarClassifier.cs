using System.Collections.Generic;
using OpenCvSharp;
using Aprheua.ViewModels;

namespace Aprheua.Models
{
    /*
    public class HaarClassifier2 : NotificationObject
    {
        private string HaarClassifierPath { get; }
        private string ImageInputPath { get; }
        private string ImageBlockOutputPath { get; }
        public HaarClassifier2(string haarClassifierPath, string imageInputPath, string imageBlockOutputPath)
        {
            HaarClassifierPath = haarClassifierPath;
            ImageInputPath = imageInputPath;
            ImageBlockOutputPath = imageBlockOutputPath;
        }

        public List<string> StartHaarClassifier(HaarClassifierChecking haarClassifierChecking, int nDetection, int minSize, int maxSize)
        {
            int count = 1;
            List<string> OutputImageBlockPath = new List<string>();
            Mat srcImage = new Mat(ImageInputPath, ImreadModes.AnyColor);
            CascadeClassifier haarClassifier = new CascadeClassifier(HaarClassifierPath + $@"\{haarClassifierChecking}.xml");
            Rect[] imageBlockRect = haarClassifier.DetectMultiScale(srcImage, 1.1, nDetection, 0, new Size(minSize, minSize), new Size(maxSize, maxSize));
            foreach (var item in imageBlockRect)
            {
                Mat imageBlock = new Mat(srcImage, item);
                imageBlock.ImWrite(ImageBlockOutputPath + $@"\{haarClassifierChecking}-{count}.jpg");
                OutputImageBlockPath.Add(ImageBlockOutputPath + $@"\{haarClassifierChecking}-{count}.jpg");
                count++;
            }
            return OutputImageBlockPath;
        }
    }
    */

    public class HaarClassifier : NotificationObject
    {
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

        private string _path;
        public string Path
        {
            get { return _path; }
            set
            {
                _path = value;
                RaisePropertyChanged("Path");
            }
        }

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

        public HaarClassifier(string path)
        {
            Path = path;
            Name = System.IO.Path.GetFileNameWithoutExtension(path);
            IsSelected = true;
        }
    }
}
