/****************************************************************
                   Copyright © 2021 Aprheua

    File Name:     ViewModels/MainWindow.cs

    Author:        Chenhao Wang (MrBBBaiXue@github.com)
                   Boyan Wang (JingNianNian@github.com)

    Version:       2.3.3.3

    Date:          2021-03-11

    Description:   MainWindow 的后台绑定源

    Classes:       MainWindow : NotificationObject
                   // NotificationObject的继承，绑定处理窗口的数据，
                      处理主窗口大部分逻辑。

****************************************************************/

using System;
using System.Collections.ObjectModel;
using System.IO;
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
                RaisePropertyChanged("WindowTitle");
            }
        }

        private bool? _selectAllCheckBoxIsChecked;
        public bool? SelectAllCheckBoxIsChecked
        {
            get { return _selectAllCheckBoxIsChecked; }
            set
            {
                _selectAllCheckBoxIsChecked = value;
                RaisePropertyChanged("SelectAllCheckBoxIsChecked");
            }
        }

        private int _selectedIndex;
        public int SelectedIndex
        {
            get { return _selectedIndex; }
            set
            {
                _selectedIndex = value;
                RaisePropertyChanged("SelectedIndex");
                RaisePropertyChanged("SelectedImage");
                RaisePropertyChanged("ImageViewerPath");
            }
        }

        private bool _showBlockOverlayCheckBoxIsChecked;
        public bool ShowBlockOverlayCheckBoxIsChecked
        {
            get { return _showBlockOverlayCheckBoxIsChecked; }
            set
            {
                _showBlockOverlayCheckBoxIsChecked = value;
                RaisePropertyChanged("ShowBlockOverlayCheckBoxIsChecked");
                RaisePropertyChanged("ImageViewerPath");
            }
        }

        public Models.OriginImage SelectedImage => (SelectedIndex >= 0) ? SourceImages[SelectedIndex] : null;

        public string ImageViewerPath
        {
            get
            {
                if (SelectedImage != null)
                {
                    return (ShowBlockOverlayCheckBoxIsChecked) ? SourceImages[SelectedIndex].OverlayImagePath : SourceImages[SelectedIndex].Path;
                }
                return null;
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
                    var sourceImage = new Models.OriginImage(path, ListBoxItemCheckBoxClickEvent, RemoveImageClickEvent);
                    SourceImages.Add(sourceImage);
                }
                ListBoxItemCheckBoxClickEvent.Execute(this);
            }
        }

        public DelegateCommand AnalyseCommand { get; set; }
        public void Analyse(object parameter)
        {
            App.CreateAnalyseWindow();
            App.Log.Info("CreateAnalyseWindow returned");
        }
        #endregion

        #region 事件 Events
        public DelegateCommand SelectAllCheckBoxClickEvent { get; set; }
        public void SelectAllCheckBoxClick(object parameter)
        {
            foreach(var sourceImage in SourceImages)
            {
                sourceImage.IsSelected = (bool)SelectAllCheckBoxIsChecked;
            }
        }
        public DelegateCommand ListBoxItemCheckBoxClickEvent { get; set; }
        public void ListBoxItemCheckBoxClick(object parameter)
        {
            int selectedCount = 0;
            foreach (var sourceImage in SourceImages)
            {
                selectedCount += sourceImage.IsSelected ? 1 : 0;
            }
            if (selectedCount == 0)
            {
                SelectAllCheckBoxIsChecked = false;
                return;
            }
            else if (selectedCount == SourceImages.Count)
            {
                SelectAllCheckBoxIsChecked = true;
                return;
            }
            SelectAllCheckBoxIsChecked = null;
        }
        public DelegateCommand NightModeToggleButtonClickEvent { get; set; }
        public void NightModeToggleButtonClick(object parameter)
        {
            HandyControl.Themes.ThemeManager.Current.ApplicationTheme =
                (HandyControl.Themes.ThemeManager.Current.ApplicationTheme != HandyControl.Themes.ApplicationTheme.Dark) ?
                HandyControl.Themes.ApplicationTheme.Dark : HandyControl.Themes.ApplicationTheme.Light;
            App.Log.Info($"Current.ApplicationTheme changed to {HandyControl.Themes.ThemeManager.Current.ApplicationTheme}.");
        }
        public DelegateCommand AddCategoryClickEvent { get; set; }
        public void AddCategory(object parameter)
        {
            if (SourceImages.Count == 0) return;
            var categoryName = App.CreateAddCategoryWindow();
            if (!String.IsNullOrWhiteSpace(categoryName))
            {
                SelectedImage.AddCategory(Path.Combine(App.AprheuaCategoriesFolder, $"{categoryName} - {SelectedImage.Name}"), $"{categoryName}");
                App.Log.Info($"Add Category : {categoryName}");
                return;
            }
            App.Log.Error("Returned categoryName is NullOrWhiteSpace, will abort...");
        }
        public DelegateCommand RemoveImageClickEvent { get; set; }
        public void RemoveImageClick(object parameter)
        {
            var index = SelectedIndex;
            Models.Utility.DeleteFolder(System.IO.Path.Combine(App.AprheuaCategoriesFolder, SourceImages[index].Name));
            SourceImages.Remove(SourceImages[index]);
            ListBoxItemCheckBoxClickEvent.Execute(this);
        }

        #endregion

        public MainWindow()
        {
            #region 变量 Variables
            WindowTitle = $"Aprheua 脸谱分割展示程序 - {Environment.CurrentDirectory}";
            SelectAllCheckBoxIsChecked = false;
            #endregion

            #region 数据 Datas
            SourceImages = new ObservableCollection<Models.OriginImage>();
            #endregion

            #region 命令 Commands
            ImportCommand = new DelegateCommand(new Action<object>(Import));
            AnalyseCommand = new DelegateCommand(new Action<object>(Analyse));
            #endregion

            #region 事件 Events
            SelectAllCheckBoxClickEvent = new DelegateCommand(new Action<object>(SelectAllCheckBoxClick));
            ListBoxItemCheckBoxClickEvent = new DelegateCommand(new Action<object>(ListBoxItemCheckBoxClick));
            NightModeToggleButtonClickEvent = new DelegateCommand(new Action<object>(NightModeToggleButtonClick));
            AddCategoryClickEvent = new DelegateCommand(new Action<object>(AddCategory));
            RemoveImageClickEvent = new DelegateCommand(new Action<object>(RemoveImageClick));
            #endregion

            //ToDo : 测试用，添加默认图像
            var testImage = new Models.OriginImage(Path.Combine(App.AprheuaResourceFolder, "default-SelectedImage.png"), ListBoxItemCheckBoxClickEvent, RemoveImageClickEvent);
            for (int i =1; i <= 3; i++)
            {
                testImage.AddCategory(Path.Combine(App.AprheuaCategoriesFolder, testImage.Name, $"Test {i}"), $"Test {i}");
            }
            SourceImages.Add(testImage);
        }
    }
}
