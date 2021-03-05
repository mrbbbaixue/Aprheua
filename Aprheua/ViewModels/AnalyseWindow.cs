namespace Aprheua.ViewModels
{
    public class AnalyseWindow : NotificationObject 
    {
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

        public AnalyseWindow()
        {
            WindowTitle = "HAAR 自动分析";
        }
    }
}
