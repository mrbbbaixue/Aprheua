using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using Aprheua.Commands;

namespace Aprheua.ViewModels
{
    public class MainWindow : NotificationObject
    {
        #region 变量 Variables
        private string _windowTitle;
        public string WindowTitle 
        { 
            get { return _windowTitle; }
            set
            {
                _windowTitle = value;
                this.RaisePropertyChanged("WindowTitle");
            }
        }

        private Models.OriginImage _selectedImage;
        public Models.OriginImage SelectedImage
        {
            get { return _selectedImage; }
            set
            {
                _selectedImage = value;
                this.RaisePropertyChanged("SelectedImage");
            }
        }

        private bool _selectAllCheckBoxIsChecked;
        public bool SelectAllCheckBoxIsChecked
        {
            get { return _selectAllCheckBoxIsChecked; }
            set
            {
                _selectAllCheckBoxIsChecked = value;
                this.RaisePropertyChanged("SelectAllCheckBoxIsChecked");
            }
        }
        #endregion

        #region 数据 Datas
        public ObservableCollection<Models.OriginImage> SourceImages { get; set; }
        #endregion

        #region 命令 Commands
        public DelegateCommand ImportCommand { get; set; }
        public void Import(object parameter)
        {
            string[] paths;
            //打开文件（允许多选）
            var dialog = new Microsoft.Win32.OpenFileDialog
            {
                Multiselect = true,
                InitialDirectory = Environment.CurrentDirectory,
                Filter = "图像文件(*.bmp, *.jpg, *.png)|*.bmp;*.jpg;*.png"
            };
            if ((bool)dialog.ShowDialog())
            {
                paths = dialog.FileNames; 
                foreach (string path in paths)
                {
                    var sourceImage = new Models.OriginImage(path);
                    SourceImages.Add(sourceImage);
                }
            }
        }
        #endregion

        #region 事件 Events
        public DelegateCommand SelectAllCheckBoxClickEvent { get; set; }
        public void SelectAllCheckBoxClick(object parameter)
        {
            foreach(var sourceImage in SourceImages)
            {
                sourceImage.IsSelected = SelectAllCheckBoxIsChecked;
            }
        }
        #endregion

        public MainWindow()
        {
            #region 变量 Variables
            WindowTitle = $"Aprheua 脸谱分割展示程序 - {Environment.CurrentDirectory}";
            SelectedImage = new Models.OriginImage(Path.Combine(Path.Combine(Environment.CurrentDirectory,"resources"), "default-SelectedImage.png"));
            SelectAllCheckBoxIsChecked = false;
            #endregion

            #region 数据 Datas
            SourceImages = new ObservableCollection<Models.OriginImage> { };
            #endregion

            #region 命令 Commands
            ImportCommand = new DelegateCommand(new Action<object>(Import));
            #endregion

            #region 事件 Events
            SelectAllCheckBoxClickEvent = new DelegateCommand(new Action<object>(SelectAllCheckBoxClick));
            #endregion

            //测试事件
            var testImage = new Models.OriginImage("resources\\test.jpg");
            SourceImages.Add(testImage);
        }
    }
}
