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

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Aprheua.Views
{
    public partial class ClipWindow : Window
    {
        public string SourceImagePath { get; set; }
        public string TargetSavePath { get; set; }
        public ClipWindow()
        {
            InitializeComponent();
            DataContext = this;
            HandyControl.Controls.Screenshot.Snapped += Screenshot_Snapped;
        }
        private void Screenshot_Snapped(object sender, HandyControl.Data.FunctionEventArgs<ImageSource> e)
        {
            ImageSourceToBitmap(e.Info).Save(TargetSavePath, System.Drawing.Imaging.ImageFormat.Jpeg);
            this.Close();
        }
        private void ClipWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var screenshot = new HandyControl.Controls.Screenshot();
            screenshot.Start();
        }
        private void ClipWindow_MouseRightButtonDown(object sender, MouseButtonEventArgs e) => Close();
        private void ClipWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape) Close();
        }
        public static System.Drawing.Bitmap ImageSourceToBitmap(ImageSource imageSource)
        {
            BitmapSource m = (BitmapSource)imageSource;

            System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(m.PixelWidth, m.PixelHeight, System.Drawing.Imaging.PixelFormat.Format32bppPArgb); // 坑点：选Format32bppRgb将不带透明度

            System.Drawing.Imaging.BitmapData data = bmp.LockBits(
            new System.Drawing.Rectangle(System.Drawing.Point.Empty, bmp.Size), System.Drawing.Imaging.ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format32bppPArgb);

            m.CopyPixels(Int32Rect.Empty, data.Scan0, data.Height * data.Stride, data.Stride);
            bmp.UnlockBits(data);

            return bmp;
        }
    }
}

