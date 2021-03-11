/****************************************************************
                   Copyright © 2021 Aprheua

    File Name:     Views/MainWindow.xaml.cs

    Author:        Chenhao Wang (MrBBBaiXue@github.com)

    Version:       2.3.3.3

    Date:          2021-03-01

    Description:   MainWindow.xaml 的交互逻辑

    Classes:       MainWindow : HandyControl.Controls.GlowWindow
                   // HandyControl辉光窗口的继承

****************************************************************/

namespace Aprheua.Views
{
    public partial class MainWindow : HandyControl.Controls.GlowWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = App.MainWindowViewModel;
        }
    }
}
