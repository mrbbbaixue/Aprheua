namespace Aprheua.Views
{
    /// <summary>
    /// AnalyseWindow.xaml 的交互逻辑
    /// </summary>
    public partial class AnalyseWindow : HandyControl.Controls.Window
    {
        private ViewModels.AnalyseWindow AnalyseWindowViewModel { get; set; }
        public AnalyseWindow()
        {
            InitializeComponent();
            AnalyseWindowViewModel = new ViewModels.AnalyseWindow();
            DataContext = AnalyseWindowViewModel;
        }
    }
}
