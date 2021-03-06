using System;

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
            SelectedImagesNotification = $"对已选的{selectedCount}项应用HAAR分析 :";
        }
        public AnalyseWindow()
        {
            WindowTitle = "HAAR 自动分析";
            SelectedImagesNotification = "";
        }
    }
}
