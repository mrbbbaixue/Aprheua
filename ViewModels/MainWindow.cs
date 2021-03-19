/****************************************************************
                   Copyright © 2021 Aprheua

    File Name:     ViewModels/MainWindow.cs

    Author:        Chenhao Wang (MrBBBaiXue@github.com)
                   Boyan Wang (JingNianNian@github.com)

    Version:       1.0.0.0

    Date:          2021-03-19

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
                // bool？值可以为null，对应checkbox的三个属性：勾选，不选，半选。
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
                // 每次切换到新图片的时候会将是否选择分割块重置，防止用户困惑
                ShowBlockOverlayCheckBoxIsChecked = false;
                RaisePropertyChanged("ShowBlockOverlayCheckBoxIsChecked");
            }
        }

        private bool _showBlockOverlayCheckBoxIsChecked;
        public bool ShowBlockOverlayCheckBoxIsChecked
        {
            get { return _showBlockOverlayCheckBoxIsChecked; }
            set
            {
                _showBlockOverlayCheckBoxIsChecked = value;
                // 每当显示分割块的checkbox改变时，应该通知UI图片浏览器的链接已经改变
                RaisePropertyChanged("ShowBlockOverlayCheckBoxIsChecked");
                RaisePropertyChanged("ImageViewerPath");
            }
        }
        public Models.OriginImage SelectedImage => (SourceImages.Count > 0) ? SourceImages[SelectedIndex] : null;
        public string ImageViewerPath => (SelectedImage != null) ?
            ((ShowBlockOverlayCheckBoxIsChecked) ? SelectedImage.OverlayImagePath : SelectedImage.Path) :
            App.AprheuaDefaultImage;
        // 三目，当当前选中图片的时候查询“显示分割块”是否已经勾选，如果是，返回分割块路径，否，返回原图路径
        #endregion

        #region 数据 Datas
        public ObservableCollection<Models.OriginImage> SourceImages { get; set; }
        #endregion

        #region 命令 Commands
        public DelegateCommand ImportCommand { get; set; }
        public void Import(object parameter)
        {
            string[] paths;
            // 打开文件（允许多选）
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
                    // 向SourceImages中逐个添加SourceImage对象。
                    var sourceImage = new Models.OriginImage(path, ListBoxItemCheckBoxClickEvent, RemoveImageClickEvent);
                    SourceImages.Add(sourceImage);
                }
                SelectedIndex = 0;
                // 检查全选按钮的情况
                ListBoxItemCheckBoxClickEvent.Execute(this);
            }
            GC.Collect();
        }

        public DelegateCommand AnalyseCommand { get; set; }
        public void Analyse(object parameter)
        {
            int selectedCount = 0;
            bool isOnlyCurrentImage = false;
            foreach (var sourceImage in SourceImages)
            {
                selectedCount += sourceImage.IsSelected ? 1 : 0;
            }
            // 统计被选择的图像的数量
            if (selectedCount == 0)
            {
                // 目前似乎用户没有使用多选功能
                // 那么应该自动选择当前查看的图片，在分析完成之后再取消勾选
                if (SourceImages.Count == 0)
                {
                    return;
                }
                SourceImages[SelectedIndex].IsSelected = true;
                isOnlyCurrentImage = true;
                App.Log.Info($"isOnlyCurrentImage : {isOnlyCurrentImage}.");
            }
            App.CreateAnalyseWindow();
            if (isOnlyCurrentImage)
            {
                SourceImages[SelectedIndex].IsSelected = false;
                // 取消勾选
                App.Log.Info("CurrentImage unselected!");
            }
            App.Log.Info("CreateAnalyseWindow process completed.");
            // 完整垃圾清理
            GC.Collect();
        }

        public DelegateCommand ExportCommand { get; set; }
        public void Export(object parameter)
        {
            // 导出事件
            var dialog = new System.Windows.Forms.FolderBrowserDialog
            {
                Description = "选择导出文件夹 :"
            };
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (string.IsNullOrEmpty(dialog.SelectedPath))
                {
                    return;
                }
                // 复制文件夹到目标位置
                var sourcePath = App.AprheuaCategoriesFolder;
                var targetPath = Path.Combine(dialog.SelectedPath,$"Aprheua-Export-{Models.Utility.GetTimeStamp()}");
                Models.Utility.CopyFolder(sourcePath, targetPath);
            }
        }
        #endregion

        #region 事件 Events
        public DelegateCommand SelectAllCheckBoxClickEvent { get; set; }
        public void SelectAllCheckBoxClick(object parameter)
        {
            // 处理图像全选事件
            foreach(var sourceImage in SourceImages)
            {
                sourceImage.IsSelected = (bool)SelectAllCheckBoxIsChecked;
            }
        }
        public DelegateCommand ListBoxItemCheckBoxClickEvent { get; set; }
        public void ListBoxItemCheckBoxClick(object parameter)
        {
            // 单个图像是否被选择的检查事件
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
            // 切换明亮/黑暗模式
            HandyControl.Themes.ThemeManager.Current.ApplicationTheme =
                (HandyControl.Themes.ThemeManager.Current.ApplicationTheme != HandyControl.Themes.ApplicationTheme.Dark) ?
                HandyControl.Themes.ApplicationTheme.Dark : HandyControl.Themes.ApplicationTheme.Light;
            App.Log.Info($"Current.ApplicationTheme changed to {HandyControl.Themes.ThemeManager.Current.ApplicationTheme}.");
        }
        public DelegateCommand AddCategoryClickEvent { get; set; }
        public void AddCategory(object parameter)
        {
            if (SourceImages.Count == 0)
            {
                return;
            }
            // 判断图像是否为空，如果为空，则按钮不应该被触发
            var categoryName = App.CreateAddCategoryWindow();
            if (!String.IsNullOrWhiteSpace(categoryName))
            {
                // 判断创建分类的返回值是否为空，如果是空则报错
                SelectedImage.AddCategory(Path.Combine(App.AprheuaCategoriesFolder, SelectedImage.Name, categoryName), categoryName);
                App.Log.Info($"Add Category : {categoryName}");
                return;
            }
            App.Log.Error("Returned categoryName is NullOrWhiteSpace, will abort...");
        }
        public DelegateCommand RemoveImageClickEvent { get; set; }
        public void RemoveImageClick(object parameter)
        {
            var index = SelectedIndex;
            // 删除后Index超出索引范围的处理方案：
            // 如果是第一个图像被删除，则当前默认选中下一张图片
            // 如果是最后一个图像被删除，则当前默认选中上一张图片
            // 如果是超过3张图片的列表，则当前默认选中下一张图片
            // 如果只剩下一张图片，就直接删除，还原初始状态
            if (index == 0 && SourceImages.Count > 1)
            {
                SelectedIndex++;
            }
            else if (index == SourceImages.Count - 1 && SourceImages.Count > 1)
            {
                SelectedIndex--;
            }
            else if (SourceImages.Count - 1 > 0)
            {
                SelectedIndex++;
            }
            Models.Utility.DeleteFolder(System.IO.Path.Combine(App.AprheuaCategoriesFolder, SourceImages[index].Name));
            SourceImages.Remove(SourceImages[index]);
            // 在删除图片之后更新多选框的状态
            ListBoxItemCheckBoxClickEvent.Execute(this);
            ShowBlockOverlayCheckBoxIsChecked = false;
            // 手动触发GC
            GC.Collect();
        }

        #endregion

        public MainWindow()
        {
            // 初始化ViewModel的属性
            #region 变量 Variables
            //更改程序标题
            WindowTitle = $"Aprheua HAAR Classifier GUI Program - {Environment.CurrentDirectory}";
            SelectAllCheckBoxIsChecked = false;
            SelectedIndex = 0;
            #endregion

            #region 数据 Datas
            SourceImages = new ObservableCollection<Models.OriginImage>();
            #endregion

            #region 命令 Commands
            ImportCommand = new DelegateCommand(new Action<object>(Import));
            AnalyseCommand = new DelegateCommand(new Action<object>(Analyse));
            ExportCommand = new DelegateCommand(new Action<object>(Export));
            #endregion

            #region 事件 Events
            SelectAllCheckBoxClickEvent = new DelegateCommand(new Action<object>(SelectAllCheckBoxClick));
            ListBoxItemCheckBoxClickEvent = new DelegateCommand(new Action<object>(ListBoxItemCheckBoxClick));
            NightModeToggleButtonClickEvent = new DelegateCommand(new Action<object>(NightModeToggleButtonClick));
            // 使用委托方式，将Model中的元素直接与主UI线程绑定
            AddCategoryClickEvent = new DelegateCommand(new Action<object>(AddCategory));
            RemoveImageClickEvent = new DelegateCommand(new Action<object>(RemoveImageClick));
            #endregion    
        }
    }
}
