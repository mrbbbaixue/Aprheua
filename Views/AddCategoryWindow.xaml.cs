/****************************************************************
                   Copyright © 2021 Aprheua

    File Name:     Views/AddCategoryWindow.xaml.cs

    Author:        Chenhao Wang (MrBBBaiXue@github.com)
                   Boyan Wang (JingNianNian@github.com)

    Version:       2.3.3.3

    Date:          2021-02-27

    Description:   AddCategoryWindow.xaml.cs 的交互逻辑

    Classes:       AddCategoryWindow : HandyControl.Controls.Window
                   // HandyControl窗口的继承，重写构造器，获得新建分类的
                      名称，并处理返回。

****************************************************************/

using System.Windows;

namespace Aprheua.Views
{
    public partial class AddCategoryWindow : HandyControl.Controls.Window
    {
        public string WindowTitle { get; set; }
        public string CategoryName { get; set; }
        public bool IsOKClicked { get; set; }

        public AddCategoryWindow()
        {
            InitializeComponent();
            WindowTitle = "添加分类";
            CategoryName = $"New Category - {Models.Utility.GetTimeStamp()}";
            IsOKClicked = false;
            DataContext = this;
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            IsOKClicked = true;
            Close();
        }
    }
}
