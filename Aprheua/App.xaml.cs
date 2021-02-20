﻿using System;
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

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            #region Step 1 : Create Folders
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
            #endregion
            //Final Step : Open MainWindow
            var mainWindow = new Aprheua.Views.MainWindow();
            mainWindow.Show();
        }
        //ToDo : 程序运行完销毁临时文件夹
        //ToDo : 要绑定，且切换主题要在ViewModel进行，需要单独定义颜色
        public static void UpdateSkin(HandyControl.Data.SkinType skin)
        {
            HandyControl.Themes.SharedResourceDictionary.SharedDictionaries.Clear();
            Current.Resources.MergedDictionaries.Add(HandyControl.Tools.ResourceHelper.GetSkin(skin));
            Current.Resources.MergedDictionaries.Add(new ResourceDictionary
            {
                Source = new Uri("pack://application:,,,/HandyControl;component/Themes/Theme.xaml")
            });
            Current.MainWindow?.OnApplyTemplate();
        }
    }
}
