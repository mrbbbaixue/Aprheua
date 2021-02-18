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

        private string _imageViewerSource;
        public string ImageViewerSource
        {
            get { return _imageViewerSource; }
            set
            {
                _imageViewerSource = value;
                this.RaisePropertyChanged("ImageViewerSource");
            }
        }
        #endregion

        #region 数据 Datas
        public ObservableCollection<Models.OriginImage> SourceImages { get; set; }
        public List<Models.OriginImage> SelectedImages { get; set; }
        //ToDo : 或许可以优化,不要用SelectedImages来承载，而直接对SourceImages进行修改
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
                    var sourceImage = new Models.OriginImage
                    {
                        Path = Path.GetFullPath(path),
                        Name = Path.GetFileName(path),
                        NumberOfBlocks = 0
                    };
                    sourceImage.GenerateThumbImage();
                    //ToDo : （低优先级）或许有优化空间？在SourceImages里采用Async Task
                    SourceImages.Add(sourceImage);
                }
            }
        }
        #endregion

        #region 事件 Events

        #endregion

        public MainWindow()
        {
            #region 变量 Variables
            WindowTitle = $"Aprheua 脸谱分割展示程序 - {Environment.CurrentDirectory}";
            ImageViewerSource = "";
            #endregion

            #region 数据 Datas
            SourceImages = new ObservableCollection<Models.OriginImage> { };
            SelectedImages = new List<Models.OriginImage> { };
            #endregion

            #region 命令 Commands
            ImportCommand = new DelegateCommand(new Action<object>(Import));
            #endregion

            #region 事件 Evnets

            #endregion
        }
    }
}
