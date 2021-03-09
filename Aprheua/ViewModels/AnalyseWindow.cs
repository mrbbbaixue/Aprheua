using System;
using System.Collections.ObjectModel;

namespace Aprheua.ViewModels
{
    public class AnalyseWindow : NotificationObject
    {
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
        public ObservableCollection<Models.HaarClassifier> Classifiers { get; set; }
        public Commands.DelegateCommand StartButtonClickEvent { get; set; }
        public void StartAnalyse(object parameter)
        {
            App.Log.Info("Analyse Process Started...");
            App.Log.Info($"MinSize : {MinSize} " +
                         $"MaxSize : {MaxSize} " +
                         $"NDetection : {NDetection} " +
                         $"_selectedCount : {_selectedCount} "
                         );
           foreach (var sourceImage in MainWindow.SourceImages)
           {
                if (sourceImage.IsSelected)
                {
                    foreach(var classifier in Classifiers)
                    {
                        if (classifier.IsSelected)
                        {
                            var classifierOutputPath = System.IO.Path.Combine(App.AprheuaCategoriesFolder, sourceImage.Name, classifier.Name);
                            App.Log.Info($"Using {classifier.Name} to analyse {sourceImage.Path}, output to {classifierOutputPath}");
                            classifier.StartHaarClassifier(sourceImage.Path, classifierOutputPath, NDetection, MinSize, MaxSize);
                            //ToDo : 如果已有分类，需要记得合并
                            sourceImage.AddCategory(classifierOutputPath, classifier.Name);
                        }
                    }
                }
           }
        }
        public void Init()
        {
            foreach(var img in MainWindow.SourceImages)
            {
                if (img.IsSelected)
                {
                    _selectedCount++;
                }
            }
            App.Log.Info($"selectedCount : {_selectedCount}");
            App.Log.Info($"Scanning classifiers under {App.AprheuaClassifiersFolder}");
            Classifiers = ScanHaarClassifierFolder(App.AprheuaClassifiersFolder);
            SelectedImagesNotification = $"对已选的 {_selectedCount} 个图像应用 HAAR 分析 :";
        }
        private ObservableCollection<Models.HaarClassifier> ScanHaarClassifierFolder(string path)
        {
            var classifiers = new ObservableCollection<Models.HaarClassifier>();
            var files = System.IO.Directory
                                 .EnumerateFiles(path, "*.xml");
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
            MaxSize = 50;
            NDetection = 0;
            Classifiers = new ObservableCollection<Models.HaarClassifier>();
            StartButtonClickEvent = new Commands.DelegateCommand(new Action<object>(StartAnalyse));
            _selectedCount = 0;
        }
    }
}
