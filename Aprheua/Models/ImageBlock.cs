/****************************************************************
                   Copyright © 2021 Aprheua

    File Name:     Models/ImageBlock.cs

    Author:        Chenhao Wang (MrBBBaiXue@github.com)

    Version:       2.3.3.3

    Date:          2021-02-27

    Description:   图像块 ImageBlock类

    Classes:       ImageBlock : ViewModels.NotificationObject
                   // NotificationObject的继承，包含图片块的路径；
                      为前台ListBox的ItemsSource，同时与前台进行间
                      接的绑定。

****************************************************************/

namespace Aprheua.Models
{
    public class ImageBlock : ViewModels.NotificationObject
    {
        private string _blockPath;
        public string BlockPath
        {
            get { return _blockPath; }
            set
            {
                _blockPath = value;
                RaisePropertyChanged("BlockPath");
            }
        }
        public Commands.DelegateCommand RemoveBlockClickEvent { get; set; }
        public ImageBlock(string blockPath, Commands.DelegateCommand removeBlockClickEvent)
        {
            RemoveBlockClickEvent = removeBlockClickEvent;
            BlockPath = blockPath;
        }
    }
}
