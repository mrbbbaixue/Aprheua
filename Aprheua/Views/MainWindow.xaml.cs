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
        private ViewModels.MainWindow _dataContext { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            _dataContext = new ViewModels.MainWindow();
            DataContext = _dataContext;
        }

        //不得不耦合了（
        private void ImagesListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            System.Collections.IList items = (System.Collections.IList)this.imagesListBox.SelectedItems;
            var collection = items.Cast<Aprheua.Models.SourceImage>();
            _dataContext.SelectedImages = collection.ToList();
            //logging
            foreach(var image in _dataContext.SelectedImages)
            {
                Console.WriteLine(image.Path);
            }
        }

    }
}
