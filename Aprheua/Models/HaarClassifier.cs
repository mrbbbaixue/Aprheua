using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenCvSharp;

namespace Aprheua.Models
{
    public class HaarClassifier
    {
        private string _haarClassifierPath { get; }
        private string _imageInputPath { get; }
        private string _imageBlockOutputPath { get; }
        public HaarClassifier(string haarClassifierPath, string imageInputPath, string imageBlockOutputPath)
        {
            _haarClassifierPath = haarClassifierPath;
            _imageInputPath = imageInputPath;
            _imageBlockOutputPath = imageBlockOutputPath;
        }

        public List<string> StartHaarClassifier(HaarClassifierChecking haarClassifierChecking, int nDetection, int minSize, int maxSize)
        {
            int count = 1;
            List<string> OutputImageBlockPath = new List<string>();
            Mat srcImage = new Mat(_imageInputPath, ImreadModes.AnyColor);
            CascadeClassifier haarClassifier = new CascadeClassifier(_haarClassifierPath + $@"\{haarClassifierChecking}.xml");
            Rect[] imageBlockRect = haarClassifier.DetectMultiScale(srcImage, 1.1, nDetection, 0, new Size(minSize, minSize), new Size(maxSize, maxSize));
            foreach (var item in imageBlockRect)
            {
                Mat imageBlock = new Mat(srcImage, item);
                imageBlock.ImWrite(_imageBlockOutputPath + $@"\{haarClassifierChecking}-{count}.jpg");
                OutputImageBlockPath.Add(_imageBlockOutputPath + $@"\{haarClassifierChecking}-{count}.jpg");
                count++;
            }
            return OutputImageBlockPath;
        }
    }

    public enum HaarClassifierChecking
    {
        eyesHaarClassifier,
        noseHaarClassifier,
        earsHaarClassifier,
        mouthHaarClassifier
    }
}
