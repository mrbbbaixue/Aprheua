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
        private ViewModels.MainWindow MainWindowData { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            MainWindowData = new ViewModels.MainWindow();
            DataContext = MainWindowData;
        }
        private void ImagesListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //把IList转换至SelectImages并修改ViewModel中的属性，由各自属性通知Views
            //又耦合了，SingleWayBinding居然不能支持SelectedItems
            System.Collections.IList items = (System.Collections.IList)this.imagesListBox.SelectedItems;
            var collection = items.Cast<Aprheua.Models.OriginImage>();
            MainWindowData.SelectedImages = collection.ToList();
            MainWindowData.ImageViewerSource = MainWindowData.SelectedImages.FirstOrDefault().Path;
        }

    }
}
