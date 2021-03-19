/****************************************************************
                   Copyright © 2021 Aprheua

    File Name:     ViewModels/AnalyseWindow.cs

    Author:        Chenhao Wang (MrBBBaiXue@github.com)
                   Boyan Wang (JingNianNian@github.com)

    Version:       2.3.3.3

    Date:          2021-03-11

    Description:   AnalyseWindow 的后台绑定源

    Classes:       AnalyseWindow : NotificationObject
                   // NotificationObject的继承，绑定处理窗口的数据，
                      完成分类分析操作。

****************************************************************/

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using OpenCvSharp;

namespace Aprheua.ViewModels
{
    public class AnalyseWindow : NotificationObject
    {
        // 提供对MainWindowViewModel的引用访问
        private ViewModels.MainWindow MainWindow => App.MainWindowViewModel;

        private int _selectedCount;

        private string _windowTitle;
        public string WindowTitle
        {
            get { return _windowTitle; }
            set
            {
                _windowTitle = value;
                RaisePropertyChanged("WindowTitle");
            }
        }

        private string _selectedImagesNotification;
        public string SelectedImagesNotification
        {
            get { return _selectedImagesNotification; }
            set
            {
                _selectedImagesNotification = value;
                RaisePropertyChanged("SelectedImagesNotification");
                // 对于选中了几张图片的通知
            }
        }

        private int _minSize;
        public int MinSize
        {
            get { return _minSize; }
            set
            {
                _minSize = value;
                RaisePropertyChanged("MinSize");
            }
        }

        private int _maxSize;
        public int MaxSize
        {
            get { return _maxSize; }
            set
            {
                _maxSize = value;
                RaisePropertyChanged("MaxSize");
            }
        }

        private int _nDetection;
        public int NDetection
        {
            get { return _nDetection; }
            set
            {
                _nDetection = value;
                RaisePropertyChanged("NDetection");
            }
        }
        // 分类器的绑定列表
        public ObservableCollection<Models.HaarClassifier> Classifiers { get; set; }
        public Commands.DelegateCommand StartButtonClickEvent { get; set; }
        public Commands.DelegateCommand CloseWindowClick { get; set; }

        // 主要HAAR分析函数
        public void StartAnalyse(object parameter)
        {
            App.Log.OpenCV("Analyse Process Started...");
            App.Log.OpenCV($"MinSize : {MinSize} " +
                         $"MaxSize : {MaxSize} " +
                         $"NDetection : {NDetection} " +
                         $"_selectedCount : {_selectedCount} "
                         );
            foreach (var sourceImage in MainWindow.SourceImages)
            {
                // 遍历SourceImage列表
                if (sourceImage.IsSelected)
                {
                    // 这个Rects列表用来绘制分割块的图片。
                    List<Rect> rects = new List<Rect>();
                    foreach (var classifier in Classifiers)
                    {
                        // 遍历Classifier列表
                        if (classifier.IsSelected)
                        {
                            var categoryIndex = -1;
                            var classifierOutputPath = System.IO.Path.Combine(App.AprheuaCategoriesFolder, sourceImage.Name, classifier.Name);
                            App.Log.OpenCV($"Using {classifier.Name} to analyse {sourceImage.Path}, output to {classifierOutputPath}");
                            bool IsCategoryAlreadyCreated()
                            {
                                // 内联函数，检查是否已经使用过分类器，或者说有没有同名分类被创建
                                // 减少多个分类器同时执行的混乱
                                foreach (var category in sourceImage.ImageCategories)
                                {
                                    if (category.Name == classifier.Name)
                                    {
                                        App.Log.OpenCV($"Category {category.Name} has been already taken, will only scan that folder!");
                                        categoryIndex = sourceImage.ImageCategories.IndexOf(category);
                                        return true;
                                    }
                                }
                                return false;
                            }
                            // 检查分类器分类是否已经使用
                            if (!IsCategoryAlreadyCreated())
                            {
                                categoryIndex = sourceImage.AddCategory(classifierOutputPath, classifier.Name);
                            }
                            // 不使用异步，虽然可能会造成卡顿，但是如果要异步修改和主线程进行绑定的ObservableCollection就太麻烦了
                            var outputRects = classifier.StartHaarClassifier(sourceImage.Path, classifierOutputPath, NDetection, MinSize, MaxSize);
                            rects.AddRange(outputRects);
                            // 把目前分类器分类出的区块加入临时列表
                            if (categoryIndex != -1)
                            {
                                var hasNewFiles = sourceImage.ImageCategories[categoryIndex].ScanImages();
                                App.Log.OpenCV($"Index : {categoryIndex}, hasNewFiles : {hasNewFiles}");
                            }
                            sourceImage.ScanNumberOfImageBlocks(null);
                            // 扫描图片块数目
                        }
                    }
                    if (rects != null)
                    {
                        // 如果有分类出东西，那么就画框框
                        Models.HaarClassifier.WriteRectsToImage(rects, sourceImage.Path,sourceImage.OverlayImagePath);
                    }
                }
            }
            CloseWindowClick.Execute(this);
            // 关闭窗口
        }

        // 初始化。扫描分类器，处理关闭事件
        public void Init(Commands.DelegateCommand closeWindowClick)
        {
            _selectedCount = 0;
            foreach(var img in MainWindow.SourceImages)
            {
                if (img.IsSelected)
                {
                    _selectedCount++;
                }
            }
            // 检查哪些SourceImage被选中了
            App.Log.Info($"selectedCount : {_selectedCount}");
            App.Log.Info($"Scanning classifiers under {App.AprheuaClassifiersFolder}");
            Classifiers = ScanHaarClassifierFolder(App.AprheuaClassifiersFolder);
            // 扫描分类器
            CloseWindowClick = closeWindowClick;
            // 与前台代码的通信，关闭窗口的通信
            SelectedImagesNotification = $"对已选的 {_selectedCount} 个图像应用 HAAR 分析 :";
        }
        private ObservableCollection<Models.HaarClassifier> ScanHaarClassifierFolder(string path)
        {
            var classifiers = new ObservableCollection<Models.HaarClassifier>();
            var files = System.IO.Directory
                                 .EnumerateFiles(path, "*.xml");
            // HAAR的数据集以.xml结尾
            foreach(var file in files)
            {
                var classifier = new Models.HaarClassifier(file);
                classifiers.Add(classifier);
                App.Log.Info($"From ScanHaarClassifier : Classifier {file} added.");
            }
            return classifiers;
        }
        public AnalyseWindow()
        {
            WindowTitle = "HAAR 分析窗口";
            SelectedImagesNotification = "";
            MinSize = 20;
            MaxSize = 100;
            NDetection = 2;
            // 推荐的默认分类参数
            Classifiers = new ObservableCollection<Models.HaarClassifier>();
            StartButtonClickEvent = new Commands.DelegateCommand(new Action<object>(StartAnalyse));
            _selectedCount = 0;
        }
    }
}
