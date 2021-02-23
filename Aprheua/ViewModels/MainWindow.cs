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

        private bool? _selectAllCheckBoxIsChecked;
        public bool? SelectAllCheckBoxIsChecked
        {
            get { return _selectAllCheckBoxIsChecked; }
            set
            {
                _selectAllCheckBoxIsChecked = value;
                this.RaisePropertyChanged("SelectAllCheckBoxIsChecked");
            }
        }

        private int _selectedIndex;
        public int SelectedIndex
        {
            get { return _selectedIndex; }
            set
            {
                _selectedIndex = value;
                this.RaisePropertyChanged("SelectedIndex");
                this.RaisePropertyChanged("SelectedImage");
                this.RaisePropertyChanged("ImageViewerPath");
            }
        }
        public Models.OriginImage SelectedImage => SourceImages[SelectedIndex];
        public string ImageViewerPath => (false) ? SourceImages[SelectedIndex].OverlayImagePath : SourceImages[SelectedIndex].Path;

        private HandyControl.Data.SkinType _currentSkin = 0;
        //SkinType : Default = 0, Dark = 1, Violet = 2.
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
                    var sourceImage = new Models.OriginImage(path, ListBoxItemCheckBoxClickEvent);
                    SourceImages.Add(sourceImage);
                }
            }

        }

        public DelegateCommand AnalyseCommand { get; set; }
        public void Analyse(object parameter)
        {

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
            _currentSkin = (_currentSkin == 0) ? HandyControl.Data.SkinType.Dark
                                               : HandyControl.Data.SkinType.Default;
            App.UpdateSkin(_currentSkin);           
        }
        #endregion

        public MainWindow()
        {
            #region 变量 Variables
            WindowTitle = $"Aprheua 脸谱分割展示程序 - {Environment.CurrentDirectory}";         
            SelectAllCheckBoxIsChecked = false;
            #endregion

            #region 数据 Datas
            SourceImages = new ObservableCollection<Models.OriginImage> { };
            #endregion

            #region 命令 Commands
            ImportCommand = new DelegateCommand(new Action<object>(Import));
            AnalyseCommand = new DelegateCommand(new Action<object>(Analyse));
            #endregion

            #region 事件 Events
            SelectAllCheckBoxClickEvent = new DelegateCommand(new Action<object>(SelectAllCheckBoxClick));
            ListBoxItemCheckBoxClickEvent = new DelegateCommand(new Action<object>(ListBoxItemCheckBoxClick));
            NightModeToggleButtonClickEvent = new DelegateCommand(new Action<object>(NightModeToggleButtonClick));
            #endregion

            //测试事件
            var testImage = new Models.OriginImage("E:\\GitHub\\Hi-Icy\\Aprheua\\build\\resources\\test.jpg", ListBoxItemCheckBoxClickEvent);
            for (int i =1; i <= 10; i++)
            {
                var cat = new Models.ImageCategory($"test - {i}");
                for (int j = 1; j <= 10; j++)
                {
                    var block = new Models.ImageBlock {
                        BlockPath = "E:\\GitHub\\Hi-Icy\\Aprheua\\build\\resources\\test.jpg"
                    };
                    cat.ImageBlocks.Add(block);
                }
                testImage.ImageCategories.Add(cat);
            }
            SourceImages.Add(testImage);
        }
    }
}
