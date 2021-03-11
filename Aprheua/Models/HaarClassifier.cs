/****************************************************************
                   Copyright © 2021 Aprheua

    File Name:     Models/ImageCategory.cs

    Author:        Chenhao Wang (MrBBBaiXue@github.com)
                   Boyan Wang (JingNianNian@github.com)

    Version:       2.3.3.3

    Date:          2021-03-10

    Description:   OpenCV HAAR分类器 HaarClassifier类

    Classes:       HaarClassifier : NotificationObject
                   // NotificationObject的继承，为分类器模型数据的抽象，
                      封装分类方法

****************************************************************/

using System.Collections.Generic;
using OpenCvSharp;
using Aprheua.ViewModels;

namespace Aprheua.Models
{
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
            App.Log.OpenCV($"Classifier {Name} @ {Path} added.");
        }
        public List<string> StartHaarClassifier(string imagePath, string outputFolderPath, int nDetection, int minSize, int maxSize)
        {
            var count = 0;
            List<string> OutputImageBlockPaths = new List<string>();
            Mat srcImage = new Mat(imagePath, ImreadModes.AnyColor);
            CascadeClassifier haarClassifier = new CascadeClassifier(Path);

            foreach (var item in haarClassifier.DetectMultiScale(srcImage, 1.1, nDetection, 0, new Size(minSize, minSize), new Size(maxSize, maxSize)))
            {
                Mat imageBlock = new Mat(srcImage, item);
                count++;
                var outputImagePath = System.IO.Path.Combine(outputFolderPath, $"{Name}-{count}-{Utility.GetTimeStamp()}.jpg");
                App.Log.OpenCV($"Classifier {Name} Output : {outputImagePath}");
                imageBlock.ImWrite(outputImagePath);
                OutputImageBlockPaths.Add(outputImagePath);
            }
            return OutputImageBlockPaths;
        }
    }
}
