/****************************************************************
                   Copyright © 2021 Aprheua

    File Name:     Models/OriginImage.cs

    Author:        Chenhao Wang (MrBBBaiXue@github.com)

    Version:       1.0.0.0

    Date:          2021-03-19

    Description:   待分类图像 OriginImage类

    Classes:       OriginImage : NotificationObject
                   // NotificationObject的继承，包含ImageCategories；
                      处理传入的图像数据，图像类型分类和图片块。同时与前台
                      进行间接的绑定。

****************************************************************/

using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Aprheua.ViewModels;

namespace Aprheua.Models
{
    /// <summary>
    /// 图像文件处理类
    /// </summary>
    public class OriginImage : NotificationObject
    {
        public string Path { get; set; }
        public string Name { get; set; }

        private string _thumbImagePath;
        public string ThumbImagePath
        {
            get { return _thumbImagePath; }
            set
            {
                _thumbImagePath = value;
                RaisePropertyChanged("ThumbImagePath");
            }
        }

        private string _overlayImagePath;
        public string OverlayImagePath
        {
            get { return _overlayImagePath; }
            set
            {
                _overlayImagePath = value;
                RaisePropertyChanged("OverlayImagePath");
            }
        }

        private int _numberOfImageBlocks;
        public int NumberOfImageBlocks
        {
            get { return _numberOfImageBlocks; }
            set
            {
                _numberOfImageBlocks = value;
                RaisePropertyChanged("NumberOfBlocks");
            }
        }

        private bool _isSelected;
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value;
                RaisePropertyChanged("IsSelected");
            }
        }
        // 该图片是否在左侧列表中被选中
        public Commands.DelegateCommand CheckBoxClickEvent { get; set; }
        public Commands.DelegateCommand RemoveImageClickEvent { get; set; }
        public Commands.DelegateCommand OpenInExplorerClickEvent { get; set; }
        public Commands.DelegateCommand ScanNumberOfImageBlocksClickEvent { get; set; }
        public void OpenInExplorerClick(object parameter)
            => System.Diagnostics.Process.Start("explorer", Path);
        // 使用文件资源管理器打开
        public ObservableCollection<ImageCategory> ImageCategories { get; set; }
        // 与UI同步的图像分类

        /// <summary>
        /// 类的构造函数
        /// </summary>
        /// <param name="path">图像路径</param>
        /// <param name="checkBoxClickEvent">点击左侧CheckBox的事件</param>
        /// <param name="removeImageClickEvent">删除图片的事件</param>
        public OriginImage(string path, Commands.DelegateCommand checkBoxClickEvent, Commands.DelegateCommand removeImageClickEvent)
        {
            Path = System.IO.Path.GetFullPath(path);
            Name = System.IO.Path.GetFileName(path);
            ThumbImagePath = System.IO.Path.Combine(App.AprheuaThumbImagesFolder, $"thumb-{Utility.GetTimeStamp()}-{Name}");
            OverlayImagePath = System.IO.Path.Combine(App.AprheuaOverlayImagesFolder, $"overlay-{Utility.GetTimeStamp()}-{Name}");
            NumberOfImageBlocks = 0;
            IsSelected = false;
            ImageCategories = new ObservableCollection<ImageCategory>();
            GetReducedImageAsync();
            // 异步获取文件的缩略图，减小内存占用
            CheckBoxClickEvent = checkBoxClickEvent;
            RemoveImageClickEvent = removeImageClickEvent;
            // 构造器接受DelegateCommand
            OpenInExplorerClickEvent = new Commands.DelegateCommand(new Action<object>(OpenInExplorerClick));
            ScanNumberOfImageBlocksClickEvent = new Commands.DelegateCommand(new Action<object>(ScanNumberOfImageBlocks));
        }
        /// <summary>
        /// 创建图像分类
        /// </summary>
        /// <param name="folderPath">图像分类的文件夹位置</param>
        /// <param name="name">图像分类的名称</param>
        /// <returns>int 返回当前图像分类在图像分类中的位置(index)</returns>
        public int AddCategory(string folderPath, string name)
        {
            Commands.DelegateCommand removeCategoryClickEvent = new Commands.DelegateCommand(new Action<object>((_) =>
            {
                // 使用Lambda表达式直接传递Action
                foreach(var imageCategory in ImageCategories)
                {
                    if (imageCategory.Name == name)
                    {
                        var deleteFolderPath = imageCategory.FolderPath;
                        ImageCategories.Remove(imageCategory);
                        GC.Collect();
                        Utility.DeleteFolder(deleteFolderPath);
                        break;
                    }
                }
            }));
            var category = new ImageCategory(folderPath, name, removeCategoryClickEvent, ScanNumberOfImageBlocksClickEvent);
            category.ScanImages();
            ImageCategories.Add(category);
            return ImageCategories.IndexOf(category);
        }
        /// <summary>
        /// 扫描图片块数量
        /// </summary>
        /// <returns>无</returns>
        public void ScanNumberOfImageBlocks(object parameter)
        {
            NumberOfImageBlocks = 0;
            foreach(var imageCategory in ImageCategories)
            {
                 NumberOfImageBlocks += imageCategory.ImageBlocks.Count;
                App.Log.Info($"imageCategory {imageCategory.Name} got {imageCategory.ImageBlocks.Count} new Blocks.");
            }
            RaisePropertyChanged("NumberOfImageBlocks");
        }
        /// <summary>
        /// 异步获得图片块
        /// </summary>
        /// <returns>无</returns>
        private async void GetReducedImageAsync()
        {
            Task GetReducedImageTask()
            {
                return Task.Run(() =>
                {
                    // 在完成后Dispose文件，防止占用
                    using (var thumbImage = new ThumbImage(Path))
                    {
                        thumbImage.GetReducedImage(50, ThumbImagePath);
                        RaisePropertyChanged("ThumbImagePath");
                    }
                });
            }
            await GetReducedImageTask().ConfigureAwait(false);
        }
    }
}
