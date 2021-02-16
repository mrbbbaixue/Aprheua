using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aprheua.Commands;

namespace Aprheua.ViewModels
{
    public class MainWindow : NotificationObject
    {
        //Datas
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
        public ObservableCollection<Models.SourceImage> SourceImages { get; set; }

        //Commands
        public DelegateCommand ImportCommand { get; set; }
        public void Import(object parameter)
        {
            string[] paths = { };
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
                    var sourceImage = new Models.SourceImage
                    {
                        Path = Path.GetFullPath(path),
                        Name = Path.GetFileName(path)
                    };
                    sourceImage.GenerateThumbImage();
                    //ToDo : 或许有优化空间？
                    SourceImages.Add(sourceImage);
                }
            }
        }

        //
        public MainWindow()
        {
            //Datas
            WindowTitle = $"Aprheua 脸谱分割展示程序 - {Environment.CurrentDirectory}";
            SourceImages = new ObservableCollection<Models.SourceImage> { };
            //Commands
            ImportCommand = new DelegateCommand(new Action<object>(Import));
        }
    }
}
