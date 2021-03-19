/****************************************************************
                   Copyright © 2021 Aprheua

    File Name:     Views/ClipWindow.xaml.cs

    Author:        Chenhao Wang (MrBBBaiXue@github.com)
                   Boyan Wang (JingNianNian@github.com)

    Version:       1.0.0.0

    Date:          2021-03-19

    Description:   ClipWindow.xaml 的交互逻辑

    Classes:       ClipWindow : Window
                   // 窗口的继承，完成截图并输出图片到目标文件夹

****************************************************************/

using System.IO;
using System.Windows;
using System.Windows.Input;

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

        private Point startPointToImage = new Point();

        private Point endPointToImage = new Point();

        private int imageWidth = 0;

        private int imageHeight = 0;

        private int screenWidth = 0;

        private int screenHeight = 0;

        private double scaleTimes = 1;

        //判断图片宽距离屏幕边缘是否有空间
        private bool widthSpace = false;
        //判断图片高距离屏幕边缘是否有空间
        private bool heightSpace = false;

        public ClipWindow()
        {
            InitializeComponent();
            DataContext = this;
        }
        private bool inClip = false;

        /// <summary>
        /// 加载截图相关信息的方法，计算使图片放大的倍率
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClipWindow_Loaded(object sender, RoutedEventArgs e)
        {
            //获得图片的大小
            using (FileStream fs = new FileStream(SourceImagePath, FileMode.Open, FileAccess.Read))
            {
                System.Drawing.Image image = System.Drawing.Image.FromStream(fs);
                imageWidth = image.Width;
                imageHeight = image.Height;
            }
            App.Log.Info($"Image : Width : {imageWidth} Height : {imageHeight}");
            //获得显示器分辨率
            screenWidth = (int)SystemParameters.PrimaryScreenWidth;
            screenHeight = (int)SystemParameters.PrimaryScreenHeight;
            App.Log.Info($"Screen : Width : {screenWidth} Height : {screenHeight}");

            //计算缩放比例
            //采用图片长宽比相对于屏幕长宽比来决定缩放比例
            if ((double)imageWidth / (double)imageHeight > (double)screenWidth / (double)screenHeight)
            {
                scaleTimes = (double)screenWidth / (double)imageWidth;
                heightSpace = true;
            }
            else
            {
                scaleTimes = (double)screenHeight / (double)imageHeight;
                widthSpace = true;
            }
            App.Log.Info($"ScaleTimes : {scaleTimes}");
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

        /// <summary>
        /// 当鼠标移动时触发，按下左键并正向拖动时计算所选区域的Width和Height。
        /// 若反向移动或未按下鼠标左键，则直接退出该方法。
        /// </summary>
        private void Image_MouseMove(object sender, MouseEventArgs e)
        {
            if (!inClip)
            { return; }
            if ((int)e.GetPosition(image).X - startPoint.x - 1 < 0
             || (int)e.GetPosition(image).Y - startPoint.y - 1 < 0)
            { return; }
            borderClipArea.Width = (int)e.GetPosition(image).X - startPoint.x - 1;
            borderClipArea.Height = (int)e.GetPosition(image).Y - startPoint.y - 1;
        }

        /// <summary>
        /// 当鼠标左键按下时触发，记录按下像素点的坐标以及换算到相对于原图的像素点坐标。
        /// </summary>
        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            inClip = true;
            var myThickness = new Thickness(e.GetPosition(image).X, e.GetPosition(image).Y, 0, 0);
            borderClipArea.Margin = myThickness;
            startPoint.x = (int)e.GetPosition(image).X;
            startPoint.y = (int)e.GetPosition(image).Y;
            startPointToImage.x = widthSpace ? (int)((startPoint.x - ((screenWidth - (imageWidth * scaleTimes)) / 2)) / scaleTimes) :
                                               (int)(startPoint.x / scaleTimes);
            startPointToImage.y = heightSpace ? (int)((startPoint.y - ((screenHeight - (imageHeight * scaleTimes)) / 2)) / scaleTimes) :
                                                (int)(startPoint.y / scaleTimes);
            App.Log.Info($"startPoint : X : {startPoint.x} Y : {startPoint.y}");
            App.Log.Info($"startPointToImage : X : {startPointToImage.x} Y : {startPointToImage.y}");
        }
        /// <summary>
        /// 当鼠标左键抬起时触发，记录释放像素点的坐标以及换算到相对于原图的像素点坐标。
        /// 进行误差判断，并调用CutPicture方法在原图片中截出所选区域图像。
        /// </summary>
        private void Image_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            endPoint.x = (int)e.GetPosition(image).X;
            endPoint.y = (int)e.GetPosition(image).Y;
            endPointToImage.x = widthSpace ? (int)((endPoint.x - ((screenWidth - (imageWidth * scaleTimes)) / 2)) / scaleTimes) :
                                               (int)(endPoint.x / scaleTimes);
            endPointToImage.y = heightSpace ? (int)((endPoint.y - ((screenHeight - (imageHeight * scaleTimes)) / 2)) / scaleTimes) :
                                                (int)(endPoint.y / scaleTimes);

            App.Log.Info($"endPoint : X : {endPoint.x} Y : {endPoint.y}");
            App.Log.Info($"endPointToImage : X : {endPointToImage.x} Y : {endPointToImage.y}");

            //如果截图是整个图片，进行误差判断。
            if (startPointToImage.x >= -10 && startPointToImage.x <= 10) { startPointToImage.x = 0; }
            if (startPointToImage.y >= -10 && startPointToImage.y <= 10) { startPointToImage.y = 0; }
            if (endPointToImage.x - imageWidth >= -10 && endPointToImage.x - imageWidth <= 10) { endPointToImage.x = imageWidth; }
            if (endPointToImage.y - imageHeight >= -10 && endPointToImage.y - imageHeight <= 10) { endPointToImage.y = imageHeight; }

            //调试看数据
            App.Log.Info($"startPoint : X : {startPointToImage.x} Y : {startPointToImage.y}");
            App.Log.Info($"endPoint : X : {endPointToImage.x} Y : {endPointToImage.y}");
            //调用裁剪方法，生成所选区域图片。
            CutPicture(startPointToImage.x,
                       startPointToImage.y,
                       endPointToImage.x - startPointToImage.x,
                       endPointToImage.y - startPointToImage.y
                       );
            Close();
        }
        /// <summary>
        /// 图片裁剪，生成所选区域图片。
        /// <param name="x">修改起点x坐标</param>
        /// <param name="y">修改起点y坐标</param>
        /// <param name="width">新图宽度</param>
        /// <param name="height">新图高度</param>
        /// </summary>
        private void CutPicture(int x, int y, int width, int height)
        {
            //定义截取矩形
            System.Drawing.Rectangle cropArea = new System.Drawing.Rectangle(x, y, width, height);
            //要截取的区域大小
            //加载图片
            System.Drawing.Image img = System.Drawing.Image.FromStream(new System.IO.MemoryStream(System.IO.File.ReadAllBytes(SourceImagePath)));
            //判断超出的位置否
            if ((img.Width <= x + width) || img.Height <= y + height)
            {
                MessageBox.Show("裁剪尺寸超出原有尺寸！");
                img.Dispose();
                return;
            }
            //定义Bitmap对象
            System.Drawing.Bitmap bmpImage = new System.Drawing.Bitmap(img);
            //进行裁剪
            System.Drawing.Bitmap bmpCrop = bmpImage.Clone(cropArea, bmpImage.PixelFormat);
            //保存成新文件
            bmpCrop.Save(TargetSavePath);
            //释放对象
            img.Dispose();
            bmpCrop.Dispose();
        }
    }
}

