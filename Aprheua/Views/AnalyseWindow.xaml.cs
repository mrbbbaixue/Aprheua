namespace Aprheua.Views
{
    /// <summary>
    /// AnalyseWindow.xaml 的交互逻辑
    /// </summary>
    public partial class AnalyseWindow : HandyControl.Controls.Window
    {
        public AnalyseWindow()
        {
            InitializeComponent();
            DataContext = App.AnalyseWindowViewModel;
            //Trigger Init function
            App.AnalyseWindowViewModel.Init();
            App.Log.Info("Test Message : App.AnalyseWindowViewModel.Init triggered !");
        }
    }
}
