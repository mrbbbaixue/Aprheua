/****************************************************************
                   Copyright © 2021 Aprheua

    File Name:     Models/ImageCategory.cs

    Author:        Chenhao Wang (MrBBBaiXue@github.com)
                   Boyan Wang (JingNianNian@github.com)

    Version:       2.3.3.3

    Date:          2021-03-16

    Description:   OpenCV HAAR分类器 HaarClassifier类

    Classes:       HaarClassifier : NotificationObject
                   // NotificationObject的继承，为分类器模型数据的抽象，
                      封装分类方法

****************************************************************/

using System.Collections.Generic;
using Aprheua.ViewModels;
using OpenCvSharp;

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
        public List<Rect> StartHaarClassifier(string imagePath, string outputFolderPath, int nDetection, int minSize, int maxSize)
        {
            var count = 0;
            List<Rect> OutputImageBlockRects = new List<Rect>();
            Mat srcImage = new Mat(imagePath, ImreadModes.AnyColor);
            CascadeClassifier haarClassifier = new CascadeClassifier(Path);

            foreach (var item in haarClassifier.DetectMultiScale(srcImage, 1.1, nDetection, 0, new Size(minSize, minSize), new Size(maxSize, maxSize)))
            {
                Mat imageBlock = new Mat(srcImage, item);
                count++;
                var outputImagePath = System.IO.Path.Combine(outputFolderPath, $"{Name}-{count}-{Utility.GetTimeStamp()}.jpg");
                imageBlock.ImWrite(outputImagePath);
                //Add to Rects for drawing.
                OutputImageBlockRects.Add(item);
            }
            srcImage.Dispose();
            return OutputImageBlockRects;
        }
        public static void WriteRectsToImage(List<Rect> rects, string imagePath ,string outputPath)
        {
            Mat srcImage = new Mat(imagePath, ImreadModes.AnyColor);
            var mostUsedRGB = ImageAnalysis.GetMostUsedColor(imagePath);
            var reversalRGB = System.Drawing.Color.FromArgb(255 - mostUsedRGB.R, 255 - mostUsedRGB.G, 255 - mostUsedRGB.B);
            App.Log.Info($"mostUsedRGB : R:{mostUsedRGB.R} G:{mostUsedRGB.G} B:{mostUsedRGB.B}");
            App.Log.Info($"reversalRGB : R:{reversalRGB.R} G:{reversalRGB.G} B:{reversalRGB.B}");
            foreach (var rect in rects)
            {
                Cv2.Rectangle(srcImage, rect, new Scalar(reversalRGB.B, reversalRGB.G, reversalRGB.R));
            }
            srcImage.ImWrite(outputPath);
            srcImage.Dispose();
        }

    }
}
