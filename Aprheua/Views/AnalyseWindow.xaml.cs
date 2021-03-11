namespace Aprheua.Views
{
    /// <summary>
    /// AnalyseWindow.xaml 的交互逻辑
    /// </summary>
    public partial class AnalyseWindow : HandyControl.Controls.Window
    {
        public Commands.DelegateCommand CloseWindowClick { get; set; }
        private void CloseWindow(object parameter)
        {
            this.Close();
        }
        public AnalyseWindow()
        {
            InitializeComponent();
            DataContext = App.AnalyseWindowViewModel;
            CloseWindowClick = new Commands.DelegateCommand(new System.Action<object>(CloseWindow));
            //Trigger Init function
            App.AnalyseWindowViewModel.Init(CloseWindowClick);
            App.Log.Info("App.AnalyseWindowViewModel.Init triggered !");
        }
    }
}
