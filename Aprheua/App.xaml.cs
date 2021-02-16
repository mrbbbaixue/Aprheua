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
        public static string AprheuaTemp => Path.Combine(Environment.GetEnvironmentVariable("temp"),"Aprheua");
        public static string AprheuaTempThumbImages => Path.Combine(AprheuaTemp,"ThumbImages");

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            // Step 1 : Create Folders
            if (!Directory.Exists(AprheuaTemp))
            {
                Directory.CreateDirectory(AprheuaTemp);
            }
            if (!Directory.Exists(AprheuaTempThumbImages))
            {
                Directory.CreateDirectory(AprheuaTempThumbImages);
            }

            //Final Step : Open MainWindow
            var mainWindow = new Aprheua.Views.MainWindow();
            mainWindow.Show();
        }
        //ToDo : 程序运行完销毁临时文件夹

    }
}
