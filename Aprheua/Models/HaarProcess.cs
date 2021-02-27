using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprheua.Models
{
    class Start
    {
        public List<string> StartClassifier()
        {
            HaarProcess haarProcess = new HaarProcess(@"", @"", @"", 3, 30, 50);//这个应该是传入的，我先这样写着
            Task task1 = new Task(new Action(haarProcess.StartEyes));
            Task task2 = new Task(new Action(haarProcess.StartEars));
            Task task3 = new Task(new Action(haarProcess.StartNose));
            Task task4 = new Task(new Action(haarProcess.StartMouth));
            task1.Start();
            task2.Start();
            task3.Start();
            task4.Start();
            return haarProcess.ImageBlockPath;
        }
    }
    class HaarProcess
    {
        public List<string> ImageBlockPath { get; set; }
        private string _haarClassifierPath { get; set; }
        private string _imageInputPath { get; set; }
        private string _imageBlockOutputPath { get; set; }
        public bool isStartEyes { get; set; }
        public bool isStartEars { get; set; }
        public bool isStartNose { get; set; }
        public bool isStartMouth { get; set; }
        public int _nDetection { get; set; }
        public int _minSize { get; set; }
        public int _maxSize { get; set; }

        HaarClassifier haarClassifier = new HaarClassifier(@"", @"", @"");
        public HaarProcess(string haarClassifierPath, string imageInputPath, string imageBlockOutputPath, int nDetection, int minSize, int maxSize)
        {
            _nDetection = nDetection;
            _minSize = minSize;
            _maxSize = maxSize;
            _haarClassifierPath = haarClassifierPath;
            _imageInputPath = imageInputPath;
            _imageBlockOutputPath = imageBlockOutputPath;
            isStartEyes = false;
            isStartEars = false;
            isStartNose = false;
            isStartMouth = false;
        }
        public void StartEyes()
        {
            List<string> list = haarClassifier.StartHaarClassifier(HaarClassifierChecking.eyesHaarClassifier, _nDetection, _minSize, _maxSize);
            foreach (var item in list)
            {
                ImageBlockPath.Add(item);
            }
            isStartEyes = true;
        }

        public void StartEars()
        {
            List<string> list = haarClassifier.StartHaarClassifier(HaarClassifierChecking.earsHaarClassifier, _nDetection, _minSize, _maxSize);
            foreach (var item in list)
            {
                ImageBlockPath.Add(item);
            }
            isStartEars = true;
        }

        public void StartNose()
        {
            List<string> list = haarClassifier.StartHaarClassifier(HaarClassifierChecking.noseHaarClassifier, _nDetection, _minSize, _maxSize);
            foreach (var item in list)
            {
                ImageBlockPath.Add(item);
            }
            isStartNose = true;
        }
        public void StartMouth()
        {
            List<string> list = haarClassifier.StartHaarClassifier(HaarClassifierChecking.mouthHaarClassifier, _nDetection, _minSize, _maxSize);
            foreach (var item in list)
            {
                ImageBlockPath.Add(item);
            }
            isStartMouth = true;
        }

    }
}
