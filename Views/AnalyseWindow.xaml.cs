/****************************************************************
                   Copyright © 2021 Aprheua

    File Name:     Views/AnalyseWindow.xaml.cs

    Author:        Chenhao Wang (MrBBBaiXue@github.com)
                   Boyan Wang (JingNianNian@github.com)

    Version:       2.3.3.3

    Date:          2021-03-09

    Description:   AnalyseWindow.xaml 的交互逻辑

    Classes:       AnalyseWindow : HandyControl.Controls.Window
                   // HandyControl窗口的继承，重写构造器，初始化分类
                      器数据

****************************************************************/

namespace Aprheua.Views
{
    public partial class AnalyseWindow : HandyControl.Controls.Window
    {
        public Commands.DelegateCommand CloseWindowClick { get; set; }
        private void CloseWindow(object parameter)
        {
            Close();
        }
        public AnalyseWindow()
        {
            InitializeComponent();
            DataContext = App.AnalyseWindowViewModel;
            CloseWindowClick = new Commands.DelegateCommand(new System.Action<object>(CloseWindow));
            //触发初始化函数
            App.AnalyseWindowViewModel.Init(CloseWindowClick);
            App.Log.Info("App.AnalyseWindowViewModel.Init triggered !");
        }
    }
}
