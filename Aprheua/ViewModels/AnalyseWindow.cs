using System;
using System.Collections.ObjectModel;

namespace Aprheua.ViewModels
{
    public class AnalyseWindow : NotificationObject
    {
        private ViewModels.MainWindow MainWindow => App.MainWindowViewModel;

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

        private int _minNeighbors;
        public int MinNeighbors
        {
            get { return _minNeighbors; }
            set
            {
                _minNeighbors = value;
                RaisePropertyChanged("MinNeighbors");
            }
        }
        public ObservableCollection<Models.HaarClassifier> Classifiers { get; set; }
        public void Init()
        {
            var selectedCount = 0;
            foreach(var img in MainWindow.SourceImages)
            {
                if (img.IsSelected)
                {
                    selectedCount++;
                }
            }
            App.Log.Info($"selectedCount : {selectedCount}");
            App.Log.Info($"Scanning classifiers under {App.AprheuaClassifiersFolder}");
            Classifiers = ScanHaarClassifierFolder(App.AprheuaClassifiersFolder);
            SelectedImagesNotification = $"对已选的{selectedCount}个图像应用 HAAR 分析 :";
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
                App.Log.Info($"Classifier {file} added.");
            }
            return classifiers;
        }
        public AnalyseWindow()
        {
            WindowTitle = "HAAR 自动分析";
            SelectedImagesNotification = "";
            MinSize = 20;
            MaxSize = 50;
            MinNeighbors = 3;
            Classifiers = new ObservableCollection<Models.HaarClassifier>();
        }
    }
}
