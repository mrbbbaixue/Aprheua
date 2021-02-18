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
        private ViewModels.MainWindow MainWindowViewModel { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            MainWindowViewModel = new ViewModels.MainWindow();
            DataContext = MainWindowViewModel;
        }
    }
}
