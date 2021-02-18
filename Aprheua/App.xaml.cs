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

    }
}
