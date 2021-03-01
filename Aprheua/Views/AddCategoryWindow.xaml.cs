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
using System.Windows.Shapes;

namespace Aprheua.Views
{
    /// <summary>
    /// AddCategoryWindow.xaml 的交互逻辑
    /// </summary>
    public partial class AddCategoryWindow : HandyControl.Controls.Window
    {
        public string WindowTitle { get; set; }
        public string CategoryName { get; set; }
        public AddCategoryWindow()
        {
            InitializeComponent();
            WindowTitle = $"添加分类";
            CategoryName = $"New Category - {Models.Utility.GetTimeStamp()}";
            DataContext = this;
        }
    }
}
