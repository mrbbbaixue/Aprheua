using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using Aprheua;

namespace Aprheua.Views
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : HandyControl.Controls.GlowWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new ViewModels.MainWindow();
        }
        private void mainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var sourceImage = new Models.SourceImage
            {
                Path = @"E:\GitHub\Hi-Icy\Aprheua\2.jpg",
                Name = $"Test2.jpg"
            };
            MainWindowViewModel.SourceImages.Add(sourceImage);
            MainWindowViewModel.SourceImages.Add(sourceImage);
        }
    }
}
