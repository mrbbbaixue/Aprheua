/****************************************************************
                   Copyright © 2021 Aprheua

    File Name:     Views/ClipWindow.xaml.cs

    Author:        Chenhao Wang (MrBBBaiXue@github.com)

    Version:       2.3.3.3

    Date:          2021-03-13

    Description:   ClipWindow.xaml 的交互逻辑

    Classes:       ClipWindow : Window
                   // 窗口的继承，完成截图并输出图片到目标文件夹

****************************************************************/

using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Aprheua.Views
{
    /// <summary>
    /// 截图窗口
    /// <param name="SourceImagePath">截图图片来源</param>
    /// <param name="TargetSavePath">保存图片路径</param>
    /// </summary>
    public partial class ClipWindow : Window
    {
        public string SourceImagePath { get; set; }
        public string TargetSavePath { get; set; }

        public struct Point {
            public int x;
            public int y;
        }

        private Point startPoint = new Point();

        private Point endPoint = new Point();
        public ClipWindow()
        {
            InitializeComponent();
            DataContext = this;
        }
        private void ClipWindow_MouseRightButtonDown(object sender, MouseButtonEventArgs e) => Close();
        private void ClipWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                Close();
            }
            if (e.Key == Key.Enter)
            {
                Close();
            }
        }

        private void Image_MouseMove(object sender, MouseEventArgs e)
        {

        }

        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            startPoint.x = (int)e.GetPosition(image).X;
            startPoint.y = (int)e.GetPosition(image).Y;
        }

        private void Image_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            endPoint.x = (int)e.GetPosition(image).X;
            endPoint.y = (int)e.GetPosition(image).Y;
            App.Log.Info($"startPoint : X : {startPoint.x} Y : {startPoint.y}");
            App.Log.Info($"endPoint : X : {endPoint.x} Y : {endPoint.y}");
            this.Close();
        }
    }
}

