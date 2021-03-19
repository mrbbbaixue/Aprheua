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
    /// <summary>
    /// HAAR分类器处理类
    /// </summary>
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

        /// <summary>
        /// 类的构造函数
        /// </summary>
        /// <param name="path">HAAR分类器数据集的路径</param>
        public HaarClassifier(string path)
        {
            Path = path;
            Name = System.IO.Path.GetFileNameWithoutExtension(path);
            IsSelected = true;
            App.Log.OpenCV($"Classifier {Name} @ {Path} added.");
        }

        /// <summary>
        /// 对图片进行分类并保存的方法，返回一个列表，其中储存分类产生的所有Rect对象。
        /// </summary>
        /// <param name="imagePath">图像文件的路径</param>
        /// <param name="outputFolderPath">输出分类图像块的文件夹</param>
        /// <param name="nDetection">重复检测次数</param>
        /// <param name="minSize">最小矩形大小</param>
        /// <param name="maxSize">最大矩形大小</param>
        /// <returns>List<Rect>对象，图片块的内容</returns>
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
            System.GC.Collect();
            return OutputImageBlockRects;
        }

        /// <summary>
        /// 在图像中勾出所有分类产生的图像块。
        /// </summary>
        /// <param name="rects">要在图像中勾出的矩形列表</param>
        /// <param name="imagePath">图像文件的路径</param>
        /// <param name="outputPath">保存勾出矩形块之后的图像路径</param>
        /// <returns>无</returns>
        public static void WriteRectsToImage(List<Rect> rects, string imagePath ,string outputPath)
        {
            Mat srcImage = new Mat(imagePath, ImreadModes.AnyColor);
            var mostUsedRGB = ImageAnalysis.GetMostUsedColor(imagePath);
            // 获得图片中最多使用的RGB颜色，使得框框更加清晰
            var reversalRGB = System.Drawing.Color.FromArgb(255 - mostUsedRGB.R, 255 - mostUsedRGB.G, 255 - mostUsedRGB.B);
            // RGB 反色算法 255 - x
            App.Log.OpenCV($"mostUsedRGB : R:{mostUsedRGB.R} G:{mostUsedRGB.G} B:{mostUsedRGB.B}");
            App.Log.OpenCV($"reversalRGB : R:{reversalRGB.R} G:{reversalRGB.G} B:{reversalRGB.B}");
            foreach (var rect in rects)
            {
                Cv2.Rectangle(srcImage, rect, new Scalar(reversalRGB.B, reversalRGB.G, reversalRGB.R));
            }
            srcImage.ImWrite(outputPath);
            srcImage.Dispose();
            System.GC.Collect();
        }
    }
}
