/****************************************************************
                   Copyright © 2021 Aprheua

    File Name:     App.xaml.cs

    Author:        Chenhao Wang (MrBBBaiXue@github.com)
                   Boyan Wang (JingNianNian@github.com)

    Version:       2.3.3.3

    Date:          2021-03-11

    Description:   整个程序的入口点，处理程序启动前的清理操作进行错误处
                   理，创建窗口。

    Classes:       App : Application
                   // 整个App的入口点，由App.xaml路由

****************************************************************/

using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;

namespace Aprheua
{
    public partial class App : Application
    {
        public static string AprheuaTempFolder => Path.Combine(Environment.GetEnvironmentVariable("temp"), "Aprheua");
        public static string AprheuaThumbImagesFolder => Path.Combine(AprheuaTempFolder, "ThumbImages");
        public static string AprheuaOverlayImagesFolder => Path.Combine(AprheuaTempFolder, "OverlayImages");
        public static string AprheuaCategoriesFolder => Path.Combine(AprheuaTempFolder, "Categories");
        public static string AprheuaLogsFolder => Path.Combine(AprheuaTempFolder, "Logs");
        public static string AprheuaClassifiersFolder => Path.Combine(Environment.CurrentDirectory, "classifiers");
        public static string AprheuaResourceFolder => Path.Combine(Environment.CurrentDirectory,"resources");
        public static string LogFilePrefix => "Aprheua-log-";
        public static Models.LogWriter Log { get; set; }
        public static ViewModels.MainWindow MainWindowViewModel { get; set; }
        public static ViewModels.AnalyseWindow AnalyseWindowViewModel { get; set; }
        public static Views.MainWindow MainWindowWindow { get; set; }
        public static Views.AddCategoryWindow AddCategoryWindowWindow { get; set; }
        public static Views.AnalyseWindow AnalyseWindowWindow { get; set; }
        private void Application_Startup(object sender, StartupEventArgs e)
        {

            // 第一步 : 创建程序的临时文件夹，用于存放各种缩略图，图片块和日志。
            // If Not Exist then Create (Consider using Delegate)
            if (!Directory.Exists(AprheuaTempFolder))
            {
                Directory.CreateDirectory(AprheuaTempFolder);
            }
            if (Directory.Exists(AprheuaThumbImagesFolder))
            {
                Models.Utility.DeleteFolder(AprheuaThumbImagesFolder);
            }
            if (!Directory.Exists(AprheuaThumbImagesFolder))
            {
                Directory.CreateDirectory(AprheuaThumbImagesFolder);
            }
            if (Directory.Exists(AprheuaOverlayImagesFolder))
            {
                Models.Utility.DeleteFolder(AprheuaOverlayImagesFolder);
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
            // 第二步 : 创建日志
            var files = Directory
                .EnumerateFiles(AprheuaLogsFolder, $"{LogFilePrefix}*")
                .OrderByDescending(x => x);
            foreach (var file in files.Skip(4))
            {
                // 如果Log文件超过5个，最早的会被删除
                try
                {
                    File.Delete(file);
                }
                catch { }
            }
            Log = new Models.LogWriter(AprheuaLogsFolder);
            var assembly = Assembly.GetExecutingAssembly();
            Log.Info($"Aprheua Image HaarClassifier GUI Application by Hi-Icy. Version {assembly.GetName().Version}");
            Log.Info("Application launched!");

            // 第三步 : 主题设置
            var brush = (System.Windows.Media.Brush)new System.Windows.Media.BrushConverter().ConvertFromString("#F52443");
            HandyControl.Themes.ThemeManager.Current.AccentColor = brush;
            Log.Info($"Accent Color is set to {brush}");

            //第四步 : 初始化ViewModel
            MainWindowViewModel = new ViewModels.MainWindow();
            Log.Info("MainWindowViewModel is successfully initialized.");
            AnalyseWindowViewModel = new ViewModels.AnalyseWindow();
            Log.Info("AnalyseWindowViewModel is successfully initialized.");
            //最后一步 : 创建MainWindow
            CreateMainWindow();
        }
        private void Application_Exit(object sender, ExitEventArgs e)
        {
            //程序退出 清理Categories文件夹
            if (Directory.Exists(AprheuaCategoriesFolder))
            {
                Models.Utility.DeleteFolder(AprheuaCategoriesFolder);
            }
            Current.Shutdown();
        }
        public static void CreateMainWindow()
        {
            MainWindowWindow = new Aprheua.Views.MainWindow();
            MainWindowWindow.Show();
            Log.Info("MainWindow Created!");
            return;
        }
        public static string CreateAddCategoryWindow()
        {
            AddCategoryWindowWindow = new Aprheua.Views.AddCategoryWindow
            {
                Owner = MainWindowWindow
            };
            Log.Info("AddCategoryWindow Created!");
            AddCategoryWindowWindow.ShowDialog();
            if (AddCategoryWindowWindow.IsOKClicked)
            {
                Log.Info($"addCategoryWindow.IsOKClicked : {AddCategoryWindowWindow.IsOKClicked}");
                Log.Info($"addCategoryWindow.CategoryName : {AddCategoryWindowWindow.CategoryName}");
                return AddCategoryWindowWindow.CategoryName;
            }
            Log.Error($"addCategoryWindow.IsOKClicked is {AddCategoryWindowWindow.IsOKClicked}, will return null CategoryName!");
            return null;
        }
        public static void CreateAnalyseWindow()
        {
            AnalyseWindowWindow = new Aprheua.Views.AnalyseWindow
            {
                Owner = MainWindowWindow
            };
            Log.Info("AnalyseWindow Created!");
            AnalyseWindowWindow.ShowDialog();
        }
        public static void CreateClipWindow(string sourceImagePath, string targetSavePath)
        {
            var clipWindow = new Aprheua.Views.ClipWindow
            {
                SourceImagePath = sourceImagePath,
                TargetSavePath = targetSavePath
            };
            Log.Info("ClipWindow Created!");
            Log.Info($"sourceImagePath : {sourceImagePath}, targetSavePath : {targetSavePath}.");
            clipWindow.ShowDialog();
        }
        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show(e.Exception.Message);
            Application_Exit(sender, null);
        }
    }
}
