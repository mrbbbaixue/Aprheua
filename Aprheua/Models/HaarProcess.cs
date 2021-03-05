﻿using System;
using System.Collections.Generic;
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

            //用哪种？

            //Action action1 = new Action(haarProcess.StartEyes);
            //Action action2= new Action(haarProcess.StartEars);
            //Action action3 = new Action(haarProcess.StartNose);
            //Action action4 = new Action(haarProcess.StartMouth);
            //Task task1 = new Task(action1);
            //Task task2 = new Task(action2);
            //Task task3 = new Task(action3);
            //Task task4 = new Task(action4);


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
        public bool isStartEyes { get; set; }
        public bool isStartEars { get; set; }
        public bool isStartNose { get; set; }
        public bool isStartMouth { get; set; }
        public int _nDetection { get; set; }
        public int _minSize { get; set; }
        public int _maxSize { get; set; }

        readonly HaarClassifier haarClassifier = null;
        public HaarProcess(string haarClassifierPath, string imageInputPath, string imageBlockOutputPath, int nDetection, int minSize, int maxSize)
        {
            haarClassifier = new HaarClassifier(haarClassifierPath, imageInputPath, imageBlockOutputPath);
            _nDetection = nDetection;
            _minSize = minSize;
            _maxSize = maxSize;
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
