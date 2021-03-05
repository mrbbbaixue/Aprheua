using System.Windows;

namespace Aprheua.Views
{
    /// <summary>
    /// AddCategoryWindow.xaml 的交互逻辑
    /// </summary>
    public partial class AddCategoryWindow : HandyControl.Controls.Window
    {
        public string WindowTitle { get; set; }
        public string CategoryName { get; set; }
        public bool IsOKClicked { get; set; }

        public AddCategoryWindow()
        {
            InitializeComponent();
            WindowTitle = $"添加分类";
            CategoryName = $"New Category - {Models.Utility.GetTimeStamp()}";
            IsOKClicked = false;
            DataContext = this;
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            IsOKClicked = true;
            Close();
        }
    }
}
