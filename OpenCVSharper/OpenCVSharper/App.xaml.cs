using System.Windows;

namespace OpenCVSharper
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            CreateMainWindow();
        }
        public void ApplicationClose()
        {
            Shutdown();
        }
        public static void CreateMainWindow()
        {
            var mainWindow = new MainWindow
            {
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };
            mainWindow.Show();
            return;
        }

    }
}
