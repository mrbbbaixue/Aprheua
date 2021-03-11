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

        private bool _isProcessing;
        public bool IsProcessing
        {
            get { return _isProcessing; }
            set
            {
                _isProcessing = value;
                RaisePropertyChanged("IsProcessing");
                RaisePropertyChanged("IsNotProcessing");
            }
        }
        public bool IsNotProcessing => !IsProcessing;
        public ObservableCollection<Models.HaarClassifier> Classifiers { get; set; }
        public Commands.DelegateCommand StartButtonClickEvent { get; set; }
        public Commands.DelegateCommand CloseWindowClick { get; set; }

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
                    foreach (var classifier in Classifiers)
                    {
                        if (classifier.IsSelected)
                        {
                            var categoryIndex = -1;
                            var classifierOutputPath = System.IO.Path.Combine(App.AprheuaCategoriesFolder, sourceImage.Name, classifier.Name);
                            App.Log.Info($"Using {classifier.Name} to analyse {sourceImage.Path}, output to {classifierOutputPath}");
                            bool IsCategoryAlreadyCreated()
                            {
                                foreach (var category in sourceImage.ImageCategories)
                                {
                                    if (category.Name == classifier.Name)
                                    {
                                        App.Log.Info($"Category {category.Name} has been already taken, will only scan that folder!");
                                        categoryIndex = sourceImage.ImageCategories.IndexOf(category);
                                        return true;
                                    }
                                }
                                return false;
                            }
                            if (!IsCategoryAlreadyCreated())
                            {
                                categoryIndex = sourceImage.AddCategory(classifierOutputPath, classifier.Name);
                            }
                            classifier.StartHaarClassifier(sourceImage.Path, classifierOutputPath, NDetection, MinSize, MaxSize);
                            if (categoryIndex != -1)
                            {
                                var hasNewFiles = sourceImage.ImageCategories[categoryIndex].ScanImages();
                                App.Log.Info($"Index : {categoryIndex}, hasNewFiles : {hasNewFiles}");
                            }
                        }
                    }
                }
            }
            //ToDo : 显示进度条，等待完成，（如果可以的话）异步
            CloseWindowClick.Execute(this);
        }
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
            App.Log.Info($"selectedCount : {_selectedCount}");
            App.Log.Info($"Scanning classifiers under {App.AprheuaClassifiersFolder}");
            Classifiers = ScanHaarClassifierFolder(App.AprheuaClassifiersFolder);
            CloseWindowClick = closeWindowClick;
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
            IsProcessing = false;
            Classifiers = new ObservableCollection<Models.HaarClassifier>();
            StartButtonClickEvent = new Commands.DelegateCommand(new Action<object>(StartAnalyse));
            _selectedCount = 0;
        }
    }
}
