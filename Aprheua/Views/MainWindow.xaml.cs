namespace Aprheua.Views
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : HandyControl.Controls.GlowWindow
    {
        private ViewModels.MainWindow MainWindowViewModel { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            MainWindowViewModel = new ViewModels.MainWindow();
            DataContext = MainWindowViewModel;
        }
    }
}
