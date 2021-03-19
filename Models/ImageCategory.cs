/****************************************************************
                   Copyright © 2021 Aprheua

    File Name:     Models/ImageCategory.cs

    Author:        Chenhao Wang (MrBBBaiXue@github.com)

    Version:       2.3.3.3

    Date:          2021-03-19

    Description:   图像分类 ImageCategory类

    Classes:       ImageCategory : NotificationObject
                   // NotificationObject的继承，包含ImageBlocks；
                      处理传入的图像类别，同时与前台进行间接的绑定。

****************************************************************/

using System;
using System.Collections.ObjectModel;
using Aprheua.ViewModels;

namespace Aprheua.Models
{
    /// <summary>
    /// 图像分类处理类
    /// </summary>
    public class ImageCategory : NotificationObject
    {
        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                RaisePropertyChanged("Name");
            }
        }

        public readonly string FolderPath;

        private int _snapCount;
        public ObservableCollection<ImageBlock> ImageBlocks { get; set; }
        public Commands.DelegateCommand RemoveCategoryClickEvent { get; set; }
        public Commands.DelegateCommand AddBlockClickEvent { get; set; }
        public Commands.DelegateCommand ScanNumberOfImageBlocksClickEvent { get; set; }
        /// <summary>
        /// 类的构造函数
        /// </summary>
        /// <param name="folderPath">图像分类路径</param>
        /// <param name="name">图像分类名称</param>
        /// <param name="removeCategoryClickEvent">删除图像分类的委托事件</param>
        /// <param name="scanNumberOfImageBlocksClickEvent">扫描图片块的委托事件</param>
        public ImageCategory(string folderPath,
                             string name,
                             Commands.DelegateCommand removeCategoryClickEvent,
                             Commands.DelegateCommand scanNumberOfImageBlocksClickEvent)
        {
            ImageBlocks = new ObservableCollection<ImageBlock>();
            Name = name;
            if (!System.IO.Directory.Exists(folderPath))
            {
                System.IO.Directory.CreateDirectory(folderPath);
            }
            FolderPath = folderPath;
            RemoveCategoryClickEvent = removeCategoryClickEvent;
            ScanNumberOfImageBlocksClickEvent = scanNumberOfImageBlocksClickEvent;
            AddBlockClickEvent = new Commands.DelegateCommand(new Action<object>(AddBlock));
            _snapCount = 0;
        }
        /// <summary>
        /// 创建图像块
        /// </summary>
        /// <param name="parameter">参数对象</param>
        /// <returns>无</returns>
        public void AddBlock(object parameter)
        {
            var outputPath = System.IO.Path.Combine(FolderPath, $"Snap-{_snapCount}.jpg");
            _snapCount++;
            App.Log.Info(outputPath);
            App.CreateClipWindow(App.MainWindowViewModel.SelectedImage.Path, outputPath);
            App.Log.Info($"AddBlockFromScreenShot for category : {Name} has been triggered!");
            ScanImages();
        }
        /// <summary>
        /// 扫描Category下的文件
        /// </summary>
        /// <returns>bool，表示是否有新图像块</returns>
        public bool ScanImages()
        {
            var hasNewFiles = false;
            App.Log.Info($"ScanImages in Category : {Name} has been triggered!");
            var files = System.IO.Directory
                                 .EnumerateFiles(FolderPath,"*.jpg");
                                 // 仅扫描JPEG文件
            foreach(var image in files)
            {
                if (!CheckIfAlreadyExists(image))
                {
                    // 检查图片是否已经存在
                    hasNewFiles = true;
                    AddBlockFromFile(image);
                }
            }
            return hasNewFiles;
        }
        private bool CheckIfAlreadyExists(string image)
        {
            if(ImageBlocks.Count > 0)
            {
                // 避免Out_Of_Range_Exception
                foreach (var imageBlock in ImageBlocks)
                {
                    if (System.IO.Path.GetFullPath(imageBlock.BlockPath).Equals(System.IO.Path.GetFullPath(image)))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        private void AddBlockFromFile(string path)
        {
            Commands.DelegateCommand removeRemoveBlockClickEvent = new Commands.DelegateCommand(new Action<object>((_) =>
            {
                // 使用Lambda表达式传递一个完整的Action。
                // 删除图像块的Action
                // 在这里调用Origin的图片块扫描
                foreach (var block in ImageBlocks)
                {
                    if (block.BlockPath == path)
                    {
                        var deleteBlockPath = block.BlockPath;
                        ImageBlocks.Remove(block);
                        GC.Collect();
                        System.IO.File.Delete(deleteBlockPath);
                        ScanNumberOfImageBlocksClickEvent.Execute(null);
                        break;
                    }
                }
            }));
            var imageBlock = new Models.ImageBlock(path, removeRemoveBlockClickEvent);
            ImageBlocks.Add(imageBlock);
            ScanNumberOfImageBlocksClickEvent.Execute(null);
            App.Log.Info($"ImageBlock {path} has been added to {Name}");
        }
    }
}
