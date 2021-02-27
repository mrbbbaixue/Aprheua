using System;
using System.IO;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Aprheua
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        public static string AprheuaTempFolder => Path.Combine(Environment.GetEnvironmentVariable("temp"),"Aprheua");
        public static string AprheuaThumbImagesFolder => Path.Combine(AprheuaTempFolder,"ThumbImages");
        public static string AprheuaOverlayImagesFolder => Path.Combine(AprheuaTempFolder,"OverlayImages");
        public static string AprheuaCategoriesFolder => Path.Combine(AprheuaTempFolder, "Categories");
        public static string AprheuaLogsFolder => Path.Combine(AprheuaTempFolder, "Logs");
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            // 第一步 : 创建文件夹
            if (!Directory.Exists(AprheuaTempFolder))
            {
                Directory.CreateDirectory(AprheuaTempFolder);
            }
            if (!Directory.Exists(AprheuaThumbImagesFolder))
            {
                Directory.CreateDirectory(AprheuaThumbImagesFolder);
            }
            if (!Directory.Exists(AprheuaOverlayImagesFolder))
            {
                Directory.CreateDirectory(AprheuaOverlayImagesFolder);
            }
            if (!Directory.Exists(AprheuaLogsFolder))
            {
                Directory.CreateDirectory(AprheuaLogsFolder);
            }
            if (!Directory.Exists(AprheuaCategoriesFolder))
            {
                Directory.CreateDirectory(AprheuaCategoriesFolder);
            }
            // 第二步 : 主题设置
            var brush = (System.Windows.Media.Brush)new System.Windows.Media.BrushConverter().ConvertFromString("#F52443");
            HandyControl.Themes.ThemeManager.Current.AccentColor = brush;
            //Final Step : Open MainWindow
            CreateMainWindow();
        }
        //ToDo : 程序运行完销毁临时文件夹
        public static void CreateAddCategoryWindow()
        {

        }

        public static void CreateMainWindow()
        {
            var mainwindow = new Aprheua.Views.MainWindow();
            mainwindow.Show();
            return;
        }
    }
}
