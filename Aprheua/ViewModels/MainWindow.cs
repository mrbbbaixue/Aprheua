﻿using System;
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

        private Models.SourceImage _selectedImage;
        public Models.SourceImage SelectedImage
        {
            get { return _selectedImage; }
            set
            {
                _selectedImage = value;
                this.RaisePropertyChanged("SelectedImage");
            }
        }
        #endregion

        #region 数据 Datas
        public ObservableCollection<Models.SourceImage> SourceImages { get; set; }
        public ObservableCollection<Models.SourceImage> SelectedImages { get; set; }
        #endregion

        #region 命令 Commands
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
                        Name = Path.GetFileName(path),
                        NumberOfBlocks = 0
                    };
                    sourceImage.GenerateThumbImage();
                    //ToDo : 或许有优化空间？
                    SourceImages.Add(sourceImage);
                }
            }
        }
        #endregion

        #region 事件 Events
        public DelegateCommand ImagesListBoxSelectionChangedEvent { get; set; }
        public void ImagesListBoxSelectionChanged(object parameter)
        {
            WindowTitle = $"ImagesListBoxSelectionChanged() triggered! {Models.Utility.GetTimeStamp()}";
        }
        #endregion

        public MainWindow()
        {
            #region 变量 Variables
            WindowTitle = $"Aprheua 脸谱分割展示程序 - {Environment.CurrentDirectory}";
            SelectedImage = new Models.SourceImage { };
            #endregion

            #region 数据 Datas
            SourceImages = new ObservableCollection<Models.SourceImage> { };
            SelectedImages = new ObservableCollection<Models.SourceImage> { };
            #endregion

            #region 命令 Commands
            ImportCommand = new DelegateCommand(new Action<object>(Import));
            #endregion

            #region 事件 Evnets
            ImagesListBoxSelectionChangedEvent = new DelegateCommand(new Action<object>(ImagesListBoxSelectionChanged));
            #endregion
        }
    }
}
